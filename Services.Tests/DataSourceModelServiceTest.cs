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
using System.Threading.Tasks;

namespace Services.Tests
{

    /// <summary>
    ///     The data source model service tests class. This class tests the logic in the data source model service.
    /// </summary>
    /// <seealso cref="IDataSourceModelRepository" />
    public class
        DataSourceModelServiceTest : ServiceTest<DataSource, DataSourceModelService, IDataSourceModelRepository>
    {

        /// <summary>
        ///     The service
        /// </summary>
        protected new IDataSourceModelService Service => base.Service;

        /// <summary>
        ///     Get datasource by GUID. Good flow
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        [Test]
        public async Task GetDataSourceByGuid_GoodFlow([DataSourceModelDataSource(1)] DataSource dataSource)
        {
            // Arrange
            RepositoryMock.Setup(repository => repository.GetDataSourceByGuid(It.IsAny<string>()))
                          .ReturnsAsync(dataSource);

            // Act
            DataSource actualDataSource = await Service.GetDataSourceByGuid(It.IsAny<string>());
            Action act = () =>
                RepositoryMock.Verify(repository => repository.GetDataSourceByGuid(It.IsAny<string>()), Times.Once);

            // Assert
            act.Should()
               .NotThrow();

            actualDataSource.Should()
                            .NotBeNull();

            actualDataSource.Should()
                            .Be(dataSource);
        }

        /// <summary>
        ///     The get data source by guid bad flow test
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetDataSourceByGuid_NoDataSourceFound()
        {
            // Arrange
            RepositoryMock.Setup(repository => repository.GetDataSourceByGuid(It.IsAny<string>()))
                          .ReturnsAsync((DataSource) null);

            // Act
            DataSource actualDataSource = await Service.GetDataSourceByGuid(It.IsAny<string>());
            Action act = () =>
                RepositoryMock.Verify(repository => repository.GetDataSourceByGuid(It.IsAny<string>()), Times.Once);

            // Assert
            actualDataSource.Should()
                            .Be(null);

            act.Should()
               .NotThrow();
        }

    }

}
