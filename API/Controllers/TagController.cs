using API.InputOutput.Tag;
using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Serilog;
using Services;
using Services.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static Models.Defaults.Defaults;

namespace API.Controllers
{

    /// <summary>
    ///     This class is responsible for handling HTTP requests that are related
    ///     to the roles, for example creating, retrieving, updating or deleting.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly ITagService tagService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TagController" /> class
        /// </summary>
        /// <param name="tagService">The tag service which is used to communicate with the logic layer.</param>
        /// <param name="mapper">The mapper which is used to convert the resources to the model to the resource result.</param>
        public TagController(ITagService tagService, IMapper mapper)
        {
            this.tagService = tagService;
            this.mapper = mapper;
        }

        /// <summary>
        ///     This method is responsible for retrieving all roles.
        /// </summary>
        /// <returns>This method returns a list of tag resource results.</returns>
        /// <response code="200">This endpoint returns a list of tags.</response>

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TagOutput>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllTags()
        {
            IEnumerable<Tag> tags = await tagService.GetAll()
                                                .ConfigureAwait(false);
            IEnumerable<TagOutput> tagsOutput = mapper.Map<IEnumerable<Tag>, IEnumerable<TagOutput>>(tags);
            return Ok(tagsOutput);
        }

        /// <summary>
        ///     This method is responsible for retrieving a single tag.
        /// </summary>
        /// <returns>This method return the tag resource result.</returns>
        /// <response code="200">This endpoint returns the tag with the specified id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified tag id is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when no tag is found with the specified tag id.</response>
        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Scopes.RoleRead))]
        [ProducesResponseType(typeof(TagOutput), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTagById(int id)
        {
            if(id < 0)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting tag.",
                    Detail = "The Id is smaller then 0 and therefore it could never be a valid tag id.",
                    Instance = "745FFB49-5968-4F8F-AE73-D621D781FCA0"
                };
                return BadRequest(problem);
            }

            Tag tag = await tagService.FindAsync(id)
                                         .ConfigureAwait(false);
            if(tag == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting tag.",
                    Detail = "The tag could not be found in the database.",
                    Instance = "2BBB4058-FCD1-47A7-BB2C-FF8E6CB439DD"
                };
                return NotFound(problem);
            }

            return Ok(mapper.Map<Tag, TagOutput>(tag));
        }

        /// <summary>
        ///     This method is responsible for retrieving a single tag.
        /// </summary>
        /// <returns>This method return the tag resource result.</returns>
        /// <response code="200">This endpoint returns the tag with the specified name.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified tag name is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when no tag is found with the specified tag name.</response>
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(TagOutput), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTagByName(string name)
        {
            Tag tag = await tagService.FindByNameAsync(name)
                                         .ConfigureAwait(false);
            if(tag == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting tag.",
                    Detail = "The tag could not be found in the database.",
                    Instance = "FD169337-D328-4E5B-977F-7B2E73995D33"
                };
                return NotFound(problem);
            }

            return Ok(mapper.Map<Tag, TagOutput>(tag));
        }

        /// <summary>
        ///     This method is responsible for retrieving a list of tags.
        /// </summary>
        /// <returns>This method return the list of tags.</returns>
        /// <response code="200">This endpoint returns the tag with the specified name.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified tag name is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when no tag is found with the specified tag name.</response>
        [HttpGet("Tags")]
        public async Task<IActionResult> GetTagList(List<Tag> tags)
        {
            List<Tag> searchTags = new List<Tag>();
            foreach(Tag tag in tags)
            {
                if(tagService.FindByName(tag.Name) == null)
                {
                    tagService.Add(tag);
                    tagService.Save();
                }
                searchTags.Add(tag);
            }
            IEnumerable<TagOutput> tagsOutput = mapper.Map<IEnumerable<Tag>, IEnumerable<TagOutput>>(searchTags);
            return Ok(tagsOutput);
        }

        /// <summary>
        ///     This method is responsible for creating the tag.
        /// </summary>
        /// <param name="tagResource">The tag resource which is used to create a tag.</param>
        /// <returns>This method returns the created tag resource result.</returns>
        /// <response code="201">This endpoint returns the created tag.</response>
        /// <response code="400">The 400 Bad Request status code is returned when unable to create tag.</response>
        [HttpPost]
        [ProducesResponseType(typeof(TagOutput), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateTagAsync([FromBody] TagInput tagResource)
        {
            if(tagResource == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to create a new tag.",
                    Detail = "The specified tag resource was null",
                    Instance = "5ABFDFE4-7151-46AE-BD2C-32804882E796"
                };
                return BadRequest(problem);
            }
            Tag tag = mapper.Map<TagInput, Tag>(tagResource);

            //foreach(RoleScope roleScope in role.Scopes)
            //{
            //    if(!roleService.IsValidScope(roleScope.Scope))
            //    {
            //        ProblemDetails problem = new ProblemDetails
            //        {
            //            Title = "Failed to create a new role.",
            //            Detail = $"The specified scope: {roleScope.Scope} is not valid.",
            //            Instance = "1F40D851-8A4C-41F6-917C-D876970D825F"
            //        };
            //        return BadRequest(problem);
            //    }
            //}

            try
            {
                await tagService.AddAsync(tag)
                                 .ConfigureAwait(false);
                tagService.Save();
                return Created(nameof(CreateTagAsync), mapper.Map<Tag, TagOutput>(tag));
            } catch(DbUpdateException e)
            {
                Log.Logger.Error(e, "Database exception");

                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to save the new tag.",
                    Detail = "There was a problem while saving the tag to the database.",
                    Instance = "4370AD4D-A33B-47B8-9A1B-D11F7A137C89"
                };
                return BadRequest(problem);
            }
        }

        /// <summary>
        ///     This method is responsible for deleting the tag.
        /// </summary>
        /// <param name="id">The tag identifier which is used for searching the tag.</param>
        /// <returns>This method returns status code 200.</returns>
        /// <response code="200">This endpoint returns status code 200. Tag is deleted.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the tag is assigned to a user.</response>
        /// <response code="401">The 401 Unauthorized status code is returned when the user is not allowed to delete tags.</response>
        /// <response code="404">The 404 Not Found status code is returned when the tag with the specified id could not be found.</response>
        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Scopes.AdminProjectWrite))]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteTag(int id)
        {
            Tag tag = await tagService.FindAsync(id)
                                         .ConfigureAwait(false);
            if(tag == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed to delete the tag.",
                                             Detail = "The tag could not be found in the database.",
                                             Instance = "0C2FA6FC-62F0-434D-B3F1-3251D60AF02C"
                };
                return NotFound(problem);
            }

            await tagService.RemoveAsync(tag.Id)
                             .ConfigureAwait(false);
            tagService.Save();
            return Ok();
        }
        
    }

}
