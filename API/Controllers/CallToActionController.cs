using API.Resources;
using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Services;

namespace API.Controllers
{

    /// <summary>
    /// The call to action controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CallToActionController : ControllerBase
    {

        private readonly IUserService userService;
        private readonly ICallToActionService callToActionService;

        /// <summary>
        /// The constructor for call to actions
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="callToActionService"></param>
        public CallToActionController(IUserService userService, ICallToActionService callToActionService)
        {
            this.userService = userService;
            this.callToActionService = callToActionService;
        }

        /// <summary>
        /// CreateCallToActionForGraduatingUsers
        /// </summary>
        /// <returns> Call To Actions </returns>
        [HttpGet]
        public async Task<IActionResult> CreateCallToActionForGraduatingUsers()
        {
            List<CallToAction> callToActions = callToActionService.GetAllGraduateCallToActions();

            return Ok(callToActions);
        }




    }
}
