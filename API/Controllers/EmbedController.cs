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

using API.Common;
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
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{

    /// <summary>
    ///     This class is responsible for handling HTTP requests that are related
    ///     to the embedded projects, for example creating, retrieving or deleting.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class EmbedController : ControllerBase
    {

        private readonly IAuthorizationHelper authorizationHelper;
        private readonly IEmbedService embedService;
        private readonly IMapper mapper;
        private readonly IProjectService projectService;
        private readonly IUserService userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EmbedController" /> class
        /// </summary>
        /// <param name="embedService">The embed service which is used to communicate with the logic layer.</param>
        /// <param name="mapper">The mapper which is used to convert the resources to the models to the resource results.</param>
        /// <param name="projectService">The project service which is used to communicate with the logic layer.</param>
        /// <param name="userService">The user service which is used to communicate with the logic layer.</param>
        /// <param name="authorizationHelper">
        ///     The authorization helper which is used to communicate with the authorization helper
        ///     class.
        /// </param>
        public EmbedController(IEmbedService embedService,
                               IMapper mapper,
                               IProjectService projectService,
                               IUserService userService,
                               IAuthorizationHelper authorizationHelper)
        {
            this.embedService = embedService;
            this.mapper = mapper;
            this.projectService = projectService;
            this.userService = userService;
            this.authorizationHelper = authorizationHelper;
        }

        /// <summary>
        ///     This method is responsible for retrieving all embedded projects.
        /// </summary>
        /// <returns>This method returns a list of embedded projects resource result.</returns>
        /// <response code="200">This endpoint returns a list with embedded projects.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EmbeddedProjectResourceResult>), (int) HttpStatusCode.OK)]
        [Authorize(Policy = nameof(Defaults.Scopes.EmbedRead))]
        public async Task<IActionResult> GetAllEmbeddedProjects()
        {
            IEnumerable<EmbeddedProject> embeddedProjects = await embedService.GetEmbeddedProjectsAsync();

            return Ok(
                mapper.Map<IEnumerable<EmbeddedProject>, IEnumerable<EmbeddedProjectResourceResult>>(embeddedProjects));
        }

        /// <summary>
        ///     This method is responsible for retrieving a single embedded project.
        /// </summary>
        /// <param name="guid">The unique identifier which is used for searching the embedded project.</param>
        /// <returns>This method returns the project resource result.</returns>
        /// <response code="200">This endpoint returns an embedded project with the specified guid.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the guid is not specified.</response>
        /// <response code="404">
        ///     The 404 Not Found status code is returned when no project could be
        ///     found with the specified guid.
        /// </response>
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
            } catch(FormatException e)
            {
                Log.Logger.Error(e, "Guid format error");

                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "The given guid was not a valid guid.",
                                             Detail =
                                                 "The format of the guid was not valid, see  https://github.com/DigitalExcellence/dex-backend/wiki/Specific-error-details for a detailed explanation.",
                                             Instance = "DA33DBE1-55DC-4574-B62F-C7B76A7309CF"
                                         };
                return NotFound(problem);
            }

            EmbeddedProject embeddedProject = await embedService.FindAsync(validGuid)
                                                                .ConfigureAwait(false);
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
            Project project = await projectService.FindWithUserCollaboratorsAndInstitutionsAsync(embeddedProject.ProjectId);
            return Ok(mapper.Map<Project, ProjectResourceResult>(project));
        }

        /// <summary>
        ///     This method is responsible for creating an embedded project.
        /// </summary>
        /// <param name="embedResource">The embed resource which is used to create an embedded project</param>
        /// <returns>This method return the embedded project resource result.</returns>
        /// <response code="201">This endpoint returns the created embedded project.</response>
        /// <response code="400">
        ///     The 400 Bad Request status code is returned when the specified
        ///     resource is invalid or the project could not be saved to the database.
        /// </response>
        /// <response code="401">
        ///     The 401 Unauthorized status code is returned when the user
        ///     is not allowed to create an embed project.
        /// </response>
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
            User user = await userService.GetUserByIdentityIdAsync(identity);
            bool isAllowed = userService.UserHasScope(identity, nameof(Defaults.Scopes.EmbedWrite));

            if(!(project.UserId == user.Id || isAllowed))
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "User is not allowed to create an embed project.",
                                             Detail =
                                                 "The user does not own the project and does not have enough privileges to add an embed project.",
                                             Instance = "D6E83BEC-D9FA-4C86-9FA7-7D74DE0F5B23"
                                         };
                return Unauthorized(problem);
            }

            //Ensure we have a non existing Guid
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
                return Created(nameof(CreateEmbeddedProject),
                               mapper.Map<EmbeddedProject, EmbeddedProjectResourceResult>(embeddedProject));
            } catch(DbUpdateException e)
            {
                Log.Logger.Error(e, "Database exception");

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
        ///     This method is responsible for deleting the embedded project.
        /// </summary>
        /// <param name="guid">The unique identifier which is used for searching the embedded project.</param>
        /// <returns>This method returns status code 200.</returns>
        /// <response code="200">This endpoint returns status code 200. The embedded project is deleted.</response>
        /// <response code="401">
        ///     The 401 Unauthorized status code is returned when the user
        ///     is not allowed to delete the embedded project .
        /// </response>
        /// <response code="404">
        ///     The 404 Not Found status code is returned when the embedded
        ///     project could not be found with the specified guid.
        /// </response>
        [HttpDelete("{guid}")]
        [Authorize]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> DeleteEmbeddedProject(string guid)
        {
            EmbeddedProject embeddedProject = await embedService.FindAsync(new Guid(guid));
            if(embeddedProject == null)
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
            User user = await userService.GetUserByIdentityIdAsync(identity);
            bool isAllowed = await authorizationHelper.UserIsAllowed(user,
                                                                     nameof(Defaults.Scopes.EmbedWrite),
                                                                     nameof(Defaults.Scopes.InstitutionEmbedWrite),
                                                                     embeddedProject.UserId);

            if(!(embeddedProject.User.IdentityId == identity || isAllowed))
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "User is not allowed to delete the embedded project.",
                                             Detail =
                                                 "The user does not own the project and does not have enough privileges to delete an embed project.",
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
