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
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Defaults;
using Serilog;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// Embedded iframe controller.
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
        /// <param name="projectService">The Project service.</param>
        /// <param name="userService">The User service.</param>
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
        /// <returns>A list of embedded projects Resource Result.</returns>
        /// <response code="200">Returns a list of embedded project resource results.</response>
        /// <response code="404">If there are no embedded project resource results.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EmbeddedProjectResourceResult>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
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
        /// <returns>The project resource result.</returns>
        /// <response code="200">Returns project resource results with the specified guid.</response>
        /// <response code="400">If the guid is not specified.</response>
        /// <response code="404">If no project could be found with the specified guid.</response>
        [HttpGet("{guid}")]
        [ProducesResponseType(typeof(ProjectResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetEmbeddedProject(string guid)
        {
            if(string.IsNullOrEmpty(guid))
            {
                ProblemDetails problem = new ProblemDetails
                 {
                     Title = "No Guid specified.",
                     Detail = "There was no guid specified.",
                     Instance = "DA33DBE1-55DC-4574-B65F-C7A76A7309CF"
                 };
                return BadRequest(problem);
            }

            Guid validGuid;
            try
            {
                validGuid = new Guid(guid);
            }
            catch(FormatException e)
            {
                Log.Logger.Error(e,"Guid format error");

                ProblemDetails problem = new ProblemDetails
                 {
                     Title = "The given guid was not a valid guid.",
                     Detail = "The format of the guid was not valid, see  https://github.com/DigitalExcellence/dex-backend/wiki/Specific-error-details for a detailed explanation.",
                     Instance = "DA33DBE1-55DC-4574-B62F-C7B76A7309CF"
                 };
                return NotFound(problem);
            }

            EmbeddedProject embeddedProject = await embedService.FindAsync(validGuid).ConfigureAwait(false);
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
            Project project = await projectService.FindWithUserAndCollaboratorsAsync(embeddedProject.ProjectId);
            return Ok(mapper.Map<Project, ProjectResourceResult>(project));
        }

        /// <summary>
        /// Creates a embedded project.
        /// </summary>
        /// <param name="embedResource">EmbedResource.</param>
        /// <returns>The embedded project resource result.</returns>
        /// <response code="201">Returns the created embedded project resource result.</response>
        /// <response code="400">Unable to create the embedded project.</response>
        /// <response code="401">If the user is not allowed to create an embedded project.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(EmbeddedProjectResourceResult), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
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

            string identity = HttpContext.User.GetIdentityId(HttpContext);
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

            //Ensure we have a non existing Guid.
            Guid guid;
            while(true)
            {
                guid = Guid.NewGuid();
                if(!await embedService.IsNonExistingGuidAsync(guid)) continue;
                break;
            }
            embeddedProject.Guid = guid;
            embeddedProject.User = user;

            try
            {
                embedService.Add(embeddedProject);
                embedService.Save();
                return Created(nameof(CreateEmbeddedProject), mapper.Map<EmbeddedProject, EmbeddedProjectResourceResult>(embeddedProject));
            } catch(DbUpdateException e)
            {
                Log.Logger.Error(e,"Database exception");

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
        /// <returns>Status code 200.</returns>
        /// <response code="200">Returns status code 200. The embedded project is deleted.</response>
        /// <response code="401">If the user is not allowed to delete the embedded project.</response>
        /// <response code="404">If the embedded project could not be found with the specified guid.</response>
        [HttpDelete("{guid}")]
        [Authorize]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
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

            string identity = HttpContext.User.GetIdentityId(HttpContext);
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
