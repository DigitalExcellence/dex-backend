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

using RestSharp;
using System;

namespace Services.Sources
{

    /// <summary>
    ///     The rest client factory interface
    /// </summary>
    public interface IRestClientFactory
    {

        /// <summary>
        ///     Creates a restclient with the specified base url.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <returns>Restclient.</returns>
        IRestClient Create(Uri baseUrl);

    }

    /// <summary>
    ///     This factory makes it easier to make rest client using code unit testable.
    /// </summary>
    /// <seealso cref="IRestClientFactory" />
    public class RestClientFactory : IRestClientFactory
    {

        /// <summary>
        ///     Creates a restclient with the specified base url.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <returns>
        ///     Restclient.
        /// </returns>
        public IRestClient Create(Uri baseUrl)
        {
            return new RestClient(baseUrl);
        }

    }

}
