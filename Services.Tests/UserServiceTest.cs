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
using Repositories;
using Repositories.Tests.DataSources;
using Services.Services;
using Services.Tests.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Tests
{
    [TestFixture]
    public class UserServiceTest : ServiceTest<User, UserService, IUserRepository>
    {
        protected new IUserService Service => (IUserService) base.Service;

        /// <summary>
        /// Test if repository method is called
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetUserAsync_GoodFlow()
        {
            int userId = 1;
            User user = new User();

            RepositoryMock.Setup(
               repository => repository.GetUserAsync(userId))
               .Returns(
                   Task.FromResult(user)
               );

            //Test
            User received = await Service.GetUserAsync(userId);

            Assert.AreEqual(user, received);
        }

        /// <summary>
        /// Test if repository method is called
        /// </summary>
        /// <returns> assert if removeuser does not thow exceptions.</returns>
        [Test]
        public async Task RemoveUserAsync_GoodFlow()
        {
            int userId = 1;

            RepositoryMock.Setup(
                repository => repository.RemoveUserAsync(userId));

            await Service.RemoveUserAsync(userId);

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.RemoveUserAsync(userId), Times.Once);
            });
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void AddRangeTest_GoodFlow([UserDataSource(50)]IEnumerable<User> entities)
        {
            base.AddRangeTest_GoodFlow(entities);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void AddTest_GoodFlow([UserDataSource]User entity)
        {
            base.AddTest_GoodFlow(entity);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override Task FindAsyncTest_GoodFlow([UserDataSource]User entity)
        {
            return base.FindAsyncTest_GoodFlow(entity);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override Task GetAll([UserDataSource(50)]List<User> entities)
        {
            return base.GetAll(entities);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void Remove([UserDataSource]User entity)
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
        public override void Update([UserDataSource]User entity)
        {
            base.Update(entity);
        }
    }
}
