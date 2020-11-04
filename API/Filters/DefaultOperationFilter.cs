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

using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace API.Filters
{
    /// <summary>
    /// Filter for all endpoints to make sure that the response
    /// media type will be set to 'application/json'.
    /// </summary>
    public class DefaultOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Foreach http status code keep the media type for application/json
        /// and remove the other media types.
        /// </summary>
        /// <param name="operation">API Operation</param>
        /// <param name="context">Filter context</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // If there already is a consume attribute, this won't be overridden,
            // so only if nothing is configured, the default will be set to application/json.
            bool hasConsumeAttribute = context.MethodInfo.GetCustomAttributes(true)
                                              .Union(context.MethodInfo.GetCustomAttributes(true))
                                              .OfType<ConsumesAttribute>()
                                              .Count() != 0;
            if(operation.RequestBody != null && !hasConsumeAttribute)
            {
                OpenApiMediaType mediaType = operation.RequestBody.Content["application/json"];
                operation.RequestBody.Content.Clear();
                operation.RequestBody.Content.Add("application/json", mediaType);
            }

            foreach(string code in operation.Responses.Keys)
            {
                if(operation.Responses[code]
                            .Content.TryGetValue("application/json", out OpenApiMediaType mediaType))
                {
                    operation.Responses[code].Content.Clear();
                    operation.Responses[code].Content.Add("application/json", mediaType);
                }
            }
        }
    }
}
