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
    ///     FakeDataGenerator for a GithubDataSourceLicenseResourceResult
    /// </summary>
    public class GithubDataSourceLicenseResourceResultDataGenerator
        : FakeDataGenerator<GithubDataSourceLicenseResourceResult>
    {

        /// <summary>
        ///     Initializes the GithubDataSourceLicenseResourceResultDataGenerator
        ///     and define dataGenerator options.
        /// </summary>
        public GithubDataSourceLicenseResourceResultDataGenerator()
        {
            Faker = new Faker<GithubDataSourceLicenseResourceResult>()
                .RuleFor(_ => _.Name, faker => faker.Name.FirstName());
        }

    }

}
