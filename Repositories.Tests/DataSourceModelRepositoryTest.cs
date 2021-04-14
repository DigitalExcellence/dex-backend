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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Tests
{

    /// <summary>
    ///     The CallToActionOptionRepositoryTest class will test the methods in the CallToActionOptionRepository.
    /// </summary>
    /// <seealso cref="RepositoryTest{TDomain,TRepository}" />
    public class DataSourceModelRepositoryTest : RepositoryTest<DataSource, DataSourceModelRepository>
    {

        protected new IDataSourceModelRepository Repository => base.Repository;

        [Test]
        public async Task GetDataSourceByGuid_GoodFlow([DataSourceModelDataSource(100)] List<DataSource> dataSources)
        {
            // Arrange
            Random rnd = new Random();

            string guid = dataSources[rnd.Next(0, 100)]
                .Guid;
            DataSource dataSourceWithSpecifiedGuid = dataSources.FirstOrDefault(d => d.Guid == guid);

            await DbContext.AddRangeAsync(dataSources);
            await DbContext.SaveChangesAsync();

            // Act
            DataSource dataSource = await Repository.GetDataSourceByGuid(guid);

            // Assert
            dataSource.Should()
                      .Be(dataSourceWithSpecifiedGuid);
        }

        [Test]
        public async Task GetDataSourceByGuid_NoDataSourceFoundWithSpecifiedGuid()
        {
            // Arrange
            string guid = Guid.NewGuid()
                              .ToString();

            // Act
            DataSource dataSource = await Repository.GetDataSourceByGuid(guid);

            // Assert
            dataSource.Should()
                      .BeNull();
        }

        [Test]
        public async Task GetDataSourceByName_GoodFlow([DataSourceModelDataSource(100)] List<DataSource> dataSources)
        {
            // Arrange
            Random rnd = new Random();

            string name = dataSources[rnd.Next(0, 100)]
                .Title;
            DataSource dataSourceWithSpecifiedName = dataSources.FirstOrDefault(d => d.Title == name);

            await DbContext.AddRangeAsync(dataSources);
            await DbContext.SaveChangesAsync();

            // Act
            DataSource dataSource = await Repository.GetDataSourceByName(name);

            // Assert
            dataSource.Should()
                      .Be(dataSourceWithSpecifiedName);
        }

        [Test]
        public async Task GetDataSourceByName_NoDataSourceFoundWithSpecifiedName()
        {
            // Arrange
            string name = string.Empty;

            // Act
            DataSource dataSource = await Repository.GetDataSourceByName(name);

            // Assert
            dataSource.Should()
                      .BeNull();
        }

                /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task AddAsyncTest_GoodFlow([DataSourceModelDataSource] DataSource entity)
        {
            return base.AddAsyncTest_GoodFlow(entity);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override void AddRangeTest_BadFlow_EmptyList()
        {
            base.AddRangeTest_BadFlow_EmptyList();
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override void AddRangeTest_BadFlow_Null()
        {
            base.AddRangeTest_BadFlow_Null();
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task AddRangeTest_GoodFlow([DataSourceModelDataSource(10)] List<DataSource> entities)
        {
            return base.AddRangeTest_GoodFlow(entities);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override void AddTest_BadFlow_Null()
        {
            base.AddTest_BadFlow_Null();
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task FindAsyncTest_BadFlow_NotExists([DataSourceModelDataSource] DataSource entity)
        {
            return base.FindAsyncTest_BadFlow_NotExists(entity);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task FindAsyncTest_GoodFlow([DataSourceModelDataSource] DataSource entity)
        {
            return base.FindAsyncTest_GoodFlow(entity);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task GetAllAsyncTest_Badflow_Empty()
        {
            return base.GetAllAsyncTest_Badflow_Empty();
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task GetAllAsyncTest_GoodFlow([DataSourceModelDataSource(10)] List<DataSource> entities)
        {
            return base.GetAllAsyncTest_GoodFlow(entities);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task RemoveAsyncTest_BadFlow_NotExists([DataSourceModelDataSource] DataSource entity)
        {
            return base.RemoveAsyncTest_BadFlow_NotExists(entity);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task RemoveAsyncTest_GoodFlow([DataSourceModelDataSource] DataSource entity)
        {
            return base.RemoveAsyncTest_GoodFlow(entity);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task UpdateTest_BadFlow_NotExists([DataSourceModelDataSource] DataSource entity,
                                                          [DataSourceModelDataSource] DataSource updateEntity)
        {
            return base.UpdateTest_BadFlow_NotExists(entity, updateEntity);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task UpdateTest_BadFlow_Null([DataSourceModelDataSource] DataSource entity)
        {
            return base.UpdateTest_BadFlow_Null(entity);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task UpdateTest_GoodFlow([DataSourceModelDataSource] DataSource entity,
                                                 [DataSourceModelDataSource] DataSource updateEntity)
        {
            return base.UpdateTest_GoodFlow(entity, updateEntity);
        }

    }

}
