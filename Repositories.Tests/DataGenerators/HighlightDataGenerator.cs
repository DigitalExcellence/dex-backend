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
    /// FakeDataGenerator for the highlights
    /// </summary>
    public class HighlightDataGenerator : FakeDataGenerator<Highlight>
    {
        /// <summary>
        /// Initializes the highlightDataGenerator
        /// and define dataGenerator options
        /// </summary>
        public HighlightDataGenerator()
        {
            Faker<User> projectUser = new Faker<User>()
                                      .RuleFor(user => user.Name, faker => faker.Name.FirstName())
                                      .RuleFor(user => user.Email, faker => faker.Internet.Email())
                                      .RuleFor(user => user.IdentityId, faker => faker.Random.Int().ToString());
            Faker<Project> fakeProject = new Faker<Project>()
                                         .RuleFor(project => project.Name, faker => faker.Name.FirstName())
                                         .RuleFor(project => project.ShortDescription, faker => faker.Lorem.Words(10).ToString())
                                         .RuleFor(project => project.Description, faker => faker.Lorem.Words(40).ToString())
                                         .RuleFor(project => project.Uri, faker => faker.Internet.Url())
                                         .RuleFor(project => project.Created, faker => faker.Date.Past())
                                         .RuleFor(project => project.Updated, DateTime.Now)
                                         .RuleFor(project => project.User, projectUser);

            Faker = new Faker<Highlight>()
                    .RuleFor(highlight => highlight.Project, fakeProject)
                    .RuleFor(highlight => highlight.StartDate, faker => faker.Date.Past())
                    .RuleFor(highlight => highlight.EndDate, faker => faker.Date.Future());
        }
    }
}
