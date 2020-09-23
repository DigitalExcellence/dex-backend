using Models;
using NUnit.Framework;
using Repositories.Tests.Base;
using Repositories.Tests.DataSources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Tests
{
    [TestFixture]
    public class RoleRepositoryTest : RepositoryTest<Role, RoleRepository>
    {
        protected new IRoleRepository Repository => base.Repository;

        /// <summary>
        /// Test if role with scope relations are retrieved correctly
        /// </summary>
        /// <param name="role">The role which is used as data to test.</param>
        [Test]
        public async Task GetAllAsync_one([RoleDataSource]Role role)
        {
            DbContext.Add(role);
            await DbContext.SaveChangesAsync();

            List<Role> retrievedRoles = await Repository.GetAllAsync();
            Assert.AreEqual(new List<Role>(){role},retrievedRoles);
        }

        /// <summary>
        /// Test if role with scope relations are retrieved correctly
        /// </summary>
        /// <param name="roles">The role which is used as data to test.</param>
        [Test]
        public async Task GetAllAsync_multiple([RoleDataSource(10)] List<Role> roles)
        {
            DbContext.AddRange(roles);
            await DbContext.SaveChangesAsync();

            List<Role> retrievedRoles = await Repository.GetAllAsync();
            Assert.AreEqual(roles, retrievedRoles);
        }

        /// <summary>
        /// Test if role with scope relations are retrieved correctly when there are no roles.
        /// </summary>
        [Test]
        public async Task GetAllAsync_NoRole()
        {
            List<Role> roles = await Repository.GetAllAsync();
            Assert.IsEmpty(roles);
        }

        /// <summary>
        /// Test if role with scope relations are retrieved correctly when a role doesnt have scopes..
        /// </summary>
        /// <param name="role">The role which is used as data to test.</param>
        [Test]
        public async Task GetAllAsync_no_Scopes([RoleDataSource] Role role)
        {
            role.Scopes = new List<RoleScope>();
            DbContext.Add(role);
            await DbContext.SaveChangesAsync();

            List<Role> retrievedRoles = await Repository.GetAllAsync();
            Assert.AreEqual(new List<Role>(){role},retrievedRoles);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task AddAsyncTest_GoodFlow([RoleDataSource]Role entity)
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
        public override Task AddRangeTest_GoodFlow([RoleDataSource(10)]List<Role> entities)
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
        public override Task FindAsyncTest_BadFlow_NotExists([RoleDataSource] Role entity)
        {
            return base.FindAsyncTest_BadFlow_NotExists(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task FindAsyncTest_GoodFlow([RoleDataSource] Role entity)
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
        public override Task GetAllAsyncTest_GoodFlow([RoleDataSource(10)]List<Role> entities)
        {
            return base.GetAllAsyncTest_GoodFlow(entities);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task RemoveAsyncTest_BadFlow_NotExists([RoleDataSource] Role entity)
        {
            return base.RemoveAsyncTest_BadFlow_NotExists(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task RemoveAsyncTest_GoodFlow([RoleDataSource] Role entity)
        {
            return base.RemoveAsyncTest_GoodFlow(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task UpdateTest_BadFlow_NotExists([RoleDataSource] Role entity, [RoleDataSource] Role updateEntity)
        {
            return base.UpdateTest_BadFlow_NotExists(entity, updateEntity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task UpdateTest_BadFlow_Null([RoleDataSource] Role entity)
        {
            return base.UpdateTest_BadFlow_Null(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task UpdateTest_GoodFlow([RoleDataSource] Role entity, [RoleDataSource] Role updateEntity)
        {
            return base.UpdateTest_GoodFlow(entity, updateEntity);
        }
    }
}
