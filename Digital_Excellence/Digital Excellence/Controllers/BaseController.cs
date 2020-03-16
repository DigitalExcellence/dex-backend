using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Services;

namespace API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IUserService userService;
        /// <summary>
        /// 
        /// </summary>
        public User CurrentUser
        {
            get
            {
                string upn = HttpContext.User.Claims.Single(s => s.Type == ClaimTypes.Upn).Value;
                return userService.GetUserByUsername(upn);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        public BaseController(IUserService userService)
        {
            this.userService = userService;
        }
    }
}
