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
        ///     
        /// </summary>
        /// <param name="tag">The tag which is used as data to test.</param>
        [Test]
        public async Task FindByName_one([TagDataSource] Tag tag)
        {
            DbContext.Add(tag);
            await DbContext.SaveChangesAsync();

            Tag retrievedTag = Repository.FindByName(tag.Name);
            Assert.AreEqual(tag,
                            retrievedTag);
        }

        [Test]
        public async Task FindByNameAsync_one([TagDataSource] Tag tag)
        {
            DbContext.Add(tag);
            await DbContext.SaveChangesAsync();

            Tag retrievedTag = await Repository.FindByNameAsync(tag.Name);
            Assert.AreEqual(tag,
                            retrievedTag);
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
