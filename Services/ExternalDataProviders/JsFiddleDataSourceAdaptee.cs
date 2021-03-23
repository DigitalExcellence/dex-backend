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

using AutoMapper;
using Models;
using Models.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using Services.ExternalDataProviders.Resources;
using Services.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Services.ExternalDataProviders
{
    /// <summary>
    ///     The JsFiddle data source
    /// </summary>
    public class JsFiddleDataSourceAdaptee : IPublicDataSourceAdaptee
    {

        /// <summary>
        ///     Mapper object from auto mapper that will automatically maps one object to another.
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        ///     A factory that will generate a rest client to make API requests.
        /// </summary>
        private readonly IRestClientFactory restClientFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="JsFiddleDataSourceAdaptee" /> class./>
        /// </summary>
        /// <param name="restClientFactory">The rest client factory which is used to create rest clients.</param>
        /// <param name="mapper">The mapper which is used to map Github resource results to projects.</param>
        public JsFiddleDataSourceAdaptee(IRestClientFactory restClientFactory, IMapper mapper)
        {
            this.restClientFactory = restClientFactory;
            this.mapper = mapper;
        }

        /// <summary>
        ///     Gets the value for the guid from the JsFiddle data source adaptee.
        /// </summary>
        public string Guid => "96666870-3afe-44e2-8d62-337d49cf972d";

        /// <summary>
        ///     Gets or sets a value for the Title property from the JsFiddle data source adaptee.
        /// </summary>
        public string Title { get; set; } = "JsFiddle";

        /// <summary>
        ///     Gets the value for the Base Url from the JsFiddle data source adaptee.
        /// </summary>
        public string BaseUrl { get; set; } = "http://jsfiddle.net/api/";

        /// <summary>
        ///     Gets or sets a value for the IsVisible property from the JsFiddle data source adaptee.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Icon property from the JsFiddle data source adaptee.
        /// </summary>
        public File Icon { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Description property from the JsFiddle data source adaptee.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets a value for the DataSourceWizardPages property from the JsFiddle data source adaptee.
        /// </summary>
        public IList<DataSourceWizardPage> DataSourceWizardPages { get; set; }

        /// <summary>
        ///     This method is responsible for retrieving all public projects from a user, via the username, from the JsFiddle API.
        /// </summary>
        /// <param name="username">The username which will be used to search to retrieve all public projects from the user.</param>
        /// <returns>This method returns a collections of public projects from the user</returns>
        public async Task<IEnumerable<Project>> GetAllPublicProjects(string username)
        {
            JsFiddleDataSourceResourceResult[] resourceResult = (await FetchAllFiddlesFromUser(username)).ToArray();
            if(!resourceResult.Any()) return null;
            return mapper.Map<IEnumerable<JsFiddleDataSourceResourceResult>, IEnumerable<Project>>(resourceResult);
        }

        /// <summary>
        ///     This method is responsible for retrieving a public project from a uri, from the JsFiddle API.
        /// </summary>
        /// <param name="sourceUri">The source uri which will be used to retrieve the correct project.</param>
        /// <returns>This method throws a not supported by external API exception.</returns>
        public Task<Project> GetPublicProjectFromUri(Uri sourceUri)
        {
            throw new NotSupportedByExternalApiException(Title, nameof(GetPublicProjectFromUri));
        }

        /// <summary>
        ///     This method is responsible for retrieving a public project from the user, by id from the JsFiddle API.
        /// </summary>
        /// <param name="identifier">The identifier which will be used to retrieve the correct project.</param>
        /// <returns>This method throws a not supported by external API exception.</returns>
        public Task<Project> GetPublicProjectById(string identifier)
        {
            throw new NotSupportedByExternalApiException(Title, nameof(GetPublicProjectById));
        }

        /// <summary>
        ///     This method is responsible for retrieving the content from all fiddles from a user.
        /// </summary>
        /// <param name="username">The username which is used to retrieve the correct fiddles.</param>
        /// <returns>This method returns a collection of JsFiddle data source resource results.</returns>
        /// <exception cref="ExternalException">
        ///     This method could throw an external exception whenever the status code is not
        ///     successful.
        /// </exception>
        private async Task<IEnumerable<JsFiddleDataSourceResourceResult>> FetchAllFiddlesFromUser(string username)
        {
            IRestClient client = restClientFactory.Create(new Uri(BaseUrl));
            IRestRequest request = new RestRequest($"user/{username}/demo/list.json", Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            if(string.IsNullOrEmpty(response.Content)) return null;
            if(!response.IsSuccessful) throw new ExternalException(response.ErrorMessage);

            IEnumerable<JsFiddleDataSourceResourceResult> resourceResult =
                JsonConvert.DeserializeObject<IEnumerable<JsFiddleDataSourceResourceResult>>(response.Content);
            return resourceResult;
        }

    }

}
