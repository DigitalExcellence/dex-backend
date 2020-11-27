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

using AngleSharp.Io;
using FluentAssertions;
using Models;
using Moq;
using NUnit.Framework;
using Repositories;
using Repositories.Tests.DataSources;
using Services.Services;
using Services.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Tests
{

    /// <summary>
    /// CallToActionServiceTest
    /// </summary>
    /// <seealso cref="ICallToActionOptionRepository" />
    [TestFixture]
    public class CallToActionOptionServiceTest : ServiceTest<CallToActionOption, CallToActionOptionService, ICallToActionOptionRepository>
    {

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        protected new ICallToActionOptionService Service => base.Service;

        /// <summary>
        /// This method tests the GetCallToActionOptionsFromTypeAsync method in a good flow.
        /// </summary>
        /// <returns>This method will return a passing result for the test.</returns>
        [Test]
        public async Task GetCallToActionOptionsFromTypeAsync_GoodFlow([CallToActionOptionDataSource(30)] IEnumerable<CallToActionOption> options)
        {
            // Arrange
            RepositoryMock
                .Setup(repository => repository.GetCallToActionOptionsFromTypeAsync(It.IsAny<int>()))
                .ReturnsAsync(options);

            // Act
            IEnumerable<CallToActionOption> actualOptions =
                await Service.GetCallToActionOptionsFromTypeAsync(It.IsAny<int>());
            Action act = () =>
                RepositoryMock.Verify(repository => repository.GetCallToActionOptionsFromTypeAsync(It.IsAny<int>()),
                                      Times.Once());

            // Assert
            act.Should()
               .NotThrow();
            actualOptions
                .Should().Contain(options);
            actualOptions.Count()
                .Should().Be(30);

        }

        /// <summary>
        /// This method tests the GetCallToActionOptionsFromTypeAsync method whenever there are no call to actions
        /// found with the specified type identifier.
        /// </summary>
        /// <returns>This method will return a passing result for the test.</returns>
        [Test]
        public async Task GetCallToActionOptionsFromTypeAsync_NoOptionsFound()
        {
            // Arrange
            RepositoryMock
                .Setup(repository => repository.GetCallToActionOptionsFromTypeAsync(It.IsAny<int>()))
                .ReturnsAsync(Enumerable.Empty<CallToActionOption>());

            // Act
            IEnumerable<CallToActionOption> actualOptions =
                await Service.GetCallToActionOptionsFromTypeAsync(It.IsAny<int>());
            Action act = () =>
                RepositoryMock.Verify(repository => repository.GetCallToActionOptionsFromTypeAsync(It.IsAny<int>()),
                                      Times.Once());

            // Assert
            actualOptions.Should()
                         .BeEmpty();
            act.Should().NotThrow();
        }

    }

}
