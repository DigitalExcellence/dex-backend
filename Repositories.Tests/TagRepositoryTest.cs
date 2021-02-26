
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
    public class TagRepositoryTest : RepositoryTest<Tag, TagRepository>
    {

        protected new ITagRepository Repository => base.Repository;

        /// <summary>
        ///     Test if tag with scope relations are retrieved correctly
        /// </summary>
        /// <param name="tag">The tag which is used as data to test.</param>
        [Test]
        public async Task GetAllAsync_one([TagDataSource] Tag tag)
        {
            DbContext.Add(tag);
            await DbContext.SaveChangesAsync();

            List<Tag> retrievedTags = await Repository.GetAllAsync();
            Assert.AreEqual(new List<Tag>
                            {
                                tag
                            },
                            retrievedTags);
        }

        /// <summary>
        ///     Test if tag with scope relations are retrieved correctly
        /// </summary>
        /// <param name="tags">The tag which is used as data to test.</param>
        [Test]
        public async Task GetAllAsync_multiple([TagDataSource(10)] List<Tag> tags)
        {
            DbContext.AddRange(tags);
            await DbContext.SaveChangesAsync();

            List<Tag> retrievedTags = await Repository.GetAllAsync();
            Assert.AreEqual(tags, retrievedTags);
        }

        /// <summary>
        ///     Test if tag with scope relations are retrieved correctly when there are no tags.
        /// </summary>
        [Test]
        public async Task GetAllAsync_NoTag()
        {
            List<Tag> tags = await Repository.GetAllAsync();
            Assert.IsEmpty(tags);
        }


        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task AddAsyncTest_GoodFlow([TagDataSource] Tag entity)
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
        public override Task AddRangeTest_GoodFlow([TagDataSource(10)] List<Tag> entities)
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
        public override Task FindAsyncTest_BadFlow_NotExists([TagDataSource] Tag entity)
        {
            return base.FindAsyncTest_BadFlow_NotExists(entity);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task FindAsyncTest_GoodFlow([TagDataSource] Tag entity)
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
        public override Task GetAllAsyncTest_GoodFlow([TagDataSource(10)] List<Tag> entities)
        {
            return base.GetAllAsyncTest_GoodFlow(entities);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task RemoveAsyncTest_BadFlow_NotExists([TagDataSource] Tag entity)
        {
            return base.RemoveAsyncTest_BadFlow_NotExists(entity);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task RemoveAsyncTest_GoodFlow([TagDataSource] Tag entity)
        {
            return base.RemoveAsyncTest_GoodFlow(entity);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task UpdateTest_BadFlow_NotExists([TagDataSource] Tag entity,
                                                          [TagDataSource] Tag updateEntity)
        {
            return base.UpdateTest_BadFlow_NotExists(entity, updateEntity);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task UpdateTest_BadFlow_Null([TagDataSource] Tag entity)
        {
            return base.UpdateTest_BadFlow_Null(entity);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override Task UpdateTest_GoodFlow([TagDataSource] Tag entity, [TagDataSource] Tag updateEntity)
        {
            return base.UpdateTest_GoodFlow(entity, updateEntity);
        }

    }

}
