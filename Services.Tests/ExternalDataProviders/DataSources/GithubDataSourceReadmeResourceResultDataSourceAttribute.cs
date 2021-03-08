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
using Services.Tests.ExternalDataProviders.DataGenerators.Github;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Services.Tests.ExternalDataProviders.DataSources
{

    /// <summary>
    ///     Attribute to generate Github Data Source Readme Resource Results
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class GithubDataSourceReadmeResourceResultDataSourceAttribute : Attribute, IParameterDataSource
    {

        private readonly int amountToGenerate;
        private readonly IFakeDataGenerator<GithubDataSourceReadmeResourceResult> fakeDataGenerator;

        /// <summary>
        ///     Initializes GithubDataSourceResourceResultDataSourceAttribute.
        /// </summary>
        public GithubDataSourceReadmeResourceResultDataSourceAttribute()
        {
            fakeDataGenerator = new GithubDataSourceReadmeResourceResultDataGenerator();
        }

        /// <summary>
        ///     Initializes GithubDataSourceReadmeResourceResultDataSourceAttribute
        ///     and setting the amount of Github data source resource results to be generated.
        /// </summary>
        public GithubDataSourceReadmeResourceResultDataSourceAttribute(int amount) : this()
        {
            amountToGenerate = amount;
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
            List<GithubDataSourceReadmeResourceResult> projects = fakeDataGenerator.GenerateRange(amountToGenerate)
                                                                             .ToList();
            return new [] {projects};
        }

    }

}
