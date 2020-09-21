using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

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
