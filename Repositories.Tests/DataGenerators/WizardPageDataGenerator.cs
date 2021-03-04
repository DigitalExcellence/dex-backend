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
using System.Collections.Generic;

namespace Repositories.Tests.DataGenerators
{

    /// <summary>
    ///     Fake data generator for wizard pages.
    /// </summary>
    public class WizardPageDataGenerator : FakeDataGenerator<WizardPage>
    {

        private int orderIndexPublicFlow;
        private int orderIndexOAuthFlow;

        private readonly Random random = new Random();

        /// <summary>
        ///     Initializes the WizardPageDataGenerator
        ///     and define dataGenerator options.
        /// </summary>
        public WizardPageDataGenerator()
        {
            int amountToGenerate = 5;

            Faker = new Faker<WizardPage>()
                    .RuleFor(option => option.Id, faker => faker.Random.Int(1))
                    .RuleFor(option => option.Name, faker => faker.Name.FirstName())
                    .RuleFor(option => option.Description, faker => faker.Name.JobDescriptor())
                    .RuleFor(option => option.DataSourceWizardPages,
                             faker => GenerateFakeDataSourceWizardPages(amountToGenerate));
        }

        /// <summary>
        ///     This method is responsible for generating an amount of data source wizard pages.
        /// </summary>
        /// <param name="amount">The amount of wizard pages that have to be generated.</param>
        /// <returns>This method returns a collection of data source wizard pages.</returns>
        private List<DataSourceWizardPage> GenerateFakeDataSourceWizardPages(int amount)
        {
            List<DataSourceWizardPage> dataSourceWizardPages = new List<DataSourceWizardPage>();
            for(int i = 0; i < amount; i++)
            {
                dataSourceWizardPages.Add(GenerateFakeDataSourceWizardPage());
            }
            return dataSourceWizardPages;
        }

        /// <summary>
        ///     This method is responsible for generating a data source wizard page
        ///     and fines the data generation options.
        /// </summary>
        /// <returns></returns>
        private Faker<DataSourceWizardPage> GenerateFakeDataSourceWizardPage()
        {

            bool isFollowingOauthFlow = random.Next(0, 2) > 0;
            int currentIndex = 0;

            if(isFollowingOauthFlow)
            {
                orderIndexOAuthFlow++;
                currentIndex = orderIndexOAuthFlow;
            } else
            {
                orderIndexPublicFlow++;
                currentIndex = orderIndexPublicFlow;
            }

            return new Faker<DataSourceWizardPage>()
                   .RuleFor(option => option.AuthFlow,
                            faker => isFollowingOauthFlow)
                   .RuleFor(option => option.OrderIndex,
                            faker => currentIndex)
                   .RuleFor(option => option.DataSourceId,
                            faker => faker.Random.Int(1, 10));

        }

    }

}
