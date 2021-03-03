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
    ///     WizardPageServiceTest
    /// </summary>
    /// <seealso cref="IWizardPageRepository" />
    [TestFixture]
    public class WizardPageServiceTest : ServiceTest<WizardPage, WizardPageService, IWizardPageRepository>
    {

        /// <summary>
        ///     This methods tests the ValidateWizardPagesExists method in a good flow where the wizard pages do exist.
        /// </summary>
        /// <param name="wizardPages">A collection of 10 generated wizard pages.</param>
        /// <returns></returns>
        [Test]
        public async Task ValidateWizardPagesExist_WizardPageExists([WizardPageDataSource(10)] IEnumerable<WizardPage> wizardPages)
        {
            // Arrange
            RepositoryMock.Setup(repository => repository.GetRange(It.IsAny<IEnumerable<int>>()))
                          .ReturnsAsync(wizardPages);

            // Act
            IEnumerable<int> wizardPagesToValidate = wizardPages.Skip(6)
                                                                .Select(wizardPage => wizardPage.Id);
            bool wizardPagesExist = await Service.ValidateWizardPagesExist(wizardPagesToValidate);

            // Assert
            Action act = () =>
                RepositoryMock.Verify(repository => repository.GetRange(It.IsAny<IEnumerable<int>>()),
                                      Times.Once());
            act.Should().NotThrow();
            wizardPagesExist.Should().BeTrue();
        }

        /// <summary>
        ///     This methods tests the ValidateWizardPagesExists method in a bad flow where the wizard pages do not exist.
        /// </summary>
        /// <param name="wizardPages">A collection of 5 generated wizard pages.</param>
        /// <returns></returns>
        [Test]
        public async Task ValidateWizardPagesExist_WizardPageDoNotExists([WizardPageDataSource(5)] IEnumerable<WizardPage> wizardPages)
        {
            // Arrange
            RepositoryMock.Setup(repository => repository.GetRange(It.IsAny<IEnumerable<int>>()))
                          .ReturnsAsync((IEnumerable<WizardPage>) null);

            // Act
            IEnumerable<int> wizardPagesToValidate = wizardPages.Select(wizardPage => wizardPage.Id);
            bool wizardPagesExist = await Service.ValidateWizardPagesExist(wizardPagesToValidate);

            // Assert
            Action act = () =>
                RepositoryMock.Verify(repository => repository.GetRange(It.IsAny<IEnumerable<int>>()),
                                      Times.Once());
            act.Should().NotThrow();
            wizardPagesExist.Should().BeFalse();
        }


        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void AddRangeTest_GoodFlow([WizardPageDataSource(10)] IEnumerable<WizardPage> entities)
        {
            base.AddRangeTest_GoodFlow(entities);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void AddTest_GoodFlow([WizardPageDataSource] WizardPage entity)
        {
            base.AddTest_GoodFlow(entity);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override Task FindAsyncTest_GoodFlow([WizardPageDataSource] WizardPage entity)
        {
            return base.FindAsyncTest_GoodFlow(entity);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override Task GetAll([WizardPageDataSource(10)] List<WizardPage> entities)
        {
            return base.GetAll(entities);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void Remove([WizardPageDataSource] WizardPage entity)
        {
            base.Remove(entity);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public Task RemoveAsync()
        {
            return base.RemoveAsync(1);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void Save()
        {
            base.Save();
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void Update([WizardPageDataSource] WizardPage entity)
        {
            base.Update(entity);
        }


    }

}
