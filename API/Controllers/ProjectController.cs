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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Defaults;
using Models.Exceptions;
using Serilog;
using Services.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
        private readonly IProjectCategoryService projectCategoryService;
        private readonly ICategoryService categoryService;
        private readonly IUserProjectLikeService userProjectLikeService;
        private readonly IUserProjectService userProjectService;
        private readonly IUserService userService;
        private readonly IProjectInstitutionService projectInstitutionService;
        private readonly IInstitutionService institutionService;
        private readonly IProjectTransferService projectTransferService;
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
        /// <param name="categoryService">The category service is used to work with categories</param>
        /// <param name="projectCategoryService">The project category service is used to connect projects and categories</param>
        /// <param name="projectInstitutionService">The projectinstitution service is responsible for link projects and institutions.</param>
        /// <param name="institutionService">The institution service which is used to communicate with the logic layer</param>
        /// /// <param name="projectTransferService">The projectTransferservice which is used to communicate with the logic layer</param>
        public ProjectController(IProjectService projectService,
                                 IUserService userService,
                                 IMapper mapper,
                                 IFileService fileService,
                                 IUserProjectLikeService userProjectLikeService,
                                 IAuthorizationHelper authorizationHelper,
                                 IFileUploader fileUploader,
                                 IUserProjectService userProjectService,
                                 IProjectInstitutionService projectInstitutionService,
                                 IInstitutionService institutionService,
                                 ICallToActionOptionService callToActionOptionService,
                                 ICategoryService categoryService,
                                 IProjectCategoryService projectCategoryService, IProjectTransferService projectTransferService)
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
            this.categoryService = categoryService;
            this.projectCategoryService = projectCategoryService;
            this.projectInstitutionService = projectInstitutionService;
            this.institutionService = institutionService;
            this.projectTransferService = projectTransferService;
        }



        /// <summary>
        ///     This method is responsible for retrieving all projects in ElasticSearch formatted model. After these project are retrieved the endpoint registers the projects at the messagebroker to synchronize.
        /// </summary>
        /// <returns>This method returns a list of in ElasticSearch formatted projects.</returns>
        /// <response code="200">This endpoint returns a list of in ElasticSearch formatted projects.</response>
        [HttpGet("export")]
        [Authorize(Policy = nameof(Defaults.Scopes.AdminProjectExport))]
        [ProducesResponseType(typeof(ProjectResultsInput), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> MigrateDatabaseToElasticSearch()
        {
            try
            {
                List<Project> projectsToExport = await projectService.GetAllWithUsersCollaboratorsAndInstitutionsAsync();
                projectService.MigrateDatabase(projectsToExport);

                return Ok(mapper.Map<List<Project>, List<ProjectOutput>>(projectsToExport));
            } catch(Exception e)
            {
                Log.Logger.Error(e.Message);
                return BadRequest(e.Message);
            }

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
        ///

        [HttpGet]
        [ProducesResponseType(typeof(ProjectResultsInput), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllProjects(
            [FromQuery] ProjectFilterParamsInput projectFilterParamsResource)
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
               projectFilterParamsResource.SortBy != "updated" &&
               projectFilterParamsResource.SortBy != "likes")
            {
                problem.Detail = "Invalid sort value: Use \"name\", \"created\", \"updated\" or \"likes\".";
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
                mapper.Map<ProjectFilterParamsInput, ProjectFilterParams>(projectFilterParamsResource);

            IEnumerable<Project> projects =
                await projectService.GetAllWithUsersCollaboratorsAndInstitutionsAsync(projectFilterParams);

            List<Project> filteredProjects = new List<Project>();


            if(HttpContext.User.CheckIfUserIsAuthenticated())
            {
                User currentUser = await HttpContext.GetContextUser(userService)
                                                    .ConfigureAwait(false);

                foreach(Project project in projects)
                {
                    if(project.CanAccess(currentUser))
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


            IEnumerable<ProjectResultInput> results =
                mapper.Map<IEnumerable<Project>, IEnumerable<ProjectResultInput>>(filteredProjects);
            ProjectResultsInput resultsResource = new ProjectResultsInput
            {
                Results = results.ToArray(),
                Count = results.Count(),
                TotalCount = await projectService.ProjectsCount(projectFilterParams),
                Page = projectFilterParams.Page,
                TotalPages = await projectService.GetProjectsTotalPages(projectFilterParams)
            };

            return Ok(resultsResource);
        }

        /// <summary>
        ///     This method returns suggestions while searching for projects
        /// </summary>
        /// <param name="query"></param>
        /// <returns>This method returns a list of autocomplete project resources.</returns>
        /// <response code="200">This endpoint returns a list with suggested projects.</response>
        /// <response code="503">A 503 status code is returned when the Elastic Search service is unavailable.</response>
        [HttpGet("search/autocomplete")]
        [ProducesResponseType(typeof(List<AutocompleteProjectOutput>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), 503)]
        public async Task<IActionResult> GetAutoCompleteProjects([FromQuery(Name ="query")] string query)
        {
            try
            {
                List<Project> projects = await projectService.FindProjectsWhereTitleStartsWithQuery(query);
                List<AutocompleteProjectOutput> autocompleteProjectResourceResults = mapper.Map<List<Project>, List<AutocompleteProjectOutput>>(projects);
                return Ok(autocompleteProjectResourceResults);
            }
            catch(ElasticUnavailableException)
            {
                return StatusCode(503,
                    new ProblemDetails
                    {
                        Title = "Autocomplete results could not be retrieved.",
                        Detail = "ElasticSearch service unavailable.",
                        Instance = "26E7C55F-21DE-4A7B-804C-BC0B74597222"
                    });
            }
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
        [ProducesResponseType(typeof(ProjectOutput), (int) HttpStatusCode.OK)]
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

            Project project = await projectService.FindWithUserCollaboratorsAndInstitutionsAsync(projectId)
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

                if(project.CanAccess(currentUser))
                {
                    return Ok(mapper.Map<Project, ProjectOutput>(project));
                }

            } else
            {
                if(project.InstitutePrivate == false)
                {
                    return Ok(mapper.Map<Project, ProjectOutput>(project));
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
        ///     404 not found when the user is not bound to and institution and tries to make the project isntitute private
        /// </response>
        [HttpPost]
        [Authorize(Policy = nameof(Defaults.Scopes.ProjectWrite))]
        [ProducesResponseType(typeof(ProjectOutput), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateProjectAsync([FromBody] ProjectInput projectResource)
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

            if(projectResource.CallToActions != null)
            {
                if(projectResource.CallToActions.GroupBy(cta => cta.OptionValue).Any(cta => cta.Count() > 1))
                {
                    ProblemDetails problem = new ProblemDetails
                    {
                        Title = "Duplicate call to action option value.",
                        Detail = "It is not possible to create a project with multiple of the same call to actions.",
                        Instance = "D2C8416A-9C55-408B-9468-F0E5C635F9B7"
                    };
                    return BadRequest(problem);
                }

                if(projectResource.CallToActions.Count > projectResource.MaximumCallToActions)
                {
                    ProblemDetails problem = new ProblemDetails
                    {
                        Title = $"Maximum amount of {projectResource.MaximumCallToActions} call to actions exceeded.",
                        Detail = $"It is not possible to create a project with more than {projectResource.MaximumCallToActions} call to actions.",
                        Instance = "E780005D-BBEB-423E-BA01-58145D3DBDF5"
                    };
                    return BadRequest(problem);
                }
                foreach(CallToActionInput callToAction in projectResource.CallToActions)
                {
                    IEnumerable<CallToActionOption> callToActionOptions =
                        await callToActionOptionService.GetCallToActionOptionFromValueAsync(
                            callToAction.OptionValue);
                    if(!callToActionOptions.Any())
                    {
                        ProblemDetails problem = new ProblemDetails
                        {
                            Title = "Call to action value was not found.",
                            Detail = $"The call to action optionvalue: '{callToAction.OptionValue}' was not found while creating the project.",
                            Instance = "40EE82EB-930F-40C8-AE94-0041F7573FE9"
                        };
                        return BadRequest(problem);
                    }
                }
                
            }

            Project project = mapper.Map<ProjectInput, Project>(projectResource);
            Models.File file = await fileService.FindAsync(projectResource.IconId);

            if(projectResource.IconId != 0 &&
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

            if(projectResource.ImageIds.Count() > 10)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Too many images.",
                    Detail = "A project can not have more than 10 images.",
                    Instance = "9E3E4F91-A6ED-415F-8726-6D33BA0F200F"
                };
                return BadRequest(problem);
            }

            foreach(int projectResourceImageId in projectResource.ImageIds)
            {
                Models.File image = await fileService.FindAsync(projectResourceImageId);
                if(image == null)
                {
                    ProblemDetails problem = new ProblemDetails
                                             {
                                                 Title = "Image was not found.",
                                                 Detail = "The specified image was not found while creating project.",
                                                 Instance = "B040FAAD-FD22-4C77-822E-C498DFA1A9CB"
                    };
                    return BadRequest(problem);
                }

                project.Images.Add(image);
            }

            project.ProjectIcon = file;
            project.User = await HttpContext.GetContextUser(userService)
                                            .ConfigureAwait(false);

            if(projectResource.Categories != null)
            {
                ICollection<ProjectCategoryInput> projectCategoryResources = projectResource.Categories;

                foreach(ProjectCategoryInput projectCategoryResource in projectCategoryResources)
                {
                    ProjectCategory alreadyExcProjectCategory = await projectCategoryService.GetProjectCategory(project.Id, projectCategoryResource.Id);
                    if(alreadyExcProjectCategory == null)
                    {
                        Category category = await categoryService.FindAsync(projectCategoryResource.Id);

                        if(category == null)
                        {
                            ProblemDetails problem = new ProblemDetails
                            {
                                Title = "Failed to save new project.",
                                Detail = "One of the given categories did not exist.",
                                Instance = "C152D170-F9C2-48DE-8111-02DBD160C768"
                            };
                            return BadRequest(problem);
                        }

                        ProjectCategory projectCategory = new ProjectCategory(project, category);
                        await projectCategoryService.AddAsync(projectCategory)
                                               .ConfigureAwait(false);
                    }
                }
            }

            if(project.InstitutePrivate)
            {
                if(project.User.InstitutionId == default)
                {
                    ProblemDetails problem = new ProblemDetails
                    {
                        Title = "Unable to create project.",
                        Detail = "The insitute private is set to true, but the user creating the project isn't bound to an institution.",
                        Instance = "b942c55d-01be-4fd1-90a1-a5ad3d172403"
                    };
                    return NotFound(problem);
                }

                project.LinkedInstitutions.Add(new ProjectInstitution { Project = project, Institution = project.User.Institution });
            }

            try
            {
                projectService.Add(project);
                projectService.Save();

                projectCategoryService.Save();

                return Created(nameof(CreateProjectAsync), mapper.Map<Project, ProjectOutput>(project));
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
        /// <response code="400">The 404 if the use tries to update the institute private property with this endpoint.</response>
        [HttpPut("{projectId}")]
        [Authorize]
        [ProducesResponseType(typeof(ProjectOutput), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProject(int projectId, [FromBody] ProjectInput projectResource)
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

            if(projectResource.CallToActions != null)
            {
                if(projectResource.CallToActions.GroupBy(cta => cta.OptionValue).Any(cta => cta.Count() > 1))
                {
                    ProblemDetails problem = new ProblemDetails
                    {
                        Title = "Duplicate call to action option value.",
                        Detail = "It is not possible to create a project with multiple of the same call to actions.",
                        Instance = "D2C8416A-9C55-408B-9468-F0E5C635F9B7"
                    };
                    return BadRequest(problem);
                }

                if(projectResource.CallToActions.Count > projectResource.MaximumCallToActions)
                {
                    ProblemDetails problem = new ProblemDetails
                    {
                        Title = $"Maximum amount of {projectResource.MaximumCallToActions} call to actions exceeded.",
                        Detail =
                            $"It is not possible to create a project with more than {projectResource.MaximumCallToActions} call to actions.",
                        Instance = "E780005D-BBEB-423E-BA01-58145D3DBDF5"
                    };
                    return BadRequest(problem);
                }

                foreach(CallToActionInput callToAction in projectResource.CallToActions)
                {
                    IEnumerable<CallToActionOption> callToActionOptions =
                        await callToActionOptionService.GetCallToActionOptionFromValueAsync(
                            callToAction.OptionValue);

                    if(!callToActionOptions.Any())
                    {
                        ProblemDetails problem = new ProblemDetails
                        {
                            Title = "Call to action value was not found.",
                            Detail =
                                $"The call to action optionvalue: '{callToAction.OptionValue}' was not found while creating the project.",
                            Instance = "40EE82EB-930F-40C8-AE94-0041F7573FE9"
                        };
                        return BadRequest(problem);
                    }
                }
                
            }

            if (projectResource.InstitutePrivate != project.InstitutePrivate)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Unable to update project.",
                    Detail = $"It is not possible to update the institute private of a project with this endpoint. Please use the {nameof(UpdateProjectPrivateStatus)} endpoint.",
                    Instance = "6a92321e-c6a7-41d9-90ee-9148566718e3"
                };
                return BadRequest(problem);
            }

            if(projectResource.IconId != 0)
            {
                Models.File file = await fileService.FindAsync(projectResource.IconId);
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
            } else
            {
                project.ProjectIcon = null;
            }

            if(projectResource.ImageIds.Count() > 10)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Too many images.",
                    Detail = "A project can not have more than 10 images.",
                    Instance = "5D179737-046E-482B-B6D8-DDDCEF518107"
                };
                return BadRequest(problem);
            }

            project.Images.Clear();
            foreach(int projectResourceImageId in projectResource.ImageIds)
            {
                Models.File image = await fileService.FindAsync(projectResourceImageId);
                if(image == null)
                {
                    ProblemDetails problem = new ProblemDetails
                                             {
                                                 Title = "Image was not found.",
                                                 Detail = "The specified image was not found while updating project.",
                                                 Instance = "FC816E40-31A6-4187-BEBA-D22F06019F8F"
                    };
                    return BadRequest(problem);
                }

                project.Images.Add(image);
            }

            await projectCategoryService.ClearProjectCategories(project);
            if(projectResource.Categories != null)
            {
                ICollection<ProjectCategoryInput> projectCategoryResources = projectResource.Categories;

                foreach(ProjectCategoryInput projectCategoryResource in projectCategoryResources)
                {
                    ProjectCategory alreadyExcProjectCategory = await projectCategoryService.GetProjectCategory(project.Id, projectCategoryResource.Id);
                    if(alreadyExcProjectCategory == null)
                    {
                        Category category = await categoryService.FindAsync(projectCategoryResource.Id);

                        if(category == null)
                        {
                            ProblemDetails problem = new ProblemDetails
                            {
                                Title = "Failed to update project.",
                                Detail = "One of the given categories did not exist.",
                                Instance = "09D1458E-B2CF-4F23-B120-DDD38A7727C9"
                            };
                            return BadRequest(problem);
                        }

                        ProjectCategory projectCategory = new ProjectCategory(project, category);
                        await projectCategoryService.AddAsync(projectCategory)
                                               .ConfigureAwait(false);
                    }
                }
            }

            mapper.Map(projectResource, project);
            projectService.Update(project);
            projectService.Save();


            return Ok(mapper.Map<Project, ProjectOutput>(project));
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
                Models.File fileToDelete = await fileService.FindAsync(project.ProjectIconId.Value);
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

            await projectCategoryService.ClearProjectCategories(project);

            await projectService.RemoveAsync(projectId)
                                .ConfigureAwait(false);
            projectService.Save();

            return Ok();
        }

        /// <summary>
        ///     Follows a project with given projectId and gets userId
        /// </summary>
        /// <param name="projectId"></param>
        /// <response code="200">Project was followed.</response>
        /// <response code="404">Project or user could not be found.</response>
        /// <response code="409">User already follows this project.</response>
        /// <returns></returns>
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
            return Ok(mapper.Map<UserProject, UserProjectOutput>(userProject));
        }

        /// <summary>
        ///     Unfollows project
        /// </summary>
        /// <param name="projectId"></param>
        /// <response code="200">Project was unfollowed.</response>
        /// <response code="404">Project or user could not be found.</response>
        /// <response code="409">User does not follow this project.</response>
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
        /// <response code="200">Project was liked.</response>
        /// <response code="400">Database failed to like the project.</response>
        /// <response code="404">Project could not be found.</response>
        /// <response code="409">User already liked this project before.</response>
        /// <returns></returns>
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

                // Update Elastic Database about this change.
                await userProjectLikeService.SyncProjectToES(projectToLike);
                return Ok(mapper.Map<ProjectLike, UserProjectLikeOutput>(like));
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
        /// <response code="200">Project was unliked.</response>
        /// <response code="404">Project could not be found.</response>
        /// <response code="409">User did not like this project before.</response>
        /// <returns></returns>
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

            // Update Elastic Database about this change.
            await userProjectLikeService.SyncProjectToES(projectToLike);
            return Ok(mapper.Map<ProjectLike, UserProjectLikeOutput>(projectLike));
        }



        /// <summary>
        /// Updates the institutionprivate property of a project to the given value in the body
        /// and adds the institution of the creator to the project if the institution isn't yet linked
        /// to the project and institutePrivate is true.
        /// </summary>
        /// <param name="projectId">The project identifier</param>
        /// <param name="institutePrivate">The new value for InstitutePrivate in the given project</param>
        /// <response code="404">If the project or current user couldn't be found.</response>
        /// <response code="404">The creator of the project doesn't belong to an institution </response>
        /// <response code="403">If the user doesn't have the rights to make the project private.</response>
        /// <response code="200">If success</response>
        [HttpPut("instituteprivate/{projectId}")]
        [Authorize]
        [ProducesResponseType(typeof(ProjectResultInput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateProjectPrivateStatus(int projectId, [FromBody] bool institutePrivate)
        {
            User currentUser = await HttpContext.GetContextUser(userService)
                                              .ConfigureAwait(false);

            if(currentUser == null)
            {
                ProblemDetails problemDetails = new ProblemDetails
                {
                    Title = "Failed to getting the user account.",
                    Detail = "The database does not contain a user with the provided user id.",
                    Instance = "88cfe86d-fcdd-42d2-8460-1ea1d582d879"
                };
                return NotFound(problemDetails);
            }

            Project project = await projectService.FindWithUserCollaboratorsAndInstitutionsAsync(projectId)
                                                  .ConfigureAwait(false);

            if(project == null)
            {
                ProblemDetails problemDetails = new ProblemDetails
                {
                    Title = "Failed setting the status of the project.",
                    Detail = "The project send in the request could not be found in the database.",
                    Instance = "68cdf62d-26ef-4b6e-9f98-fdddcfc0fc71"
                };
                return NotFound(problemDetails);
            }

            bool isAllowed = await authorizationHelper.UserIsAllowed(currentUser,
                                                                     nameof(Defaults.Scopes.AdminProjectWrite),
                                                                     nameof(Defaults.Scopes.InstitutionProjectWrite),
                                                                     project.UserId);

            bool isCreatorAndHasScope =
                project.IsCreator(currentUser.Id) &&
                userService.UserHasScope(currentUser.IdentityId, nameof(Defaults.Scopes.ProjectWrite));

            //If the current user isn't the creator of the project or the dataofficer of the organization which the creator belongs to
            //Then this user is not authorized to link the institution to the project
            if(!(isCreatorAndHasScope ||
                 isAllowed))
            {
                return Forbid();
            }

            int projectCreatorInstitutionId = project.User.InstitutionId.GetValueOrDefault();

            if(projectCreatorInstitutionId == default)
            {
                ProblemDetails problemDetails = new ProblemDetails
                {
                    Title = "Failed setting the status of the project.",
                    Detail = "The creator of the project send in the request is not bound to an institution and can therefore not make the project private.",
                    Instance = "6972f184-7715-41e6-8bca-8b4ee63d5c58"
                };
                return NotFound(problemDetails);
            }

            //Link institution of the creator of the project to project if the institution isn't linked to the project yet
            if(institutePrivate &&
                !projectInstitutionService.InstitutionIsLinkedToProject(projectId, projectCreatorInstitutionId))
            {
                ProjectInstitution projectInstitution = new ProjectInstitution(projectId, projectCreatorInstitutionId);
                await projectInstitutionService.AddAsync(projectInstitution).ConfigureAwait(false);
            }

            project.InstitutePrivate = institutePrivate;
            projectService.Update(project);

            projectService.Save();

            return Ok(mapper.Map<Project, ProjectOutput>(project));
        }

        /// <summary>
        /// Links given project and institution. This function is admin only!
        /// </summary>
        /// <param name="projectId">The project identifier</param>
        /// <param name="institutionId">The institution identifier</param>
        /// <response code="404">If the project, institution or current user couldn't be found.</response>
        /// <response code="409">If the project is already linked to the institution.</response>
        /// <response code="201">If success</response>
        [HttpPost("linkedinstitution/{projectId}/{institutionId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.AdminProjectWrite))]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProjectInstitutionOutput), StatusCodes.Status201Created)]
        public async Task<IActionResult> LinkInstitutionToProjectAsync(int projectId, int institutionId)
        {
            User currentUser = await HttpContext.GetContextUser(userService)
                                            .ConfigureAwait(false);

            if(currentUser == null)
            {
                ProblemDetails problemDetails = new ProblemDetails
                {
                    Title = "Failed getting the user account.",
                    Detail = "The database does not contain a user with the provided user id.",
                    Instance = "11deede6-c76c-42cd-ac50-97743e3cff2a"
                };
                return NotFound(problemDetails);
            }
            if(!await projectService.ProjectExistsAsync(projectId))
            {
                ProblemDetails problemDetails = new ProblemDetails
                {
                    Title = "Failed setting the status of the project.",
                    Detail = "The project send in the request could not be found in the database.",
                    Instance = "4a73928f-827f-44a5-b508-e6a8c73c1717"
                };
                return NotFound(problemDetails);
            }

            if(!await institutionService.InstitutionExistsAsync(institutionId))
            {
                ProblemDetails problemDetails = new ProblemDetails
                {
                    Title = "Failed getting the institution.",
                    Detail = "The institution send in the request could not be found in the database.",
                    Instance = "01cdc6f4-42aa-4394-bbc8-2576e4f99498"
                };
                return NotFound(problemDetails);
            }

            if(projectInstitutionService.InstitutionIsLinkedToProject(projectId, institutionId))
            {
                ProblemDetails problemDetails = new ProblemDetails
                {
                    Title = "Institution is already linked to project.",
                    Detail = "The institution cannot be linked to the project because the institution is already part of the project.",
                    Instance = "01b26993-0c9e-4890-a7fe-f0f39450e616"
                };
                return Conflict(problemDetails);
            }

            ProjectInstitution projectInstitution = new ProjectInstitution(projectId, institutionId);
            await projectInstitutionService.AddAsync(projectInstitution);
            projectInstitutionService.Save();

            //Maybe a bit wasteful to get the entire project and institution but its fast enough for now.
            projectInstitution.Project = await projectService.FindAsync(projectId);
            projectInstitution.Institution = await institutionService.FindAsync(institutionId);

            return Created(nameof(LinkInstitutionToProjectAsync), mapper.Map<ProjectInstitution, ProjectInstitutionOutput>(projectInstitution));
        }

        /// <summary>
        /// Links given project and institution. This function is admin only!
        /// </summary>
        /// <param name="projectId">The project identifier</param>
        /// <param name="institutionId">The institution identifier</param>
        /// <response code="404">If the project, institution or current user couldn't be found.</response>
        /// <response code="409">If the project is not yet linked to the institution.</response>
        /// <response code="200">If success</response>
        [HttpDelete("linkedinstitution/{projectId}/{institutionId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.AdminProjectWrite))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UnlinkInstitutionFromProjectAsync(int projectId, int institutionId)
        {
            User currentUser = await HttpContext.GetContextUser(userService)
                                            .ConfigureAwait(false);

            if(currentUser == null)
            {
                ProblemDetails problemDetails = new ProblemDetails
                {
                    Title = "Failed to getting the user account.",
                    Detail = "The database does not contain a user with the provided user id.",
                    Instance = "7561e57a-d957-4823-9b90-d107aa54f893"
                };
                return NotFound(problemDetails);
            }

            if(!await projectService.ProjectExistsAsync(projectId))
            {
                ProblemDetails problemDetails = new ProblemDetails
                {
                    Title = "Failed setting the status of the project.",
                    Detail = "The project send in the request could not be found in the database.",
                    Instance = "f358fc51-0761-4929-8cca-71a6225fe0cd"
                };
                return NotFound(problemDetails);
            }

            if(!await institutionService.InstitutionExistsAsync(institutionId))
            {
                ProblemDetails problemDetails = new ProblemDetails
                {
                    Title = "Failed getting the institution.",
                    Detail = "The institution send in the request could not be found in the database.",
                    Instance = "7065c1e7-0543-470e-ab55-403c13418626"
                };
                return NotFound(problemDetails);
            }

            if(!projectInstitutionService.InstitutionIsLinkedToProject(projectId, institutionId))
            {
                ProblemDetails problemDetails = new ProblemDetails
                {
                    Title = "Institution is not yet linked to the project.",
                    Detail = "The institution cannot be unlinked from the project, because it is not linked to the project in the first place.",
                    Instance = "695ef305-06b3-4ad8-99f2-773cd0f03e6e"
                };
                return Conflict(problemDetails);
            }

            projectInstitutionService.RemoveByProjectIdAndInstitutionId(projectId, institutionId);
            projectInstitutionService.Save();

            return Ok();
        }

        /// <summary>
        ///     Categorize a project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="categoryId"></param>
        /// <response code="200">Category was added to the project.</response>
        /// <response code="401">User is not authorized to add the category to the project.</response>
        /// <response code="404">Project or category was not found.</response>
        /// <response code="409">Project is already categorized with the category.</response>
        /// <returns></returns>
        [HttpPost("category/{projectId}/{categoryId}")]
        [Authorize]
        [ProducesResponseType(typeof(ProjectOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ProjectAddCategory(int projectId, int categoryId)
        {
            Project project = await projectService.FindAsync(projectId)
                                                  .ConfigureAwait(false);
            if(project == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to categorize the project.",
                    Detail = "The project could not be found in the database.",
                    Instance = "1C8D069D-E6CE-43E2-9CF9-D82C0A71A292"
                };
                return NotFound(problem);
            }

            Category category = await categoryService.FindAsync(categoryId)
                                      .ConfigureAwait(false);

            if(category == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to categorize the project.",
                    Detail = "The category could not be found in the database.",
                    Instance = "93C6B5BD-EC14-482A-9907-001C888F3D3F"
                };
                return NotFound(problem);
            }

            User user = await HttpContext.GetContextUser(userService)
                                         .ConfigureAwait(false);
            bool isAllowed = await authorizationHelper.UserIsAllowed(user,
                                                                     nameof(Defaults.Scopes.AdminProjectWrite),
                                                                     nameof(Defaults.Scopes.InstitutionProjectWrite),
                                                                     project.UserId);

            if(project.UserId != user.Id && !isAllowed)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to categorize the project.",
                    Detail = "The user is not allowed to modify the project.",
                    Instance = "1243016C-081F-441C-A388-3D56B0998D2E"
                };
                return Unauthorized(problem);
            }

            ProjectCategory alreadyCategorized = await projectCategoryService.GetProjectCategory(projectId, categoryId);

            if(alreadyCategorized != null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to categorize the project.",
                    Detail = "Project has already been categorized with this category.",
                    Instance = "4986CBC6-FB6D-4255-ACE8-833E92B25FBD"
                };
                return Conflict(problem);
            }


            ProjectCategory projectCategory = new ProjectCategory(project, category);
            await projectCategoryService.AddAsync(projectCategory)
                                   .ConfigureAwait(false);

            projectCategoryService.Save();

            return Ok(mapper.Map<Project, ProjectOutput>(project));
        }

        /// <summary>
        ///     Remove a category from a project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="categoryId"></param>
        /// <response code="200">Category was removed from the project.</response>
        /// <response code="401">User is not authorized to remove the category from the project.</response>
        /// <response code="404">Project or category was not found.</response>
        /// <response code="409">Project is not categorized with the category.</response>
        /// <returns></returns>
        [HttpDelete("category/{projectId}/{categoryId}")]
        [Authorize]
        [ProducesResponseType(typeof(ProjectOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ProjectRemoveCategory(int projectId, int categoryId)
        {
            Project project = await projectService.FindAsync(projectId)
                                                  .ConfigureAwait(false);
            if(project == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to remove the category from the project.",
                    Detail = "The project could not be found in the database.",
                    Instance = "2CC94251-9103-4AAC-B461-F99939E78AD0"
                };
                return NotFound(problem);
            }

            Category category = await categoryService.FindAsync(categoryId)
                                      .ConfigureAwait(false);

            if(category == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to remove the category from the project.",
                    Detail = "The category could not be found in the database.",
                    Instance = "3E41B5DC-F78B-429B-89AB-1A98A6F65FDC"
                };
                return NotFound(problem);
            }

            User user = await HttpContext.GetContextUser(userService)
                                         .ConfigureAwait(false);
            bool isAllowed = await authorizationHelper.UserIsAllowed(user,
                                                                     nameof(Defaults.Scopes.AdminProjectWrite),
                                                                     nameof(Defaults.Scopes.InstitutionProjectWrite),
                                                                     project.UserId);

            if(project.UserId != user.Id && !isAllowed)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to remove the category from the project.",
                    Detail = "The user is not allowed to modify the project.",
                    Instance = "4D1878C1-1606-4224-841A-73F30AE4F930"
                };
                return Unauthorized(problem);
            }

            ProjectCategory alreadyCategorized = await projectCategoryService.GetProjectCategory(projectId, categoryId);

            if(alreadyCategorized == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to remove the category from the project.",
                    Detail = "Project has not been categorized with this category.",
                    Instance = "5EABA4F3-47E6-45A7-8522-E87268716912"
                };
                return Conflict(problem);
            }

            await projectCategoryService.RemoveAsync(alreadyCategorized.Id)
                             .ConfigureAwait(false);

            projectCategoryService.Save();

            return Ok(mapper.Map<Project, ProjectOutput>(project));
        }
    }

}
