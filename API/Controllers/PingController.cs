using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("ping")]
    public class PingController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok( "pong a niffo" );
        }

        private static Random staticRandom = new Random();

        [HttpGet("bruh")]
        public async Task<IActionResult> SlowIndex()
        {
            await Task.Delay(staticRandom.Next(20, 250));
            return Ok( "Langzame pong a niffo" );
        }
    }
}
