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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Models.Defaults;
using System.Reflection;

namespace API.Extensions
{
    /// <summary>
    /// ServicesExtensions
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// Adds the policies.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection AddPolicies(this IServiceCollection services)
        {
            services.Configure<AuthorizationOptions>(o =>
            {
                FieldInfo[] fields = typeof(Defaults.Scopes).GetFields();

                foreach (FieldInfo field in fields)
                {
                    o.AddPolicy(field.Name,
                        policy => { policy.RequireClaim("scope", field.GetRawConstantValue().ToString().Split(' ')); });
                }
            });

            return services;
        }
    }
}