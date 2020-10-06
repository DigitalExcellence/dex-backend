using Models;
using Moq;
using NUnit.Framework;
using Repositories;
using Repositories.Tests.DataSources;
using Services.Services;
using Services.Tests.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Tests
{
    [TestFixture]
    public class RoleServiceTest : ServiceTest<Role, RoleService, IRoleRepository>
    {
        
        protected new IRoleService Service => (IRoleService) base.Service;

        /// <summary>
        /// Test whether the repository method is called and no changes have been applied to the object
        /// </summary>
        /// <param name="roles">A set of 10 roles.</param>
        /// <returns></returns>
        [Test]
        public async Task GetAllAsync([RoleDataSource(10)] List<Role> roles)
        {

            RepositoryMock.Setup(
                repository => repository.GetAllAsync())
                .Returns(
                    Task.FromResult(roles)
                );

            List<Role> retrievedRoles = await Service.GetAllAsync();

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once);
            });

            Assert.AreEqual(roles, retrievedRoles);
            Assert.AreEqual(10, retrievedRoles.Count);
        }

        /// <summary>
        /// Tests if the get valid scopes returns the list of valid scopes as seen in defaults.Scopes.
        /// Please update when adding scopes.
        /// </summary>
        [Test]
        public void GetValidScopes()
        {
            List<string> currentScopes = new List<string>()
            {
                "ProjectRead",
                "ProjectWrite",
                "UserRead",
                "UserWrite",
                "HighlightRead",
                "HighlightWrite",
                "RoleRead",
                "RoleWrite",
                "EmbedWrite",
                "EmbedRead"
            };
            List<string> retrievedScopes = Service.GetValidScopes();
            Assert.AreEqual(currentScopes,retrievedScopes);
        }

        /// <summary>
        /// Tests if isValidScope returns true on an existing scope.
        /// </summary>
        [Test]
        public void isValidScope_true()
        {
            List<string> currentScopes = new List<string>()
                                         {
                                             "ProjectRead",
                                             "ProjectWrite",
                                             "UserRead",
                                             "UserWrite",
                                             "HighlightRead",
                                             "HighlightWrite",
                                             "RoleRead",
                                             "RoleWrite",
                                             "EmbedWrite",
                                             "EmbedRead"
                                         };
            bool isValidScope = Service.IsValidScope("RoleRead");

            Assert.IsTrue(isValidScope);
        }

        /// <summary>
        /// Tests if isValidScope returns false on a non existing scope.
        /// </summary>
        [Test]
        public void isValidScope_false()
        {
            bool isValidScope = Service.IsValidScope("role:read");

            Assert.IsFalse(isValidScope);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void AddRangeTest_GoodFlow([RoleDataSource(10)]IEnumerable<Role> entities)
        {
            base.AddRangeTest_GoodFlow(entities);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void AddTest_GoodFlow([RoleDataSource] Role entity)
        {
            base.AddTest_GoodFlow(entity);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override Task FindAsyncTest_GoodFlow([RoleDataSource] Role entity)
        {
            return base.FindAsyncTest_GoodFlow(entity);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override Task GetAll([RoleDataSource(10)]List<Role> entities)
        {
            return base.GetAll(entities);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void Remove([RoleDataSource] Role entity)
        {
            base.Remove(entity);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public Task RemoveAsync()
        {
            return base.RemoveAsync(1);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void Save()
        {
            base.Save();
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void Update([RoleDataSource] Role entity)
        {
            base.Update(entity);
        }
    }
}
