using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace API.Filters
{
    /// <summary>
    /// Filter for all endpoints to make sure that the response
    /// media type will be set to 'application/json'
    /// </summary>
    public class DefaultOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Foreach http status code keep the media type for application/json
        /// and remove the other media types
        /// </summary>
        /// <param name="operation">API Operation</param>
        /// <param name="context">Filter context</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            bool hasConsumeAttribute = context.MethodInfo.GetCustomAttributes(true)
                                              .Union(context.MethodInfo.GetCustomAttributes(true))
                                              .OfType<ConsumesAttribute>()
                                              .Count() != 0;
            if(operation.RequestBody != null && !hasConsumeAttribute)
            {
                var mediaType = operation.RequestBody.Content["application/json"];
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
