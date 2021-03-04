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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Tests
{

    [TestFixture]
    public class CategoryServiceTest : ServiceTest<Category, CategoryService, ICategoryRepository>
    {

        protected new ICategoryService Service => base.Service;

        /// <summary>
        ///     Test whether the repository method is called and no changes have been applied to the object
        /// </summary>
        /// <param name="categories">A set of 10 categories.</param>
        /// <returns></returns>
        [Test]
        public async Task GetAllAsync([CategoryDataSource(10)] List<Category> categories)
        {
            RepositoryMock.Setup(repository => repository.GetAllAsync())
                          .Returns(Task.FromResult(categories));

            List<Category> retrievedCategories = await Service.GetAllAsync();

            Assert.DoesNotThrow(() => { RepositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once); });

            Assert.AreEqual(categories, retrievedCategories);
            Assert.AreEqual(10, retrievedCategories.Count);
        }




        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void AddRangeTest_GoodFlow([CategoryDataSource(10)] IEnumerable<Category> entities)
        {
            base.AddRangeTest_GoodFlow(entities);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void AddTest_GoodFlow([CategoryDataSource] Category entity)
        {
            base.AddTest_GoodFlow(entity);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override Task FindAsyncTest_GoodFlow([CategoryDataSource] Category entity)
        {
            return base.FindAsyncTest_GoodFlow(entity);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override Task GetAll([CategoryDataSource(10)] List<Category> entities)
        {
            return base.GetAll(entities);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void Remove([CategoryDataSource] Category entity)
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
        public override void Update([CategoryDataSource] Category entity)
        {
            base.Update(entity);
        }

    }

}
