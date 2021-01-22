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

using Moq;
using NUnit.Framework;
using Repositories.Base;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Services.Tests.Base
{
    /// <summary>
    /// Base test class which should be inherited from when creating unittests for the services.
    /// By inheriting, all the default tests are included in the new test class.
    /// YOU SHOULD OVERRIDE THE DEFAULT TEST TO ADD THE [Test] attribute.
    /// If you do not override the tests and add the [Test] attribute, the default tests will not be triggered.
    /// </summary>
    /// <typeparam name="TDomain">Modelclass which is used to test</typeparam>
    /// <typeparam name="TService">Service which should be tested</typeparam>
    /// <typeparam name="TRepository">IRepository which is used to mock the used Repository</typeparam>
    public abstract class ServiceTest<TDomain, TService, TRepository>
        where TDomain : class 
        where TService: class, IService<TDomain>
        where TRepository: class, IRepository<TDomain>
    {
        protected TService Service;
        protected Mock<TRepository> RepositoryMock;

        /// <summary>
        /// Initialize runs before every test
        /// Mock the repository given as generic in TRepository
        /// Initialize the service with reflection
        /// </summary>
        [SetUp]
        public virtual void Initialize() 
        {
            // Mock the repository
            RepositoryMock = new Mock<TRepository>();

            // Create the service with reflection
            Type serviceType = typeof(TService);
            ConstructorInfo serviceCtor = serviceType.GetConstructor(new[] { typeof(TRepository) });
            Service = (TService)serviceCtor.Invoke(new object[] { RepositoryMock.Object });
        }

        /// <summary>
        /// Test if the repository method is called and check if anything has changed to the entity
        /// </summary>
        /// <param name="entity">The entity which is used as data to test</param>
        /// <returns></returns>
        public virtual async Task FindAsyncTest_GoodFlow(TDomain entity)
        {
            RepositoryMock.Setup(
                repository => repository.FindAsync(1))
                .Returns(
                    Task.FromResult(entity)
                );

            TDomain retrievedEntity = await Service.FindAsync(1);

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.FindAsync(1), Times.Once);
            });

            Assert.AreEqual(entity, retrievedEntity);
        }

        /// <summary>
        /// Test if the repository method is called
        /// </summary>
        /// <param name="entity">The entity which is used as data to test</param>
        public virtual void AddTest_GoodFlow(TDomain entity)
        {
            RepositoryMock.Setup(
                repository => repository.Add(entity));

            Service.Add(entity);

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.Add(entity), Times.Once);
                });
        }

        /// <summary>
        /// Test if the repository method is called
        /// </summary>
        /// <param name="entities">The entities which are used as data to test</param>
        public virtual void AddRangeTest_GoodFlow(IEnumerable<TDomain> entities)
        {
            RepositoryMock.Setup(
                repository => repository.AddRange(entities));

            Service.AddRange(entities);

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.AddRange(entities), Times.Once);
            });

        }

        /// <summary>
        /// Test if the repository method is called
        /// </summary>
        /// <param name="entity">The entity which is used as data to test</param>
        public virtual void Update(TDomain entity)
        {
            RepositoryMock.Setup(
                repository => repository.Update(entity));

            Service.Update(entity);

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.Update(entity), Times.Once);
            });
        }

        /// <summary>
        /// Test if the repository method is called
        /// </summary>
        /// <param name="entity">The entity which is used as data to test</param>
        public virtual void Remove(TDomain entity)
        {
            RepositoryMock.Setup(
                repository => repository.Remove(entity));

            Service.Remove(entity);

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.Remove(entity), Times.Once);
            });
        }

        /// <summary>
        /// Test if the repository method is called
        /// </summary>
        /// <param name="id">The id which is used to remove the entity</param>
        /// <returns></returns>
        public virtual async Task RemoveAsync(int id)
        {
            RepositoryMock.Setup(
                repository => repository.RemoveAsync(id));

            await Service.RemoveAsync(id);

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.RemoveAsync(id), Times.Once);
            });
        }

        /// <summary>
        /// Test if the repository method is called
        /// </summary>
        /// <param name="entities">The entities which are used as data to test</param>
        /// <returns></returns>
        public virtual async Task GetAll(List<TDomain> entities)
        {
            int amountToTest = entities.Count;
            RepositoryMock.Setup(
                repository => repository.GetAll())
                .Returns(
                    Task.FromResult((IEnumerable<TDomain>)entities)
                );

            List<TDomain> retrievedList = (List<TDomain>)await Service.GetAll();

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.GetAll(), Times.Once);
            });

            Assert.AreEqual(amountToTest, retrievedList.Count);
        }

        /// <summary>
        /// Test if the repository method is called
        /// </summary>
        public virtual void Save()
        {
            RepositoryMock.Setup(
                repository => repository.Save());

            Service.Save();

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.Save(), Times.Once);
            });
        }

    }
}
