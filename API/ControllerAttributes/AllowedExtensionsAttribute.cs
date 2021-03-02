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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.ControllerAttributes
{
    /// <summary>
    /// Attribute for settings the allowed file extensions for every single file in the request.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AllowedExtensionsAttribute : ActionFilterAttribute
    {
        // The allowed extensions for files .jpg, .png, etc.
        private readonly string[] allowedExtensions;

        /// <summary>
        /// Constructor of the AllowedExtensionsAttribute consumes allowedExtensions.
        /// </summary>
        /// <param name="allowedExtensions"></param>
        public AllowedExtensionsAttribute(string[] allowedExtensions)
        {
            this.allowedExtensions = allowedExtensions.Select(x => x.ToLower()).ToArray();
        }

        /// <summary>
        /// This method is called before the controller(action) is called.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            bool fileExtensionsAreValid = true;
            IFormCollection form = context.HttpContext.Request.Form;
            foreach(IFormFile file in form.Files)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                // Check if file extension is allowed
                if(!allowedExtensions.Contains(fileExtension))
                {
                    fileExtensionsAreValid = false;
                    ProblemDetails problem = new ProblemDetails
                    {
                        Title = "Failed posting file.",
                        Detail = $"{fileExtension} is not accepted as a valid file extension.",
                        Instance = "a218b143-37a3-402b-b7e4-f5996a86428a"
                    };
                    context.Result = new JsonResult(problem);
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            if(fileExtensionsAreValid)
            {
                await next.Invoke();
            }
        }
    }
}
