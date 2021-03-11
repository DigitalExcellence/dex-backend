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
using Models.Exceptions;
using Moq;
using NUnit.Framework;
using Services.ExternalDataProviders;
using Services.ExternalDataProviders.Resources;
using Services.Tests.Base;
using Services.Tests.ExternalDataProviders.DataSources.JsFiddle;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Services.Tests.ExternalDataProviders
{

    [TestFixture]
    public class JsFiddleDataSourceAdapteeTest : AdapteeTest<IJsFiddleDataSourceAdaptee>
    {

        [Test]
        public async Task FetchAllFiddlesFromUser_GoodFlow(
            [JsFiddleDataSourceResourceResultDataSource(50)] IEnumerable<JsFiddleDataSourceResourceResult> resourceResults)
        {
            // Arrange
            MockRestClient(resourceResults, HttpStatusCode.OK);
            DataSourceAdaptee = new JsFiddleDataSourceAdaptee(ClientFactoryMock.Object, Mapper);

            // Act
            Action act = () => DataSourceAdaptee.FetchAllFiddlesFromUser(It.IsAny<string>());
            IEnumerable<JsFiddleDataSourceResourceResult> results = await DataSourceAdaptee.FetchAllFiddlesFromUser(It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            results.Should().BeEquivalentTo(resourceResults);
        }

        [Test]
        public void FetchAllFiddlesFromUser_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = new JsFiddleDataSourceAdaptee(ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchAllFiddlesFromUser(It.IsAny<string>());

            // Assert
            act.Should().ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);

        }

        [Test]
        public void GetPublicProjectById_GoodFlow()
        {
            // Arrange
            DataSourceAdaptee = new JsFiddleDataSourceAdaptee(ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchPublicFiddleFromUri(It.IsAny<Uri>());

            // Assert
            act.Should().ThrowExactly<NotSupportedByExternalApiException>()
               .WithMessage("JsFiddle does not support the GetPublicProjectFromUri functionality");
        }

        [Test]
        public void FetchPublicFiddleById_GoodFlow()
        {
            // Arrange
            DataSourceAdaptee = new JsFiddleDataSourceAdaptee(ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchPublicFiddleById(It.IsAny<string>());

            // Assert
            act.Should().ThrowExactly<NotSupportedByExternalApiException>()
               .WithMessage("JsFiddle does not support the GetPublicProjectById functionality");
        }

    }

}
