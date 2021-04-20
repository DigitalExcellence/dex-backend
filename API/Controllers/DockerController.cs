using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{

    /// <summary>
    /// A api controller for testing DockerSwarm Scaling
    /// </summary>
    [ApiController]
    public class DockerController : Controller
    {

        /// <summary>
        /// Simulates endpoint
        /// </summary>
        /// <returns>Pong</returns>
        [HttpGet("/api/docker/ping")]
        public async Task<IActionResult> Ping()
        {
            await Task.Delay(new Random().Next(10,30));
            return Ok("Pong");
        }

    }

}
