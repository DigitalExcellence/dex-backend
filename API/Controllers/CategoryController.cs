using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Serilog;
using Services.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static Models.Defaults.Defaults;

namespace API.Controllers
{

    /// <summary>
    ///     This class is responsible for handling HTTP requests that are related
    ///     to categories, for example creating, retrieving, updating or deleting.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly ICategoryService categoryService;
        private readonly IProjectCategoryService projectCategoryService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CategoryController" /> class
        /// </summary>
        /// <param name="categoryService">The category service which is used to communicate with the logic layer.</param>
        /// <param name="projectTagService">The project tag service which is used to communicate with the logic layer.</param>
        /// <param name="mapper">The mapper which is used to convert the resources to the model to the resource result.</param>
        public CategoryController(ICategoryService categoryService, IProjectCategoryService projectTagService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this.projectCategoryService = projectTagService;
            this.mapper = mapper;
        }

        /// <summary>
        ///     This method is responsible for retrieving all categories.
        /// </summary>
        /// <returns>This method returns a list of category resource results.</returns>
        /// <response code="200">This endpoint returns a list of categories.</response>
        [HttpGet]
        [Authorize(Policy = nameof(Scopes.CategoryRead))]
        [ProducesResponseType(typeof(IEnumerable<CategoryResourceResult>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllCategories()
        {
            List<Category> categories = await categoryService.GetAllAsync()
                                                .ConfigureAwait(false);

            return Ok(mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResourceResult>>(categories));
        }

        /// <summary>
        ///     This method is responsible for retrieving a single category.
        /// </summary>
        /// <returns>This method returns the category resource result.</returns>
        /// <response code="200">This endpoint returns the category with the specified id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified category id is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when no category is found with the specified category id.</response>
        [HttpGet("{categoryId}")]
        [Authorize(Policy = nameof(Scopes.CategoryRead))]
        [ProducesResponseType(typeof(CategoryResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            if(categoryId < 0)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting category.",
                                             Detail =
                                                 "The Id is smaller then 0 and therefore it could never be a valid category id.",
                                             Instance = "758F4B36-A047-42D4-9F9E-B09BF8106F85"
                                         };
                return BadRequest(problem);
            }

            Category category = await categoryService.FindAsync(categoryId)
                                         .ConfigureAwait(false);
            if(category == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting category.",
                                             Detail = "The category could not be found in the database.",
                                             Instance = "872DEE7C-D1C8-4161-B8BA-B577EAA5A1C9"
                };
                return NotFound(problem);
            }

            return Ok(mapper.Map<Category, CategoryResourceResult>(category));
        }

        /// <summary>
        ///     This method is responsible for creating the category.
        /// </summary>
        /// <param name="categoryResource">The category resource which is used to create a category.</param>
        /// <returns>This method returns the created category resource result.</returns>
        /// <response code="201">This endpoint returns the created category.</response>
        /// <response code="400">The 400 Bad Request status code is returned when unable to create category.</response>
        [HttpPost]
        [Authorize(Policy = nameof(Scopes.CategoryWrite))]
        [ProducesResponseType(typeof(CategoryResourceResult), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryResource categoryResource)
        {
            if(categoryResource == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed to create a new category.",
                                             Detail = "The specified category resource was null",
                                             Instance = "ABA3B997-1B80-47FC-A72B-69BC0D8DFA93"
                                         };
                return BadRequest(problem);
            }
            Category category = mapper.Map<CategoryResource, Category>(categoryResource);

            try
            {
                await categoryService.AddAsync(category)
                                 .ConfigureAwait(false);
                categoryService.Save();
                return Created(nameof(CreateCategoryAsync), mapper.Map<Category, CategoryResourceResult>(category));
            }
            catch(DbUpdateException e)
            {
                Log.Logger.Error(e, "Database exception");

                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed to save the new category.",
                                             Detail = "There was a problem while saving the category to the database.",
                                             Instance = "D56DBE55-57A1-4655-99C5-4F4ECEEE3BE4"
                                         };
                return BadRequest(problem);
            }
        }

        /// <summary>
        ///     This method is responsible for updating the category.
        /// </summary>
        /// <param name="categoryId">The category identifier which is used for searching the category.</param>
        /// <param name="categoryResource">The category resource which is used to update the category.</param>
        /// <returns>This method returns the updated category resource result.</returns>
        /// <response code="200">This endpoint returns the updated category.</response>
        /// <response code="404">The 404 Not Found status code is returned when the category with the specified id could not be found.</response>
        [HttpPut("{categoryId}")]
        [Authorize(Policy = nameof(Scopes.CategoryWrite))]
        [ProducesResponseType(typeof(CategoryResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateCategory(int categoryId, CategoryResource categoryResource)
        {
            Category currentCategory = await categoryService.FindAsync(categoryId)
                                                .ConfigureAwait(false);
            if(currentCategory == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed to update the category.",
                                             Detail = "The specified category could not be found in the database",
                                             Instance = "8F167FDF-3B2B-4E71-B3D0-AA2B1C1CE2C3"
                                         };
                return NotFound(problem);
            }
            mapper.Map(categoryResource, currentCategory);

            categoryService.Update(currentCategory);
            categoryService.Save();

            return Ok(mapper.Map<Category, CategoryResourceResult>(currentCategory));
        }

        /// <summary>
        ///     This method is responsible for deleting the category.
        /// </summary>
        /// <param name="categoryId">The category identifier which is used for searching the category.</param>
        /// <returns>This method returns status code 200.</returns>
        /// <response code="200">This endpoint returns status code 200. Category is deleted.</response>
        /// <response code="404">The 404 Not Found status code is returned when the category with the specified id could not be found.</response>
        /// <response code="409">The 409 Conflict status code is returned when the category is still connected to a project.</response>
        [HttpDelete("{categoryId}")]
        [Authorize(Policy = nameof(Scopes.CategoryWrite))]
        [ProducesResponseType(typeof(CategoryResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            Category category = await categoryService.FindAsync(categoryId)
                                         .ConfigureAwait(false);
            if(category == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed to delete the category.",
                                             Detail = "The category could not be found in the database.",
                                             Instance = "A0853DE4-C881-4597-A5A7-42F6761CECE0"
                };
                return NotFound(problem);
            }

            ProjectCategory projectCategory = await projectCategoryService.GetProjectCategory(categoryId);

            if(projectCategory != null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to delete the category.",
                    Detail = "The category is still connected to a project.",
                    Instance = "4AA5102B-3A6F-4144-BF01-0EC32B4E69A8"
                };
                return Conflict(problem);
            }

            await categoryService.RemoveAsync(category.Id)
                             .ConfigureAwait(false);
            categoryService.Save();

            List<Category> categories = await categoryService.GetAllAsync()
                                                .ConfigureAwait(false);

            return Ok(mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResourceResult>>(categories));
        }
    }

}
