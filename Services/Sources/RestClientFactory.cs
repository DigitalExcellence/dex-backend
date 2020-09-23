using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Sources
{
    /// <summary>
    /// The rest client factory interface
    /// </summary>
    public interface IRestClientFactory
    {
        /// <summary>
        /// Creates a restclient with the specified base url.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <returns>Restclient.</returns>
        IRestClient Create(Uri baseUrl);


    }
    /// <summary>
    /// This factory makes it easier to make rest client using code unit testable.
    /// </summary>
    /// <seealso cref="Services.Sources.IRestClientFactory" />
    public class RestClientFactory : IRestClientFactory
    {
        /// <summary>
        /// Creates a restclient with the specified base url.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <returns>
        /// Restclient.
        /// </returns>
        public IRestClient Create(Uri baseUrl)
        {
            return new RestClient(baseUrl);
        }
    }
}
