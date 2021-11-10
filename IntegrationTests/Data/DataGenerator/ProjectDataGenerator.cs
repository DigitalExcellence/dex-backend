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
using Bogus;
using IntegrationTests.Data.DataGenerator.Base;
using Models;
using System;

namespace IntegrationTests.Data.DataGenerator
{

    /// <summary>
    ///     FakeDataGenerator for the projects
    /// </summary>
    public class ProjectDataGenerator : FakeDataGenerator<Project>
    {
         
        /// <summary>
        ///     Initializes the projectDataGenerator
        ///     and define dataGenerator options
        /// </summary>
        public ProjectDataGenerator()
        {
            Faker = new Faker<Project>()
                    .RuleFor(project => project.Name, faker => faker.Name.FirstName())
                    .RuleFor(project => project.ShortDescription,
                             faker => faker.Lorem.Words(10)
                                           .ToString())
                    .RuleFor(project => project.Description,
                             faker => faker.Lorem.Words(40)
                                           .ToString())
                    .RuleFor(project => project.Uri, faker => faker.Internet.Url());
        }

    }

}
