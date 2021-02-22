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
using NUnit.Framework.Interfaces;
using Repositories.Tests.DataGenerators;
using Repositories.Tests.DataGenerators.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Repositories.Tests.DataSources
{
    /// <summary>
    /// This attribute can generate a variable amount of data sources.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class DataSourceModelDataSource : Attribute, IParameterDataSource
    {

        private readonly IFakeDataGenerator<DataSource> fakeDataGenerator;
        private readonly int amountToGenerate = 0;

        /// <summary>
        /// Initializes DataSourceModelDataSource
        /// </summary>
        public DataSourceModelDataSource()
        {
            fakeDataGenerator = new DataSourceModelDataGenerator();
        }

        /// <summary>
        /// Initializes DataSourceModelDataSource
        /// and setting the amount of data sources to be generated.
        /// </summary>
        public DataSourceModelDataSource(int amount) : this()
        {
            amountToGenerate = amount;
        }

        /// <summary>
        /// This method will generate the requested data sources.
        /// </summary>
        /// <param name="parameter">Extra parameters given in the attribute, not in use but required due to inheritance</param>
        /// <returns>This method returns the generated data.</returns>
        public IEnumerable GetData(IParameterInfo parameter)
        {
            if(amountToGenerate <= 1)
            {
                return new[] {fakeDataGenerator.Generate()};
            }

            List<DataSource> dataSources = fakeDataGenerator.GenerateRange(amountToGenerate)
                                                            .ToList();
            return new[] {dataSources};
        }

    }

}
