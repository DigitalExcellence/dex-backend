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
using API.HelperClasses;
using API.Resources;
using AutoMapper;
using MessageBrokerPublisher.HelperClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Defaults;
using Serilog;
using Services.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using File = Models.File;

namespace API.Controllers
{

    /// <summary>
    ///     This class is responsible for handling HTTP requests that are related
    ///     to the projects, for example creating, retrieving, updating or deleting.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {

        private readonly IAuthorizationHelper authorizationHelper;
        private readonly ICallToActionOptionService callToActionOptionService;
        private readonly IFileService fileService;
        private readonly IFileUploader fileUploader;
        private readonly IMapper mapper;
        private readonly IProjectService projectService;
        private readonly IUserProjectLikeService userProjectLikeService;
        private readonly IUserProjectService userProjectService;
        private readonly IUserService userService;
        private readonly IEmailSender emailSender;
        private readonly ICollaboratorLinkRequestService collaboratorLinkRequestService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectController" /> class
        /// </summary>
        /// <param name="projectService">The project service which is used to communicate with the logic layer.</param>
        /// <param name="userService">The user service which is used to communicate with the logic layer.</param>
        /// <param name="mapper">The mapper which is used to convert the resources to the models to the resource results.</param>
        /// <param name="fileService">The file service which is used to communicate with the logic layer.</param>
        /// <param name="userProjectLikeService">
        ///     The service that handles liked project by users which is used to communicate with
        ///     the logic layer.
        /// </param>
        /// <param name="authorizationHelper">
        ///     The authorization helper which is used to communicate with the authorization helper
        ///     class.
        /// </param>
        /// <param name="fileUploader">The file uploader service is used to upload the files into the file system</param>
        /// <param name="userProjectService">
        ///     The user project service is responsible for users that are following / liking
        ///     projects.
        /// </param>
        /// <param name="callToActionOptionService">The call to action option service is used to communicate with the logic layer.</param>
        /// <param name="emailSender">Something something something</param>
        /// <param name="collaboratorLinkRequestService">something something</param>
        public ProjectController(IProjectService projectService,
                                 IUserService userService,
                                 IMapper mapper,
                                 IFileService fileService,
                                 IUserProjectLikeService userProjectLikeService,
                                 IAuthorizationHelper authorizationHelper,
                                 IFileUploader fileUploader,
                                 IUserProjectService userProjectService,
                                 ICallToActionOptionService callToActionOptionService,
                                 IEmailSender emailSender,
                                 ICollaboratorLinkRequestService collaboratorLinkRequestService)
        {
            this.projectService = projectService;
            this.userService = userService;
            this.userProjectLikeService = userProjectLikeService;
            this.fileService = fileService;
            this.fileUploader = fileUploader;
            this.mapper = mapper;
            this.authorizationHelper = authorizationHelper;
            this.userProjectService = userProjectService;
            this.callToActionOptionService = callToActionOptionService;
            this.emailSender = emailSender;
            this.collaboratorLinkRequestService = collaboratorLinkRequestService;
        }

        /// <summary>
        ///     This method is responsible for retrieving all projects.
        /// </summary>
        /// <param name="projectFilterParamsResource">The parameters to filter which is used to sort and paginate the projects.</param>
        /// <returns>This method returns the project result resource.</returns>
        /// <response code="200">This endpoint returns all projects.</response>
        /// <response code="400">
        ///     The 400 Bad Request status code is returned when values inside project filter params resource are
        ///     invalid.
        /// </response>
        [HttpGet]
        [ProducesResponseType(typeof(ProjectResultsResource), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllProjects(
            [FromQuery] ProjectFilterParamsResource projectFilterParamsResource)
        {
            ProblemDetails problem = new ProblemDetails
                                     {
                                         Title = "Invalid search request."
                                     };
            if(projectFilterParamsResource.Page != null &&
               projectFilterParamsResource.Page < 1)
            {
                problem.Detail = "The page number cannot be smaller then 1.";
                problem.Instance = "65EB6EF1-2CF4-4F7B-8A0A-C047C701337A";
                return BadRequest(problem);
            }
            if(projectFilterParamsResource.SortBy != null &&
               projectFilterParamsResource.SortBy != "name" &&
               projectFilterParamsResource.SortBy != "created" &&
               projectFilterParamsResource.SortBy != "updated")
            {
                problem.Detail = "Invalid sort value: Use \"name\", \"created\" or \"updated\".";
                problem.Instance = "5CE2F569-C0D5-4179-9299-62916270A058";
                return BadRequest(problem);
            }
            if(projectFilterParamsResource.SortDirection != null &&
               projectFilterParamsResource.SortDirection != "asc" &&
               projectFilterParamsResource.SortDirection != "desc")
            {
                problem.Detail = "Invalid sort direction: Use \"asc\" or \"desc\".";
                problem.Instance = "3EE043D5-070B-443A-A951-B252A1BB8EF9";
                return BadRequest(problem);
            }

            ProjectFilterParams projectFilterParams =
                mapper.Map<ProjectFilterParamsResource, ProjectFilterParams>(projectFilterParamsResource);

            IEnumerable<Project> projects =
                await projectService.GetAllWithUsersAndCollaboratorsAsync(projectFilterParams);

            List<Project> filteredProjects = new List<Project>();


            if(HttpContext.User.CheckIfUserIsAuthenticated())
            {
                User currentUser = await HttpContext.GetContextUser(userService)
                                                    .ConfigureAwait(false);

                foreach(Project project in projects)
                {
                    if(project.InstitutePrivate == false)
                    {
                        filteredProjects.Add(project);
                    }
                    if(project.InstitutePrivate &&
                       currentUser.InstitutionId == project.User.InstitutionId)
                    {
                        filteredProjects.Add(project);
                    }
                }
            } else
            {
                foreach(Project project in projects)
                {
                    if(project.InstitutePrivate == false)
                    {
                        filteredProjects.Add(project);
                    }
                }
            }


            IEnumerable<ProjectResultResource> results =
                mapper.Map<IEnumerable<Project>, IEnumerable<ProjectResultResource>>(filteredProjects);
            ProjectResultsResource resultsResource = new ProjectResultsResource
                                                     {
                                                         Results = results.ToArray(),
                                                         Count = results.Count(),
                                                         TotalCount =
                                                             await projectService.ProjectsCount(projectFilterParams),
                                                         Page = projectFilterParams.Page,
                                                         TotalPages =
                                                             await projectService.GetProjectsTotalPages(
                                                                 projectFilterParams)
                                                     };

            return Ok(resultsResource);
        }

        /// <summary>
        ///     This method is responsible for retrieving a single project.
        /// </summary>
        /// <returns>This method returns the project resource result.</returns>
        /// <response code="200">This endpoint returns the project with the specified id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified id is invalid.</response>
        /// <response code="404">
        ///     The 404 Not Found status code is returned when the project could not be found with the specified
        ///     id.
        /// </response>
        [HttpGet("{projectId}")]
        [ProducesResponseType(typeof(ProjectResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProject(int projectId)
        {
            if(projectId < 0)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting project.",
                                             Detail =
                                                 "The Id is smaller then 0 and therefore it could never be a valid project id.",
                                             Instance = "D590A4FE-FDBA-4AE5-B184-BC7395C45D4E"
                                         };
                return BadRequest(problem);
            }

            Project project = await projectService.FindWithUserAndCollaboratorsAsync(projectId)
                                                  .ConfigureAwait(false);
            if(project == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting project.",
                                             Detail = "The project could not be found in the database.",
                                             Instance = "38516C41-4BFB-47BE-A759-1206BE6D2D13"
                                         };
                return NotFound(problem);
            }

            if(HttpContext.User.CheckIfUserIsAuthenticated())
            {
                User currentUser = await HttpContext.GetContextUser(userService)
                                                    .ConfigureAwait(false);

                if(project.InstitutePrivate &&
                   currentUser.InstitutionId == project.User.InstitutionId)
                {
                    return Ok(mapper.Map<Project, ProjectResourceResult>(project));
                }
                if(project.InstitutePrivate == false)
                {
                    return Ok(mapper.Map<Project, ProjectResourceResult>(project));
                }
            } else
            {
                if(project.InstitutePrivate == false)
                {
                    return Ok(mapper.Map<Project, ProjectResourceResult>(project));
                }
            }

            return NoContent();
        }


        /// <summary>
        ///     This method is responsible for creating a Project.
        /// </summary>
        /// <returns>This method returns the project resource result.</returns>
        /// <response code="200">This endpoint returns the created project.</response>
        /// <response code="400">
        ///     The 400 Bad Request status code is returned when the project
        ///     resource is not specified or failed to save project to the database.
        /// </response>
        [HttpPost]
        [Authorize(Policy = nameof(Defaults.Scopes.ProjectWrite))]
        [ProducesResponseType(typeof(ProjectResourceResult), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateProjectAsync([FromBody] ProjectResource projectResource)
        {
            if(projectResource == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed to create a new project.",
                                             Detail = "The specified project resource was null.",
                                             Instance = "8D3D9119-0D12-4631-B2DC-56494639A849"
                                         };
                return BadRequest(problem);
            }

            if(projectResource.CallToAction != null)
            {
                IEnumerable<CallToActionOption> callToActionOptions =
                    await callToActionOptionService.GetCallToActionOptionFromValueAsync(
                        projectResource.CallToAction.OptionValue);
                if(!callToActionOptions.Any())
                {
                    ProblemDetails problem = new ProblemDetails
                                             {
                                                 Title = "Call to action value was not found.",
                                                 Detail =
                                                     "The specified call to action value was not found while creating the project.",
                                                 Instance = "40EE82EB-930F-40C8-AE94-0041F7573FE9"
                                             };
                    return BadRequest(problem);
                }
            }

            Project project = mapper.Map<ProjectResource, Project>(projectResource);
            File file = await fileService.FindAsync(projectResource.FileId);

            if(projectResource.FileId != 0 &&
               file == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "File was not found.",
                                             Detail = "The specified file was not found while creating project.",
                                             Instance = "8CABE64D-6B73-4C88-BBD8-B32FA9FE6EC7"
                                         };
                return BadRequest(problem);
            }

            project.ProjectIcon = file;
            project.User = await HttpContext.GetContextUser(userService)
                                            .ConfigureAwait(false);

            try
            {
                projectService.Add(project);
                projectService.Save();
                return Created(nameof(CreateProjectAsync), mapper.Map<Project, ProjectResourceResult>(project));
            } catch(DbUpdateException e)
            {
                Log.Logger.Error(e, "Database exception");


                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed to save new project.",
                                             Detail = "There was a problem while saving the project to the database.",
                                             Instance = "9FEEF001-F91F-44E9-8090-6106703AB033"
                                         };
                return BadRequest(problem);
            }
        }

        /// <summary>
        ///     This method is responsible for updating the project with the specified identifier.
        /// </summary>
        /// <param name="projectId">The project identifier which is used for searching the project.</param>
        /// <param name="projectResource">The project resource which is used for updating the project.</param>
        /// <returns>This method returns the project resource result.</returns>
        /// <response code="200">This endpoint returns the updated project.</response>
        /// <response code="401">The 401 Unauthorized status code is return when the user has not the correct permission to update.</response>
        /// <response code="404">The 404 not Found status code is returned when the project to update is not found.</response>
        [HttpPut("{projectId}")]
        [Authorize]
        [ProducesResponseType(typeof(ProjectResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateProject(int projectId, [FromBody] ProjectResource projectResource)
        {
            Project project = await projectService.FindAsync(projectId)
                                                  .ConfigureAwait(false);
            if(project == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed to update project.",
                                             Detail = "The specified project could not be found in the database.",
                                             Instance = "b27d3600-33b0-42a0-99aa-4b2f28ea07bb"
                                         };
                return NotFound(problem);
            }

            User user = await HttpContext.GetContextUser(userService)
                                         .ConfigureAwait(false);
            bool isAllowed = userService.UserHasScope(user.IdentityId, nameof(Defaults.Scopes.AdminProjectWrite));

            if(!(project.UserId == user.Id || isAllowed))
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed to edit the project.",
                                             Detail = "The user is not allowed to edit the project.",
                                             Instance = "906cd8ad-b75c-4efb-9838-849f99e8026b"
                                         };
                return Unauthorized(problem);
            }

            if(projectResource.CallToAction != null)
            {
                IEnumerable<CallToActionOption> callToActionOptions =
                    await callToActionOptionService.GetCallToActionOptionFromValueAsync(
                        projectResource.CallToAction.OptionValue);
                if(!callToActionOptions.Any())
                {
                    ProblemDetails problem = new ProblemDetails
                                             {
                                                 Title = "Call to action value was not found.",
                                                 Detail =
                                                     "The specified call to action value was not found while creating the project.",
                                                 Instance = "40EE82EB-930F-40C8-AE94-0041F7573FE9"
                                             };
                    return BadRequest(problem);
                }
            }

            // Upload the new file if there is one
            File file = null;
            if(projectResource.FileId != 0)
            {
                if(project.ProjectIconId != 0 &&
                   project.ProjectIconId != null)
                {
                    if(project.ProjectIconId != projectResource.FileId)
                    {
                        File fileToDelete = await fileService.FindAsync(project.ProjectIconId.Value);

                        // Remove the file from the filesystem
                        fileUploader.DeleteFileFromDirectory(fileToDelete);

                        // Remove file from DB
                        await fileService.RemoveAsync(project.ProjectIconId.Value);


                        fileService.Save();
                    }
                }

                // Get the uploaded file
                file = await fileService.FindAsync(projectResource.FileId);

                if(file != null)
                {
                    project.ProjectIcon = file;
                } else
                {
                    ProblemDetails problem = new ProblemDetails
                                             {
                                                 Title = "File was not found.",
                                                 Detail = "The specified file was not found while updating project.",
                                                 Instance = "69166D3D-6D34-4050-BD25-71F1BEBE43D3"
                                             };
                    return BadRequest(problem);
                }
            }
            mapper.Map(projectResource, project);
            projectService.Update(project);
            projectService.Save();
            return Ok(mapper.Map<Project, ProjectResourceResult>(project));
        }

        /// <summary>
        ///     This method is responsible for deleting a project.
        /// </summary>
        /// <returns>This method returns the status code 200.</returns>
        /// <response code="200">This endpoint returns status code 200. The project is deleted.</response>
        /// <response code="401">
        ///     The 401 Unauthorized status code is returned when the the user has not the correct permission to
        ///     delete.
        /// </response>
        /// <response code="404">The 404 Not Found status code is returned when the project to delete was not found.</response>
        [HttpDelete("{projectId}")]
        [Authorize]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            Project project = await projectService.FindAsync(projectId)
                                                  .ConfigureAwait(false);
            if(project == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed to delete the project.",
                                             Detail = "The project could not be found in the database.",
                                             Instance = "AF63CF48-ECAA-4996-BAA0-BF52926D12AC"
                                         };
                return NotFound(problem);
            }

            User user = await HttpContext.GetContextUser(userService)
                                         .ConfigureAwait(false);
            bool isAllowed = await authorizationHelper.UserIsAllowed(user,
                                                                     nameof(Defaults.Scopes.AdminProjectWrite),
                                                                     nameof(Defaults.Scopes.InstitutionProjectWrite),
                                                                     project.UserId);

            if(!(project.UserId == user.Id || isAllowed))
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed to delete the project.",
                                             Detail = "The user is not allowed to delete the project.",
                                             Instance = "D0363680-5B4F-40A1-B381-0A7544C70164"
                                         };
                return Unauthorized(problem);
            }

            if(project.ProjectIconId.HasValue)
            {
                // We need to delete the old file.
                File fileToDelete = await fileService.FindAsync(project.ProjectIconId.Value);
                try
                {
                    // Remove the file from the database
                    await fileService.RemoveAsync(fileToDelete.Id)
                                     .ConfigureAwait(false);
                    fileService.Save();

                    // Remove the file from the filesystem
                    fileUploader.DeleteFileFromDirectory(fileToDelete);
                } catch(FileNotFoundException)
                {
                    ProblemDetails problem = new ProblemDetails
                                             {
                                                 Title = "File could not be deleted because the path does not exist.",
                                                 Detail = "File could not be found.",
                                                 Instance = "367594c4-1fab-47ae-beb4-a41b53c65a18"
                                             };

                    return NotFound(problem);
                }
            }

            await projectService.RemoveAsync(projectId)
                                .ConfigureAwait(false);
            projectService.Save();

            return Ok();
        }

        /// <summary>
        ///     Follows a project with given projectId and gets userId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>200 if success 409 if user already follows project</returns>
        [HttpPost("follow/{projectId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> FollowProject(int projectId)
        {
            User user = await HttpContext.GetContextUser(userService)
                                         .ConfigureAwait(false);

            if(await userService.FindAsync(user.Id) == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting the user account.",
                                             Detail = "The database does not contain a user with this user id.",
                                             Instance = "B778C55A-D41E-4101-A7A0-F02F76E5A6AE"
                                         };
                return NotFound(problem);
            }

            if(userProjectService.CheckIfUserFollows(user.Id, projectId))
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "User already follows this project",
                                             Detail = "You are already following this project.",
                                             Instance = "27D14082-9906-4EB8-AE4C-65BAEC0BB4FD"
                                         };
                return Conflict(problem);
            }

            Project project = await projectService.FindAsync(projectId);

            if(await projectService.FindAsync(projectId) == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting the project.",
                                             Detail = "The database does not contain a project with this project id.",
                                             Instance = "57C13F73-6D22-41F3-AB05-0CCC1B3C8328"
                                         };
                return NotFound(problem);
            }
            UserProject userProject = new UserProject(project, user);
            userProjectService.Add(userProject);

            userProjectService.Save();
            return Ok(mapper.Map<UserProject, UserProjectResourceResult>(userProject));
        }

        /// <summary>
        ///     Unfollows project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpDelete("follow/{projectId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UnfollowProject(int projectId)
        {
            User user = await HttpContext.GetContextUser(userService)
                                         .ConfigureAwait(false);

            if(await userService.FindAsync(user.Id) == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting the user account.",
                                             Detail = "The database does not contain a user with this user id.",
                                             Instance = "B778C55A-D41E-4101-A7A0-F02F76E5A6AE"
                                         };
                return NotFound(problem);
            }

            if(userProjectService.CheckIfUserFollows(user.Id, projectId) == false)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "User is not following this project",
                                             Detail = "You are not following this project.",
                                             Instance = "27D14082-9906-4EB8-AE4C-65BAEC0BB4FD"
                                         };
                return Conflict(problem);
            }

            Project project = await projectService.FindAsync(projectId);

            if(await projectService.FindAsync(projectId) == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting the project.",
                                             Detail = "The database does not contain a project with this project id.",
                                             Instance = "57C13F73-6D22-41F3-AB05-0CCC1B3C8328"
                                         };
                return NotFound(problem);
            }
            UserProject userProject = new UserProject(project, user);
            userProjectService.Remove(userProject);

            userProjectService.Save();
            return Ok();
        }

        /// <summary>
        ///     Likes an individual project with the provided projectId.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>
        ///     StatusCode 200 If success,
        ///     StatusCode 409 If the user already liked the project,
        ///     StatusCode 404 if the project could not be found.
        /// </returns>
        [HttpPost("like/{projectId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LikeProject(int projectId)
        {
            User currentUser = await HttpContext.GetContextUser(userService)
                                                .ConfigureAwait(false);

            if(currentUser == null)
            {
                ProblemDetails problemDetails = new ProblemDetails
                                                {
                                                    Title = "Failed to getting the user account.",
                                                    Detail =
                                                        "The database does not contain a user with the provided user id.",
                                                    Instance = "F8DB2F94-48DA-4FEB-9BDA-FF24A59333C1"
                                                };
                return NotFound(problemDetails);
            }

            if(userProjectLikeService.CheckIfUserAlreadyLiked(currentUser.Id, projectId))
            {
                ProblemDetails problemDetails = new ProblemDetails
                                                {
                                                    Title = "User already liked this project",
                                                    Detail = "You already liked this project.",
                                                    Instance = "5B0104E2-C864-4ADB-9321-32CD352DC124"
                                                };
                return Conflict(problemDetails);
            }

            Project projectToLike = await projectService.FindAsync(projectId);

            if(projectToLike == null)
            {
                ProblemDetails problemDetails = new ProblemDetails
                                                {
                                                    Title = "Failed to getting the project.",
                                                    Detail =
                                                        "The database does not contain a project with the provided project id.",
                                                    Instance = "711B2DDE-D028-479E-8CB7-33F587478F8F"
                                                };
                return NotFound(problemDetails);
            }

            try
            {
                ProjectLike like = new ProjectLike(projectToLike, currentUser);
                await userProjectLikeService.AddAsync(like)
                                            .ConfigureAwait(false);

                userProjectLikeService.Save();
                return Ok(mapper.Map<ProjectLike, UserProjectLikeResourceResult>(like));
            } catch(DbUpdateException e)
            {
                Log.Logger.Error(e, "Database exception!");

                ProblemDetails problemDetails = new ProblemDetails
                                                {
                                                    Title = "Could not create the liked project details.",
                                                    Detail = "The database failed to save the liked project.",
                                                    Instance = "F941879E-6C25-4A35-A962-8E86382E1849"
                                                };
                return BadRequest(problemDetails);
            }
        }

        /// <summary>
        ///     Unlikes an individual project with the provided projectId.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>
        ///     StatusCode 200 If success,
        ///     StatusCode 409 if the user didn't like the project already,
        ///     StatusCode 404 if the project could not be found.
        /// </returns>
        [HttpDelete("like/{projectId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UnlikeProject(int projectId)
        {
            User currentUser = await HttpContext.GetContextUser(userService)
                                                .ConfigureAwait(false);

            if(currentUser == null)
            {
                ProblemDetails problemDetails = new ProblemDetails
                                                {
                                                    Title = "Failed to getting the user account.",
                                                    Detail =
                                                        "The database does not contain a user with the provided user id.",
                                                    Instance = "F8DB2F94-48DA-4FEB-9BDA-FF24A59333C1"
                                                };
                return NotFound(problemDetails);
            }

            if(!userProjectLikeService.CheckIfUserAlreadyLiked(currentUser.Id, projectId))
            {
                ProblemDetails problemDetails = new ProblemDetails
                                                {
                                                    Title = "User didn't like this project.",
                                                    Detail = "You did not like this project at the moment.",
                                                    Instance = "03590F81-C06D-4707-A646-B9B7F79B8A15"
                                                };
                return Conflict(problemDetails);
            }


            Project projectToLike = await projectService.FindAsync(projectId);

            if(projectToLike == null)
            {
                ProblemDetails problemDetails = new ProblemDetails
                                                {
                                                    Title = "Failed to getting the project.",
                                                    Detail =
                                                        "The database does not contain a project with the provided project id.",
                                                    Instance = "711B2DDE-D028-479E-8CB7-33F587478F8F"
                                                };
                return NotFound(problemDetails);
            }

            ProjectLike projectLike = new ProjectLike(projectToLike, currentUser);
            userProjectLikeService.Remove(projectLike);

            userProjectLikeService.Save();
            return Ok(mapper.Map<ProjectLike, UserProjectLikeResourceResult>(projectLike));
        }

        /// <summary>
        ///     Request to link an exising DeX user to a collaborator within a project.
        /// </summary>
        /// <param name="projectId">The project related to the linking.</param>
        /// <param name="collaboratorId">The ID of the collaborator within the given project</param>
        /// <param name="userEmail">The email of DeX user what the collaborator should represent.</param>
        /// <returns>
        ///     StatusCode 200 if success,
        ///     StatusCode 400 if the collaborator has already been linked to a(nother) DeX user,
        ///     StatusCode 404 if some requested resources aren't found.
        /// </returns>
        [HttpPost("collaborator/link/{projectId}/{collaboratorId}/{userEmail}")]
        [Authorize]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LinkCollaborator(int projectId, int collaboratorId, string userEmail)
        {
            //TODO: how to handle collaborator if it doesn't exist (rename method to LinkExistingCollaborator)?
            // Solution: Make a new controller specifically for this task, and if the selected collaboratorId doesn't exist: bad request

            User currentUser = await HttpContext.GetContextUser(userService)
                                                .ConfigureAwait(false);

            Project project = await projectService.FindWithUserAndCollaboratorsAsync(projectId)
                                                  .ConfigureAwait(false);

            if(project == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed linking collaborator.",
                    Detail = "The project could not be found in the database.",
                    Instance = "078187fa-a8be-11eb-bcbc-0242ac130002"
                };
                return NotFound(problem);
            }

            Collaborator collaborator = project.Collaborators.Where(c => c.Id == collaboratorId).FirstOrDefault();

            if(collaborator.LinkedUser != null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed linking collaborator.",
                    Detail = "Collaborator already has been linked.",
                    Instance = "0da7858b-a8be-11eb-bcbc-0242ac130002"
                };
                return BadRequest(problem);
            }

            User user = await userService.GetUserByEmail(userEmail);

            if(user == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed linking collaborator.",
                    Detail = "The user could not be found in the database.",
                    Instance = "10c0d23c-a8be-11eb-bcbc-0242ac130002"
                };
                return NotFound(problem);
            }

            //TODO: Should this code be put into a service?
            if(currentUser.Id == user.Id)// [User flow] The user wants to link themselves to the collaborator.
            {
                CollaboratorLinkedUser linkedUserRequest = new CollaboratorLinkedUser()
                {
                    Id = 1,
                    User = user,
                    AcceptanceHash = "HASH-123",//TODO: generate link/hash code
                    Status = LinkedUserStatus.PENDING
                };

                collaborator.LinkedUser = linkedUserRequest;

                //TODO: save changes to db
                //projectService.Save();
                //collaboratorLinkRequestService.Save();

                //Generate a mail for the project owner/creator concerning the changed collaborator.
                ProjectCollaboratorLinkRequestEmail mail =
                    await projectService.GenerateLinkRequestMail(collaborator, linkedUserRequest.AcceptanceHash);

                string emailAddress = project.User.Email;

                //TODO: send mail
                emailSender.Send(emailAddress, mail.Content, mail.Content);

                return Ok(mail);
            }
            else// [Project Creator/Owner flow] The project creator/owner wants to link a user to a collaborator.
            {
                return StatusCode(501);//Not Implemented
                //if(await projectService.CanUserEdit(currentUser, project) == false)
                //{
                //    ProblemDetails problem = new ProblemDetails
                //    {
                //        Title = "Failed linking collaborator.",
                //        Detail = "Not allowed to send invite.",
                //        Instance = "4af1780c-a8c7-11eb-bcbc-0242ac130002"
                //    };
                //    return Unauthorized(problem);
                //}

                //CollaboratorLinkRequest collaboratorLinkRequest =
                //    await collaboratorLinkRequestService.RegisterCollaboratorLinkRequest(collaborator);

                //ProjectCollaboratorLinkRequestEmail email =
                //    await projectService.PrepareLinkRequestMail(collaborator,
                //                                                user,
                //                                                collaboratorLinkRequest.RequestHash);
                //projectService.Save();
                //collaboratorLinkRequestService.Save();

                //emailSender.Send(user.Email, email.Content, email.Content);

                //return Ok(email);
            }

            return NotFound();

        }

        /// <summary>
        ///     Accept collaborator link request by given hash, previously sent in an email
        /// </summary>
        /// <param name="requestHash">Request identifier sent in an email</param>
        /// <returns>
        ///     StatusCode 200 if the linked collaborator has been accepted,
        ///     StatusCode 400 if requestHash doesn't exist.
        /// </returns>
        [HttpPost("collaborator/accept/{requestHash}")]
        [Authorize]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AcceptCollaborator(string requestHash)
        {
            CollaboratorLinkRequest collaboratorLinkRequest =
                (await collaboratorLinkRequestService.GetAll())
                .Where(clr => clr.RequestHash == requestHash).FirstOrDefault();

            if(collaboratorLinkRequest == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed accepting link collaborator.",
                    Detail = "Hash does not exist.",
                    Instance = "67443384-a8c0-11eb-bcbc-0242ac130002"
                };
                return BadRequest(problem);
            }

            collaboratorLinkRequest.Collaborator.LinkedUser.Status = LinkedUserStatus.ACCEPTED;

            collaboratorLinkRequestService.Update(collaboratorLinkRequest);
            collaboratorLinkRequestService.Save();

            return Ok();
        }


    }

}
