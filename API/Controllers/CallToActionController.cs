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
            List<User> users = userService.GetAllExpectedGraduatingUsers();
            IEnumerable<CallToAction> allCallToActions = await callToActionService.GetAll();
            DateTime now = DateTime.Now;
            DateTime max = DateTime.Now.AddMonths(6);

            List<CallToAction> callToActions = new List<CallToAction>();
            foreach(CallToAction callToAction in users.Where(user => user.ExpectedGraduationDate <= max &&
                                                                     user.ExpectedGraduationDate >= now)
                                                      .SelectMany(user => from cta in allCallToActions where user.Id == cta.UserId && cta.Type == 0
                                                                          select new CallToAction(user.Id, CallToActionType.graduationReminder)))
            {
                callToActions.Add(callToAction);

                callToActionService.Add(callToAction);
            }
            callToActionService.Save();

            return Ok(callToActions);
        }




    }
}
