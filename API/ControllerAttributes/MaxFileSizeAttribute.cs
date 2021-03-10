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
using System.Threading.Tasks;

namespace API.ControllerAttributes
{
    /// <summary>
    /// Attribute for settings the maximum allowed upload size for every single file in bytes in the request.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MaxFileSizeAttribute : ActionFilterAttribute
    {
        // Max file size in bytes.
        private readonly int maxFileSize;

        /// <summary>
        /// Constructor of the MaxFileSizeAttribute consumes maxFileSize in bytes
        /// </summary>
        /// <param name="maxFileSize">Max file size in bytes</param>
        public MaxFileSizeAttribute(int maxFileSize)
        {
            this.maxFileSize = maxFileSize;
        }

        /// <summary>
        /// This method gets called before the controller(Action) is called.
        /// </summary>
        /// <param name="context">Httpcontext</param>
        /// <param name="next"></param>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            bool fileSizeIsValid = true;
            IFormCollection form = context.HttpContext.Request.Form;
            foreach(IFormFile file in form.Files)
            {
                // Check if any files in the request exceeds the maximum file size.
                if(file.Length > maxFileSize)
                {
                    fileSizeIsValid = false;
                    ProblemDetails problem = new ProblemDetails
                    {
                        Title = "Failed posting file.",
                        Detail = $"File is exceeds max upload size of {maxFileSize} bytes.",
                        Instance = "92483d11-fb44-431e-a682-5b6f150d6425"
                    };
                    context.Result = new JsonResult(problem);
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;
                }
            }
            if(fileSizeIsValid)
            {
                await next.Invoke();
            }        
        }
    }
}
