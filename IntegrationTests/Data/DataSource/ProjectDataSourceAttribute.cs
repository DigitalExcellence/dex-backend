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

using API.Resources;
using IntegrationTests.Data.DataGenerator;
using IntegrationTests.Data.DataGenerator.Base;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegrationTests.Data.DataSource
{

    /// <summary>
    ///     Attribute to generate projects
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ProjectDataSourceAttribute : DataAttribute, IEnumerable<object[]>
    {

        private readonly IFakeDataGenerator<Project> fakeDataGenerator;
        private DbContext dbContext;

        /// <summary>
        ///     Initializes collaboratorDataSourceAttribute
        /// </summary>
        public ProjectDataSourceAttribute()
        {
            fakeDataGenerator = new ProjectDataGenerator();
            this.dbContext = DatabaseConnection.DbContext;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return GetEnumerator().Current;
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            var generated = fakeDataGenerator.Generate();

            dbContext.Add(generated);

            yield return new[] { generated };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
