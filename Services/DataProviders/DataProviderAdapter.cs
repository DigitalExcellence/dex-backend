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

using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.DataProviders
{

    public interface IDataProviderAdapter
    {

        Task<IEnumerable<Project>> GetAllProjects(string token, bool needsAuth);

        Task<Project> GetProjectByGuid(string token, string id, bool needsAuth);


        string GetOauthUrl();

        Task<OauthTokens> GetTokens(string code);

    }

    public class DataProviderAdapter : IDataProviderAdapter
    {

        private readonly IDataSourceAdaptee adaptee;

        public DataProviderAdapter(IDataSourceAdaptee adaptee)
        {
            this.adaptee = adaptee;
        }

        public async Task<IEnumerable<Project>> GetAllProjects(string token, bool needsAuth)
        {
            if(!needsAuth)
            {
                return await GetAllProjectsWithoutAccessToken(token);
            }
            
            return await GetAllProjectWithAccessToken(token);
        }

        public Task<Project> GetProjectByGuid(string token, string id, bool needsAuth)
        {
            throw new NotImplementedException();
        }


        public string GetOauthUrl()
        {
            IAuthorizedDataSourceAdaptee authorizedDataSource = adaptee as IAuthorizedDataSourceAdaptee;
            return authorizedDataSource?.OauthUrl;
        }

        public async Task<OauthTokens> GetTokens(string code)
        {
            IAuthorizedDataSourceAdaptee dataProvider = adaptee as IAuthorizedDataSourceAdaptee;
            if(dataProvider == null) return null;
            return await dataProvider.GetTokens(code);
        }

        private async Task<IEnumerable<Project>> GetAllProjectWithAccessToken(string accessToken)
        {
            IAuthorizedDataSourceAdaptee authorizedDataSourceAdaptee = adaptee as IAuthorizedDataSourceAdaptee;
            if(authorizedDataSourceAdaptee == null) return null;
            IEnumerable<Project> projects = await authorizedDataSourceAdaptee.GetAllProjects(accessToken);
            return projects;
        }

        private async Task<IEnumerable<Project>> GetAllProjectsWithoutAccessToken(string username)
        {
            IPublicDataSourceAdaptee publicDataSourceAdaptee = adaptee as IPublicDataSourceAdaptee;
            if(publicDataSourceAdaptee == null) return null;
            IEnumerable<Project> projects = await publicDataSourceAdaptee.GetAllPublicProjects(username);
            return projects;
        }

    }

}
