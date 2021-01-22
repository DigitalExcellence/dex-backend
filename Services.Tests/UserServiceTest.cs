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
using Services.Resources;
using Services.Services;
using Services.Tests.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Tests
{
    [TestFixture]
    public class UserServiceTest : ServiceTest<User, UserService, IUserRepository>
    {
        protected new IUserService Service;
        protected  Mock<IUserRepository> UserRepositoryMock;
        protected Mock<IProjectRepository> ProjectRepositoryMock;
        protected Mock<ElasticConfig> ElasticConfig;

        [SetUp]
        public override void  Initialize()
        {
            // Mock the repository
            UserRepositoryMock = new Mock<IUserRepository>();
            ProjectRepositoryMock = new Mock<IProjectRepository>();
            ElasticConfig = new Mock<ElasticConfig>();

            Service = new UserService(UserRepositoryMock.Object, ProjectRepositoryMock.Object, ElasticConfig.Object);
        }
        /// <summary>
        /// Test if repository method is called
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetUserAsync_GoodFlow()
        {
            int userId = 1;
            User user = new User();

            UserRepositoryMock.Setup(
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

            UserRepositoryMock.Setup(
                repository => repository.RemoveUserAsync(userId));

            await Service.RemoveUserAsync(userId);

            Assert.DoesNotThrow(() => {
                UserRepositoryMock.Verify(repository => repository.RemoveUserAsync(userId), Times.Once);
            });
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void AddRangeTest_GoodFlow([UserDataSource(50)]IEnumerable<User> entities)
        {
            UserRepositoryMock.Setup(
               repository => repository.AddRange(entities));

            Service.AddRange(entities);

            Assert.DoesNotThrow(() => {
                UserRepositoryMock.Verify(repository => repository.AddRange(entities), Times.Once);
            });
        }

        


        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void AddTest_GoodFlow([UserDataSource]User entity)
        {
            UserRepositoryMock.Setup(
                repository => repository.Add(entity));

            Service.Add(entity);

            Assert.DoesNotThrow(() => {
                UserRepositoryMock.Verify(repository => repository.Add(entity), Times.Once);
            });
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override async Task FindAsyncTest_GoodFlow([UserDataSource]User entity)
        {
            UserRepositoryMock.Setup(
                repository => repository.FindAsync(1))
                .Returns(
                    Task.FromResult(entity)
                );

            User retrievedEntity = await Service.FindAsync(1);

            Assert.DoesNotThrow(() => {
                UserRepositoryMock.Verify(repository => repository.FindAsync(1), Times.Once);
            });

            Assert.AreEqual(entity, retrievedEntity);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override async Task GetAll([UserDataSource(50)]List<User> entities)
        {
            int amountToTest = entities.Count;
            UserRepositoryMock.Setup(
                repository => repository.GetAll())
                .Returns(
                    Task.FromResult((IEnumerable<User>) entities)
                );

            List<User> retrievedList = (List<User>) await Service.GetAll();

            Assert.DoesNotThrow(() => {
                UserRepositoryMock.Verify(repository => repository.GetAll(), Times.Once);
            });

            Assert.AreEqual(amountToTest, retrievedList.Count);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void Remove([UserDataSource]User entity)
        {
            UserRepositoryMock.Setup(
                repository => repository.Remove(entity));

            Service.Remove(entity);

            Assert.DoesNotThrow(() => {
                UserRepositoryMock.Verify(repository => repository.Remove(entity), Times.Once);
            });
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public  async Task RemoveAsync()
        {
            UserRepositoryMock.Setup(
                repository => repository.RemoveAsync(1));

            await Service.RemoveAsync(1);

            Assert.DoesNotThrow(() => {
                UserRepositoryMock.Verify(repository => repository.RemoveAsync(1), Times.Once);
            });
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void Save()
        {
            UserRepositoryMock.Setup(
                repository => repository.Save());

            Service.Save();

            Assert.DoesNotThrow(() => {
                UserRepositoryMock.Verify(repository => repository.Save(), Times.Once);
            });
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void Update([UserDataSource]User entity)
        {
            UserRepositoryMock.Setup(
                repository => repository.Update(entity));

            Service.Update(entity);

            Assert.DoesNotThrow(() => {
                UserRepositoryMock.Verify(repository => repository.Update(entity), Times.Once);
            });
        }
    }
}
