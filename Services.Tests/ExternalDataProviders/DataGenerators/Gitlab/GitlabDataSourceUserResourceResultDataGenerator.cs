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
using Repositories.Tests.DataGenerators.Base;
using Services.ExternalDataProviders.Resources;
using System;

namespace Services.Tests.ExternalDataProviders.DataGenerators.Gitlab
{

    /// <summary>
    ///     FakeDataGenerator for a GitlabDataSourceUserResourceResultDataGenerator.
    /// </summary>
    public class GitlabDataSourceUserResourceResultDataGenerator
        : FakeDataGenerator<GitlabDataSourceUserResourceResult>
    {

        /// <summary>
        ///     Initializes the GitlabDataSourceUserResourceResultDataGenerator
        ///     and define dataGenerator options.
        /// </summary>
        public GitlabDataSourceUserResourceResultDataGenerator()
        {
            Faker = new Faker<GitlabDataSourceUserResourceResult>()
                .RuleFor(_ => _.Id, faker => faker.Random.Long())
                .RuleFor(_ => _.Username, faker => faker.Name.FirstName())
                .RuleFor(_ => _.Name, faker => faker.Name.FirstName())
                .RuleFor(_ => _.Email, faker => faker.Person.Email)
                .RuleFor(_ => _.AvatarUrl, faker => new Uri(faker.Image.PicsumUrl()))
                .RuleFor(_ => _.WebUrl, faker => new Uri(faker.Image.PicsumUrl()))
                .RuleFor(_ => _.Bio, faker =>faker.Lorem.Sentence(5));
        }

    }

}
