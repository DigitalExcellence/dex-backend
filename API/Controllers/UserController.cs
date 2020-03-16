using System.Threading.Tasks;
using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialize a new instance of UserController
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="mapper"></param>
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }


        /// <summary>
        /// Get a user account.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        //[Authorize(Roles = nameof(Defaults.Roles.Student), Policy = nameof(Defaults.Scopes.StudentRead))]
        public async Task<IActionResult> GetUser(int userId)
        {
            if (userId < 0)
            {
                return BadRequest("Invalid user Id");
            }

            User user = await _userService.FindAsync(userId);
            if (user == null)
            {
                return NoContent();
            }

            return Ok(user);
        }


        /// <summary>
        /// Create a user account.
        /// </summary>
        /// <param name="accountResource"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] UserResource accountResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = _mapper.Map<UserResource, User>(accountResource);
            try
            {
                _userService.Add(user);
                _userService.Save();
                return Created(nameof(CreateAccount), _mapper.Map<User, UserResourceResult>(user));
            }
            catch
            {
                return BadRequest("Could not Create the User account");
            }
        }

        /// <summary>
        /// Update the User account.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userResource"></param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateAccount(int userId, [FromBody] UserResource userResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await _userService.FindAsync(userId);
            if (user == null)
            {
                return NoContent();
            }

            _mapper.Map<UserResource, User>(userResource, user);

            _userService.Update(user);
            _userService.Save();

            return Ok(_mapper.Map<User, UserResourceResult>(user));
        }

        /// <summary>
        /// Gets the student information.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAccount(int userId)
        {
            if (await _userService.FindAsync(userId) == null)
            {
                return NoContent();
            }

            await _userService.RemoveAsync(userId);
            _userService.Save();
            return Ok();
        }
    }
}