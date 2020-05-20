/*
* Digital Excellence Copyright (C) 2020 Brend Smits
* 
* This program is free software: you can redistribute it and/or modify 
* it under the terms of the GNU Lesser General Public License as published 
* by the Free Software Foundation version 3 of the License.
* 
* This program is distributed in the hope that it will be useful, 
* but WITHOUT ANY WARRANTY; without even the implied warranty 
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
* See the GNU Lesser General Public License for more details.
* 
* You can find a copy of the GNU Lesser General Public License 
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using API.Extensions;
using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Defaults;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{

    /// <summary>
    /// Embedded iframe controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class EmbedController : ControllerBase
    {

        private readonly IEmbedService embedService;
        private readonly IMapper mapper;
        private readonly IProjectService projectService;
        private readonly IUserService userService;
        /// <summary>
        /// Initializes a new instance of the <see cref="EmbedController"/> class.
        /// </summary>
        /// <param name="embedService">The embed service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="projectService"></param>
        /// <param name="userService"></param>
        public EmbedController(IEmbedService embedService, IMapper mapper, IProjectService projectService, IUserService userService)
        {
            this.embedService = embedService;
            this.mapper = mapper;
            this.projectService = projectService;
            this.userService = userService;
        }


        /// <summary>
        /// Gets all embedded projects.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = nameof(Defaults.Scopes.EmbedRead))]
        public async Task<IActionResult> GetAllEmbeddedProjects()
        {
            IEnumerable<EmbeddedProject> embeddedProjects = await embedService.GetEmbeddedProjectsAsync();

            if(!embeddedProjects.Any())
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "No Embedded Projects found.",
                    Detail = "There are no Embedded projects in the database.",
                    Instance = "FEA62EAE-3D3C-4CE7-BDD8-6B273D56068D"
                };
                return NotFound(problem);
            }
            return Ok(mapper.Map<IEnumerable<EmbeddedProject>, IEnumerable<EmbeddedProjectResourceResult>>(embeddedProjects));

        }

        /// <summary>
        /// Gets the embedded project.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        public async Task<IActionResult> GetEmbeddedProject(string guid)
        {

            EmbeddedProject embeddedProject = await embedService.FindAsync(new Guid(guid));
            if(embeddedProject == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "No Embedded Project found.",
                    Detail = "There is no embedded project with this GUID.",
                    Instance = "DA33DBE1-55DC-4574-B62F-C7A76A7309CF"
                };
                return NotFound(problem);
            }
            return Ok(mapper.Map<Project, ProjectResourceResult>(embeddedProject.Project));
        }

        /// <summary>
        ///     Creates a embedded project
        /// </summary>
        /// <param name="embedResource"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateEmbeddedProject(EmbeddedProjectResource embedResource)
        {
            if(embedResource == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "the embed resource is not valid.",
                    Detail = "The embed resource is null.",
                    Instance = "48C4A6DD-30AD-434F-BE98-694AA9F80140"
                };
                return BadRequest(problem);
            }
            EmbeddedProject embeddedProject = mapper.Map<EmbeddedProjectResource, EmbeddedProject>(embedResource);

            Project project = await projectService.FindAsync(embedResource.ProjectId);
            if(project == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Project does not exist.",
                    Detail = "There is no project with this project ID.",
                    Instance = "644FE34C-FC98-4BE9-8BB7-D0773409F636"
                };
                return BadRequest(problem);
            }

            string identity = HttpContext.User.GetStudentId(HttpContext);
            bool isAllowed = userService.UserHasScope(identity, nameof(Defaults.Scopes.EmbedWrite));
            User user = await userService.GetUserByIdentityIdAsync(identity);

            if(!(project.UserId == user.Id || isAllowed))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "User is not allowed to create an embed project.",
                    Detail = "The user does not own the project and does not have enough privileges to add an embed project.",
                    Instance = "D6E83BEC-D9FA-4C86-9FA7-7D74DE0F5B23"
                };
                return Unauthorized(problem);
            }

            //Ensure we have a non existing Guid
            Guid guid;
            while(true)
            {
                guid = Guid.NewGuid();
                if(await embedService.IsNonExistingGuid(guid))
                {
                    break;
                }
            }
            embeddedProject.Guid = guid;
            embeddedProject.User = user;


            try
            {
                embedService.Add(embeddedProject);
                embedService.Save();
                return Created(nameof(CreateEmbeddedProject), mapper.Map<EmbeddedProject, EmbeddedProjectResourceResult>(embeddedProject));
            } catch
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Could not create the Embedded project.",
                    Detail = "The database failed to save the embed project.",
                    Instance = "D481A8DD-B507-4AC5-A2CB-16EBEF758097"
                };
                return BadRequest(problem);
            }
        }

        /// <summary>
        /// Deletes the embeddedProject.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        [HttpDelete("{guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteEmbeddedProject(string guid)
        {
            EmbeddedProject embeddedProject = await embedService.FindAsync(new Guid(guid));
            if( embeddedProject == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Embedded project not found.",
                    Detail = "There was no embedded project found with this GUID.",
                    Instance = "35730158-1DED-4767-9C70-253C7A975715"
                };
                return NotFound(problem);
            }

            
            string identity = HttpContext.User.GetStudentId(HttpContext);
            bool isAllowed = userService.UserHasScope(identity, nameof(Defaults.Scopes.EmbedWrite));

            if(!(embeddedProject.User.IdentityId == identity || isAllowed))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "User is not allowed to delete the embedded project.",
                    Detail = "The user does not own the project and does not have enough privileges to delete an embed project.",
                    Instance = "35730158-1DED-4767-9C70-253C7A975715"
                };
                return Unauthorized(problem);
            }

            await embedService.RemoveAsync(embeddedProject.Id);
            embedService.Save();
            return Ok();
        }

    }

}
