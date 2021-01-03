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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Services.DataProviders
{

    public interface IDataProviderLoader
    {

        IEnumerable<IDataSourceAdaptee> GetAllDataSources();

        IDataSourceAdaptee GetDataSourceByGuid(string guid);

    }

    public class DataProviderLoader : IDataProviderLoader
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public DataProviderLoader(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public IEnumerable<IDataSourceAdaptee> GetAllDataSources()
        {
            List<IDataSourceAdaptee> dataSources = new List<IDataSourceAdaptee>();
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string folder = Path.GetDirectoryName(executingAssembly.Location);
            
            
            foreach(string dll in Directory.GetFiles(folder, "*.dll"))
            {
                Assembly assembly = Assembly.LoadFrom(dll);
                foreach(Type type in assembly.GetTypes())
                {
                    if(type.GetInterface("IDataSourceAdaptee") != typeof(IDataSourceAdaptee)) continue;
                    object dataSourceAdaptee = scope.ServiceProvider.GetService(type);
                    if(dataSourceAdaptee != null)
                        dataSources.Add(dataSourceAdaptee as IDataSourceAdaptee);
                }
            }

            return dataSources;
        }

        public IDataSourceAdaptee GetDataSourceByGuid(string guid)
        {
            return GetAllDataSources()
                .SingleOrDefault(d => d.Guid == guid);
        }

    }

}
