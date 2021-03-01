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

using Bogus;
using Models;
using Repositories.Tests.DataGenerators.Base;
using System;

namespace Repositories.Tests.DataGenerators
{

    /// <summary>
    ///     This fake data generator is capable of generating data sources.
    /// </summary>
    public class DataSourceModelDataGenerator : FakeDataGenerator<DataSource>
    {

        /// <summary>
        ///     Initializes the data source model data generator and defines dataGenerator options.
        /// </summary>
        public DataSourceModelDataGenerator()
        {
            Faker = new Faker<DataSource>()
                    .RuleFor(d => d.Title, f => f.Name.FirstName())
                    .RuleFor(d => d.Guid,
                             f => Guid.NewGuid()
                                      .ToString())
                    .RuleFor(d => d.IsVisible, f => f.Random.Bool())
                    .RuleFor(d => d.Description, f => f.Name.JobDescriptor());
        }

    }

}
