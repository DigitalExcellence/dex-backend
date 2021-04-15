
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
using NUnit.Framework;
using Repositories.Tests.Base;
using Repositories.Tests.DataSources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Tests
{

    [TestFixture]
    public class CategoryRepositoryTest : RepositoryTest<Category, CategoryRepository>
    {

        protected new ICategoryRepository Repository => base.Repository;

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task AddAsyncTest_GoodFlow([CategoryDataSource] Category entity)
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
        public override Task AddRangeTest_GoodFlow([CategoryDataSource(10)] List<Category> entities)
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
        public override Task FindAsyncTest_BadFlow_NotExists([CategoryDataSource] Category entity)
        {
            return base.FindAsyncTest_BadFlow_NotExists(entity);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task FindAsyncTest_GoodFlow([CategoryDataSource] Category entity)
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
        public override Task GetAllAsyncTest_GoodFlow([CategoryDataSource(10)] List<Category> entities)
        {
            return base.GetAllAsyncTest_GoodFlow(entities);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task RemoveAsyncTest_BadFlow_NotExists([CategoryDataSource] Category entity)
        {
            return base.RemoveAsyncTest_BadFlow_NotExists(entity);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task RemoveAsyncTest_GoodFlow([CategoryDataSource] Category entity)
        {
            return base.RemoveAsyncTest_GoodFlow(entity);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task UpdateTest_BadFlow_NotExists([CategoryDataSource] Category entity,
                                                          [CategoryDataSource] Category updateEntity)
        {
            return base.UpdateTest_BadFlow_NotExists(entity, updateEntity);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task UpdateTest_BadFlow_Null([CategoryDataSource] Category entity)
        {
            return base.UpdateTest_BadFlow_Null(entity);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task UpdateTest_GoodFlow([CategoryDataSource] Category entity, [CategoryDataSource] Category updateEntity)
        {
            return base.UpdateTest_GoodFlow(entity, updateEntity);
        }

    }

}
