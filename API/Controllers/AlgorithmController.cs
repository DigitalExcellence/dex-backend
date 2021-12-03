using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Defaults;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlgorithmController : ControllerBase
    {
        [HttpPost("Activity")]
        [Authorize(Policy = nameof(Defaults.Roles.BackendApplication))]
        public async Task<IActionResult> ActivityAlgorithm()
        {
            return Ok("Test");
        }
    }
}
