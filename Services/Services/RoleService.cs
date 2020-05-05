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
using Repositories;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using static Models.Defaults.Defaults;

namespace Services.Services
{

    public interface IRoleService : IService<Role>
    {

        Task<List<Role>> GetAllAsync();
        List<string> GetValidScopes();
        bool isValidScope(string scope);

    }

    public class RoleService : Service<Role>, IRoleService
    {

        public RoleService(IRoleRepository repository) : base(repository) { }

        protected new IRoleRepository Repository => (IRoleRepository) base.Repository;

        public Task<List<Role>> GetAllAsync()
        {
            return Repository.GetAllAsync();
        }
        /// <summary>
        /// Gets the valid scopes.
        /// </summary>
        /// <returns></returns>
        public List<string> GetValidScopes()
        {
            Type scopeType = typeof(Scopes);
            FieldInfo[] scopeTypeFields = scopeType.GetFields(BindingFlags.Static | BindingFlags.Public);
            List<string> validScopes = new List<string>();
            foreach(FieldInfo x in scopeTypeFields)
            {
                validScopes.Add(x.Name);
            }
            return validScopes;
        }

        /// <summary>
        /// Determines whether the scope is a valid scope.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <returns>
        ///   <c>true</c> if [is valid scope] [the specified scope]; otherwise, <c>false</c>.
        /// </returns>
        public bool isValidScope(string scope)
        {
            List<string> scopes = GetValidScopes();
            return scopes.Contains(scope);
        }

    }

}
