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


using NUnit.Framework.Interfaces;
using Repositories.Tests.DataGenerators.Base;
using Services.ExternalDataProviders.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Services.Tests.ExternalDataProviders.DataSources.Gitlab
{

    /// <summary>
    ///     Attribute to generate Gitlab Data Source User Resource Results
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class GitlabDataSourceUserResourceResultDataSourceAttribute : Attribute, IParameterDataSource
    {

        private readonly int amountToGenerate;
        private readonly IFakeDataGenerator<GitlabDataSourceUserResourceResult> fakeDataGenerator;

        /// <summary>
        ///     Initializes GitlabDataSourceUserResourceResultDataSourceAttribute.
        /// </summary>
        public GitlabDataSourceUserResourceResultDataSourceAttribute()
        {
            fakeDataGenerator = new GitlabDataSourceUserResourceResultDataGenerator();
        }

        /// <summary>
        ///     Initializes GitlabDataSourceUserResourceResultDataSourceAttribute
        ///     and setting the amount of Github data source resource results to be generated.
        /// </summary>
        public GitlabDataSourceUserResourceResultDataSourceAttribute(int amountToGenerate)
        {
            this.amountToGenerate = amountToGenerate;
        }

        /// <summary>
        ///     Generate the data and return it.
        /// </summary>
        /// <param name="parameter">Extra parameters given in the attribute, not in use but required due to inheritance.</param>
        /// <returns>The generated data.</returns>
        public IEnumerable GetData(IParameterInfo parameter)
        {
            if(amountToGenerate <= 1)
            {
                return new[] {fakeDataGenerator.Generate()};
            }
            List<GitlabDataSourceUserResourceResult> projects = fakeDataGenerator.GenerateRange(amountToGenerate)
                                                                             .ToList();
            return new [] {projects};
        }

    }

}
