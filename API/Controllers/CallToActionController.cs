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

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Services;

namespace API.Controllers
{

    /// <summary>
    /// This class is responsible for handling HTTP requests that are related
    /// to the call to actions, for example creating, retrieving or deleting.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class CallToActionController : ControllerBase
    {

        private readonly ICallToActionService callToActionService;

        /// <summary>
        /// The constructor for call to actions
        /// </summary>
        /// <param name="callToActionService"></param>
        public CallToActionController(ICallToActionService callToActionService)
        {
            this.callToActionService = callToActionService;
        }

        /// <summary>
        /// Creates and returns all graduation call to actions for expecting graduation users.
        /// </summary>
        /// <returns> All call To actions which are created or open for graduation users. </returns>
        [HttpGet]
        public async Task<IActionResult> CreateCallToActionForGraduatingUsers()
        {
            List<CallToAction> callToActions = await callToActionService.GetAllOpenGraduateCallToActions();

            return Ok(callToActions);
        }




    }
}
