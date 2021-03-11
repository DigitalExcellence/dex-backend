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


using Models;
using Moq;
using NUnit.Framework;
using Repositories.Tests.DataSources;
using Services.ExternalDataProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Tests.ExternalDataProviders
{

    /// <summary>
    ///     The data provider service tests class. This class tests the logic in the data provider service.
    /// </summary>
    /// <seealso cref="DataProviderServiceTest" />
    [TestFixture]
    public class DataProviderServiceTest
    {

        private IDataProviderService service;

        private Mock<IDataProviderLoader> loaderMock;

        private Mock<IDataSourceAdaptee> dataSourceMock;

        /// <summary>
        ///     Initialize runs before every test
        ///     Mock the data provider loader
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            dataSourceMock = new Mock<IDataSourceAdaptee>();

            loaderMock = new Mock<IDataProviderLoader>();
            loaderMock.Setup(_ => _.GetDataSourceByGuid(It.IsAny<string>()))
                      .ReturnsAsync(dataSourceMock.Object);
            loaderMock.Setup(_ => _.GetDataSourceByName(It.IsAny<string>()))
                      .ReturnsAsync(dataSourceMock.Object);

            service = new DataProviderService(loaderMock.Object);
        }

        public async Task GetAllProjects_GoodFlow([ProjectDataSource(50)] IEnumerable<Project> projects)
        {
            // Arrange

            // Act
            Action act = () => service.GetAllProjects(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>());
            IEnumerable<Project> retrievedProjects =
                await service.GetAllProjects(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>());

            // Assert


        }

    }

}
