using Models;
using NUnit.Framework;
using Repositories.Tests.Base;
using Repositories.Tests.DataSources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Tests
{
    [TestFixture]
    public class UserRepositoryTest : RepositoryTest<User, UserRepository>
    {
        protected new IUserRepository Repository => (IUserRepository)base.Repository;

        /// <summary>
        /// User is retrieved correctly
        /// </summary>
        [Test]
        public async Task GetUserAsync_GoodFlow([UserDataSource]User entity)
        {
            DbContext.Add(entity);
            await DbContext.SaveChangesAsync();

            //Test
            User retrieved = await Repository.GetUserAsync(entity.Id);

            Assert.AreEqual(entity, retrieved);
        }

        /// <summary>
        /// User not added and thus not found in repo
        /// </summary>
        [Test]
        public async Task GetUserAsync_NotFound([UserDataSource]User entity)
        {
            //Test
            User retrieved = await Repository.GetUserAsync(entity.Id);

            Assert.IsNull(retrieved);
        }

        /// <summary>
        /// User correctly removed from repo
        /// </summary>
        [Test]
        public async Task RemoveUserAsync_GoodFlow([UserDataSource]User entity)
        {
            DbContext.Add(entity);
            await DbContext.SaveChangesAsync();

            //Test
            bool removed = await Repository.RemoveUserAsync(entity.Id);
            await DbContext.SaveChangesAsync();

            User retrieved = await Repository.GetUserAsync(entity.Id);

            Assert.IsTrue(removed);
            Assert.IsNull(retrieved);
        }

        /// <summary>
        /// User that was not in repo not found
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Test]
        public async Task RemoveUserAsync_NotFound([UserDataSource]User entity)
        {
            //Test
            bool removed = await Repository.RemoveUserAsync(entity.Id);
            await DbContext.SaveChangesAsync();

            Assert.IsFalse(removed);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task AddAsyncTest_GoodFlow([UserDataSource]User entity)
        {
            return base.AddAsyncTest_GoodFlow(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override void AddRangeTest_BadFlow_EmptyList()
        {
            base.AddRangeTest_BadFlow_EmptyList();
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override void AddRangeTest_BadFlow_Null()
        {
            base.AddRangeTest_BadFlow_Null();
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task AddRangeTest_GoodFlow([UserDataSource(50)]List<User> entities)
        {
            return base.AddRangeTest_GoodFlow(entities);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override void AddTest_BadFlow_Null()
        {
            base.AddTest_BadFlow_Null();
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task FindAsyncTest_BadFlow_NotExists([UserDataSource]User entity)
        {
            return base.FindAsyncTest_BadFlow_NotExists(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task FindAsyncTest_GoodFlow([UserDataSource]User entity)
        {
            return base.FindAsyncTest_GoodFlow(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task GetAllAsyncTest_Badflow_Empty()
        {
            return base.GetAllAsyncTest_Badflow_Empty();
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task GetAllAsyncTest_GoodFlow([UserDataSource(50)]List<User> entities)
        {
            return base.GetAllAsyncTest_GoodFlow(entities);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task RemoveAsyncTest_BadFlow_NotExists([UserDataSource]User entity)
        {
            return base.RemoveAsyncTest_BadFlow_NotExists(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task RemoveAsyncTest_GoodFlow([UserDataSource]User entity)
        {
            return base.RemoveAsyncTest_GoodFlow(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task UpdateTest_BadFlow_NotExists([UserDataSource]User entity, [UserDataSource]User updateEntity)
        {
            return base.UpdateTest_BadFlow_NotExists(entity, updateEntity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task UpdateTest_BadFlow_Null([UserDataSource]User entity)
        {
            return base.UpdateTest_BadFlow_Null(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task UpdateTest_GoodFlow([UserDataSource]User entity, [UserDataSource]User updateEntity)
        {
            return base.UpdateTest_GoodFlow(entity, updateEntity);
        }
    }
}
