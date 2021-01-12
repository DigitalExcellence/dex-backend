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
    /// InstitutionServiceTest
    /// </summary>
    /// <seealso cref="IInstitutionRepository" />
    [TestFixture]
    public class InstitutionServiceTest : ServiceTest<Institution, InstitutionService, IInstitutionRepository>
    {

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        protected new IInstitutionService Service => base.Service;

        /// <summary>
        /// This method tests the GetInstitutionsAsync method in a good flow.
        /// </summary>
        /// <param name="institutions">The institutions stored generated to mock the institutions from the repository.</param>
        /// <returns>This method will return a passing result for the test.</returns>
        [Test]
        public async Task GetInstitutionsAsync_GoodFlow([InstitutionDataSource(30)]IEnumerable<Institution> institutions)
        {
            // Arrange
            RepositoryMock
                .Setup(repository => repository.GetInstitutionsAsync())
                .ReturnsAsync(institutions);

            // Act
            IEnumerable<Institution> actualInstitutions = await Service.GetInstitutionsAsync();
            Action act = () => RepositoryMock.Verify(repository => repository.GetInstitutionsAsync(), Times.Once);

            // Assert
            act.Should().NotThrow();
            actualInstitutions.Should().Contain(institutions);
            actualInstitutions.Count().Should().Be(30);
        }

        /// <summary>
        /// This method tests the GetInstitutionsAsync method whenever there are no institutions stored.
        /// </summary>
        /// <returns>This method will return a passing result for the test.</returns>
        [Test]
        public async Task GetInstitutionAsync_NoInstitutions()
        {
            // Arrange
            RepositoryMock
                .Setup(repository => repository.GetInstitutionsAsync())
                .ReturnsAsync(Enumerable.Empty<Institution>());

            // Act
            IEnumerable<Institution> actualInstitutions = await Service.GetInstitutionsAsync();
            Action act = () => RepositoryMock.Verify(repository => repository.GetInstitutionsAsync(), Times.Once);

            // Assert
            actualInstitutions.Should().BeEmpty();
            act.Should().NotThrow();
        }

        /// <summary>
        /// This method tests the GetInstitutionByInstitutionIdentityId method whenever it returns an institution.
        /// </summary>
        /// <param name="institution">The institution, generated to mock the institution found from the repository.</param>
        /// <returns>This method will return a passing result for the test.</returns>
        [Test]
        public async Task GetInstitutionByInstitutionIdentityId_ExistingIdentityId([InstitutionDataSource(1)] Institution institution)
        {
            // Arrange
            RepositoryMock
                .Setup(repository => repository.GetInstitutionByInstitutionIdentityId(It.IsAny<string>()))
                .ReturnsAsync(institution);

            // Act
            Institution actualInstitution = await Service.GetInstitutionByInstitutionIdentityId(string.Empty);
            Action act = () => RepositoryMock.Verify(repository => repository.GetInstitutionByInstitutionIdentityId(string.Empty), Times.Once);

            // Assert
            act.Should().NotThrow();
            actualInstitution.Should()
                             .Be(institution);
            actualInstitution.Should()
                             .NotBeNull();
        }

        /// <summary>
        /// This method tests the GetInstitutionByInstitutionIdentityId method whenever it can't find an institution.
        /// </summary>
        /// <returns>This method will return a passing result for the test.</returns>
        [Test]
        public async Task GetInstitutionByInstitutionIdentityId_NoIdentityId()
        {
            // Arrange
            RepositoryMock
                .Setup(repository => repository.GetInstitutionByInstitutionIdentityId(It.IsAny<string>()))
                .ReturnsAsync((Institution)null);

            // Act
            Institution actualInstitution = await Service.GetInstitutionByInstitutionIdentityId(string.Empty);
            Action act = () => RepositoryMock.Verify(repository => repository.GetInstitutionByInstitutionIdentityId(string.Empty), Times.Once);

            // Assert
            actualInstitution.Should().BeNull();
            act.Should().NotThrow();
        }

    }

}
