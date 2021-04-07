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
    ///     FakeDataGenerator for a GithubDataSourceContributorResourceResult
    /// </summary>
    public class GithubDataSourceContributorResourceResultDataGenerator
        : FakeDataGenerator<GithubDataSourceContributorResourceResult>
    {

        /// <summary>
        ///     Initializes the GithubDataSourceContributorResourceResultDataGenerator
        ///     and define data generator options.
        /// </summary>
        public GithubDataSourceContributorResourceResultDataGenerator()
        {
            Faker = new Faker<GithubDataSourceContributorResourceResult>()
                .RuleFor(_ => _.Login, faker => faker.Name.FirstName())
                .RuleFor(_ => _.Id, faker => faker.Random.Long())
                .RuleFor(_ => _.Contributions, faker => faker.Random.Long())
                .RuleFor(_ => _.AvatarUrl, faker => new Uri(faker.Image.PicsumUrl()))
                .RuleFor(_ => _.Url, faker => new Uri(faker.Image.PicsumUrl()))
                .RuleFor(_ => _.HtmlUrl, faker => new Uri(faker.Image.PicsumUrl()))
                .RuleFor(_ => _.FollowersUrl, faker => new Uri(faker.Image.PicsumUrl()))
                .RuleFor(_ => _.SubscriptionsUrl, faker => new Uri(faker.Image.PicsumUrl()))
                .RuleFor(_ => _.OrganizationsUrl, faker => new Uri(faker.Image.PicsumUrl()))
                .RuleFor(_ => _.ReposUrl, faker => new Uri(faker.Image.PicsumUrl()))
                .RuleFor(_ => _.EventsUrl, faker => faker.Image.PicsumUrl())
                .RuleFor(_ => _.ReceivedEventsUrl, faker => new Uri(faker.Image.PicsumUrl()))
                .RuleFor(_ => _.SiteAdmin, faker => faker.Random.Bool())
                .RuleFor(_ => _.FollowingUrl, faker => faker.Image.PicsumUrl())
                .RuleFor(_ => _.StarredUrl, faker => faker.Image.PicsumUrl())
        }

    }

}
