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

namespace Services.Tests.ExternalDataProviders.DataGenerators.Github
{

    /// <summary>
    ///     FakeDataGenerator for a GithubDataSourceReadmeResourceResultDataGenerator
    /// </summary>
    public class GithubDataSourceReadmeResourceResultDataGenerator
        : FakeDataGenerator<GithubDataSourceReadmeResourceResult>
    {

        /// <summary>
        ///     Initializes the GithubDataSourceReadmeResourceResultDataGenerator
        ///     and define dataGenerator options.
        /// </summary>
        public GithubDataSourceReadmeResourceResultDataGenerator()
        {
            GithubDataSourceReadmeLinksResourceResultDataGenerator linksGenerator =
                new GithubDataSourceReadmeLinksResourceResultDataGenerator();

            Faker = new Faker<GithubDataSourceReadmeResourceResult>()
                    .RuleFor(_ => _.Size, faker => faker.Random.Long())
                    .RuleFor(_ => _.Name, faker => faker.Name.FirstName())
                    .RuleFor(_ => _.Content, faker => faker.Lorem.Sentences(10))
                    .RuleFor(_ => _.Url, faker => new Uri(faker.Person.Website))
                    .RuleFor(_ => _.GitUrl, faker => new Uri(faker.Person.Website))
                    .RuleFor(_ => _.HtmlUrl, faker => new Uri(faker.Person.Website))
                    .RuleFor(_ => _.DownloadUrl, faker => new Uri(faker.Person.Website))
                    .RuleFor(_ => _.Links, linksGenerator.Generate);
        }

    }

}
