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

using FluentAssertions;
using Models;
using NUnit.Framework;
using Repositories.Tests.Base;
using Repositories.Tests.DataSources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Tests
{

    /// <summary>
    /// The CallToActionOptionRepositoryTest class will test the methods in the CallToActionOptionRepository.
    /// </summary>
    /// <seealso cref="RepositoryTest{CallToActionOption,CallToActionOptionRepository}"/>
    public class CallToActionOptionRepositoryTest : RepositoryTest<CallToActionOption, CallToActionOptionRepository>
    {

        /// <summary>
        /// Gets the repository
        /// </summary>
        /// <value>
        /// The repository</value>
        protected new ICallToActionOptionRepository Repository => base.Repository;

        /// <summary>
        /// This method tests the GetCallToActionOptionsFromTypeAsync method in a good flow scenario.
        /// </summary>
        /// <param name="options">The call to action options stored, generated to mock the options from the database.</param>
        /// <returns>This method will return a passing result for the test.</returns>
        [Test]
        public async Task GetCallToActionOptionsFromTypeAsync_GoodFlow(
            [CallToActionOptionDataSource(30)] IEnumerable<CallToActionOption> options)
        {
            // Arrange
            DbContext.AddRange(options);
            await DbContext.SaveChangesAsync();

            // Act
            IEnumerable<CallToActionOption> retrievedOptions =
                await Repository.GetCallToActionOptionsFromTypeAsync(short.MaxValue);

            // Assert
            retrievedOptions.Count()
                .Should().Be(30);
            retrievedOptions
                .Should().BeEquivalentTo(options);
        }

        /// <summary>
        /// This method tests the GetCallToActionOptionsFromTypeAsync method whenever there are no call to action options found
        /// with the specified type identifier.
        /// </summary>
        /// <returns>This method will return a passing reuslt for the test.</returns>
        [Test]
        public async Task GetCallToActionOptionsFromTypeAsync_NoCallToActionOptionsFound()
        {
            // Arrange

            // Act
            IEnumerable<CallToActionOption> retrievedOptions =
                await Repository.GetCallToActionOptionsFromTypeAsync(short.MaxValue);

            // Assert
            retrievedOptions
                .Should().NotBeNull();
            retrievedOptions
                .Should().BeEmpty();

        }

    }

}
