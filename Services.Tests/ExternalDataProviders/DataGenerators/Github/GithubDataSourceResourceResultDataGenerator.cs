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

namespace Services.Tests.ExternalDataProviders.DataGenerators.Github
{

    /// <summary>
    ///     FakeDataGenerator for a GithubDataSourceResourceResult.
    /// </summary>
    public class GithubDataSourceResourceResultDataGenerator
        : FakeDataGenerator<GithubDataSourceResourceResult>
    {

        /// <summary>
        ///     Initializes the GithubDataSourceResourceResultDataGenerator
        ///     and define dataGenerator options.
        /// </summary>
        public GithubDataSourceResourceResultDataGenerator()
        {
            GithubDataSourceOwnerResourceResultDataGenerator ownerGenerator =
                new GithubDataSourceOwnerResourceResultDataGenerator();

            Faker = new Faker<GithubDataSourceResourceResult>()
                    .RuleFor(_ => _.Id, faker => faker.Random.Int())
                    .RuleFor(_ => _.Archived, faker => faker.Random.Bool())
                    .RuleFor(_ => _.Disabled, faker => faker.Random.Bool())
                    .RuleFor(_ => _.Size, faker => faker.Random.Long())
                    .RuleFor(_ => _.Description, faker => faker.Name.JobDescriptor())
                    .RuleFor(_ => _.StargazersCount, faker => faker.Random.Long())
                    .RuleFor(_ => _.WatchersCount, faker => faker.Random.Long())
                    .RuleFor(_ => _.ForksCount, faker => faker.Random.Long())
                    .RuleFor(_ => _.Language, faker => faker.Address.Country())
                    .RuleFor(_ => _.HasIssues, faker => faker.Random.Bool())
                    .RuleFor(_ => _.HasProjects, faker => faker.Random.Bool())
                    .RuleFor(_ => _.HasDownloads, faker => faker.Random.Bool())
                    .RuleFor(_ => _.HasWiki, faker => faker.Random.Bool())
                    .RuleFor(_ => _.Owner, ownerGenerator.Generate())
                    .RuleFor(_ => _.HasPages, faker => faker.Random.Bool());
        }

    }

}
