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
    ///     to the tags, for example creating, retrieving, updating or deleting.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly ITagService tagService;
        private readonly IProjectTagService projectTagService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TagController" /> class
        /// </summary>
        /// <param name="tagService">The tag service which is used to communicate with the logic layer.</param>
        /// <param name="projectTagService">The project tag service which is used to communicate with the logic layer.</param>
        /// <param name="mapper">The mapper which is used to convert the resources to the model to the resource result.</param>
        public TagController(ITagService tagService, IProjectTagService projectTagService, IMapper mapper)
        {
            this.tagService = tagService;
            this.projectTagService = projectTagService;
            this.mapper = mapper;
        }

        /// <summary>
        ///     This method is responsible for retrieving all tags.
        /// </summary>
        /// <returns>This method returns a list of tag resource results.</returns>
        /// <response code="200">This endpoint returns a list of tags.</response>
        [HttpGet]
        [Authorize(Policy = nameof(Scopes.TagRead))]
        [ProducesResponseType(typeof(IEnumerable<TagResourceResult>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllTags()
        {
            List<Tag> tags = await tagService.GetAllAsync()
                                                .ConfigureAwait(false);

            return Ok(mapper.Map<IEnumerable<Tag>, IEnumerable<TagResourceResult>>(tags));
        }

        /// <summary>
        ///     This method is responsible for retrieving a single tag.
        /// </summary>
        /// <returns>This method returns the tag resource result.</returns>
        /// <response code="200">This endpoint returns the tag with the specified id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified tag id is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when no tag is found with the specified tag id.</response>
        [HttpGet("{tagId}")]
        [Authorize(Policy = nameof(Scopes.TagRead))]
        [ProducesResponseType(typeof(TagResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTag(int tagId)
        {
            if(tagId < 0)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting tag.",
                                             Detail =
                                                 "The Id is smaller then 0 and therefore it could never be a valid tag id.",
                                             Instance = "5024ADDA-6DE2-4B49-896A-526E8EC4313D"
                                         };
                return BadRequest(problem);
            }

            Tag tag = await tagService.FindAsync(tagId)
                                         .ConfigureAwait(false);
            if(tag == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting tag.",
                                             Detail = "The tag could not be found in the database.",
                                             Instance = "1739EFA6-3F31-4C88-B596-74DA403AC51B"
                                         };
                return NotFound(problem);
            }

            return Ok(mapper.Map<Tag, TagResourceResult>(tag));
        }

        /// <summary>
        ///     This method is responsible for creating the tag.
        /// </summary>
        /// <param name="tagResource">The tag resource which is used to create a tag.</param>
        /// <returns>This method returns the created tag resource result.</returns>
        /// <response code="201">This endpoint returns the created tag.</response>
        /// <response code="400">The 400 Bad Request status code is returned when unable to create tag.</response>
        [HttpPost]
        [Authorize(Policy = nameof(Scopes.TagWrite))]
        [ProducesResponseType(typeof(TagResourceResult), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateTagAsync([FromBody] TagResource tagResource)
        {
            if(tagResource == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed to create a new tag.",
                                             Detail = "The specified tag resource was null",
                                             Instance = "ABA3B997-1B80-47FC-A72B-69BC0D8DFA93"
                                         };
                return BadRequest(problem);
            }
            Tag tag = mapper.Map<TagResource, Tag>(tagResource);

            try
            {
                await tagService.AddAsync(tag)
                                 .ConfigureAwait(false);
                tagService.Save();
                return Created(nameof(CreateTagAsync), mapper.Map<Tag, TagResourceResult>(tag));
            }
            catch(DbUpdateException e)
            {
                Log.Logger.Error(e, "Database exception");

                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed to save the new tag.",
                                             Detail = "There was a problem while saving the tag to the database.",
                                             Instance = "D56DBE55-57A1-4655-99C5-4F4ECEEE3BE4"
                                         };
                return BadRequest(problem);
            }
        }

        /// <summary>
        ///     This method is responsible for updating the tag.
        /// </summary>
        /// <param name="tagId">The tag identifier which is used for searching the tag.</param>
        /// <param name="tagResource">The tag resource which is used to update the tag.</param>
        /// <returns>This method returns the updated tag resource result.</returns>
        /// <response code="200">This endpoint returns the updated tag.</response>
        /// <response code="404">The 404 Not Found status code is returned when the tag with the specified id could not be found.</response>
        [HttpPut("{tagId}")]
        [Authorize(Policy = nameof(Scopes.TagWrite))]
        [ProducesResponseType(typeof(TagResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateTag(int tagId, TagResource tagResource)
        {
            Tag currentTag = await tagService.FindAsync(tagId)
                                                .ConfigureAwait(false);
            if(currentTag == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed to update the tag.",
                                             Detail = "The specified tag could not be found in the database",
                                             Instance = "8F167FDF-3B2B-4E71-B3D0-AA2B1C1CE2C3"
                                         };
                return NotFound(problem);
            }
            mapper.Map(tagResource, currentTag);

            tagService.Update(currentTag);
            tagService.Save();

            return Ok(mapper.Map<Tag, TagResourceResult>(currentTag));
        }

        /// <summary>
        ///     This method is responsible for deleting the tag.
        /// </summary>
        /// <param name="tagId">The tag identifier which is used for searching the tag.</param>
        /// <returns>This method returns status code 200.</returns>
        /// <response code="200">This endpoint returns status code 200. Tag is deleted.</response>
        /// <response code="404">The 404 Not Found status code is returned when the tag with the specified id could not be found.</response>
        /// <response code="409">The 409 Conflict status code is returned when the tag is still connected to a project.</response>
        [HttpDelete("{tagId}")]
        [Authorize(Policy = nameof(Scopes.TagWrite))]
        [ProducesResponseType(typeof(TagResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteTag(int tagId)
        {
            Tag tag = await tagService.FindAsync(tagId)
                                         .ConfigureAwait(false);
            if(tag == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed to delete the tag.",
                                             Detail = "The tag could not be found in the database.",
                                             Instance = "A0853DE4-C881-4597-A5A7-42F6761CECE0"
                };
                return NotFound(problem);
            }

            ProjectTag projectTag = await projectTagService.GetProjectTag(tagId);

            if(projectTag != null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to delete the tag.",
                    Detail = "The tag is still connected to a project.",
                    Instance = "4AA5102B-3A6F-4144-BF01-0EC32B4E69A8"
                };
                return Conflict(problem);
            }

            await tagService.RemoveAsync(tag.Id)
                             .ConfigureAwait(false);
            tagService.Save();

            List<Tag> tags = await tagService.GetAllAsync()
                                                .ConfigureAwait(false);

            return Ok(mapper.Map<IEnumerable<Tag>, IEnumerable<TagResourceResult>>(tags));
        }
    }

}
