using FluentAssertions;
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
    public abstract class ServiceTest<TDomain, TService, TRepository>
        where TDomain : class 
        where TService: class, IService<TDomain>
        where TRepository: class, IRepository<TDomain>
    {
        protected TService Service;
        protected Mock<TRepository> RepositoryMock;

        [SetUp]
        public void Initialize() 
        {
            // Mock the repository
            RepositoryMock = new Mock<TRepository>();

            // Create the service with reflection
            Type serviceType = typeof(TService);
            ConstructorInfo serviceCtor = serviceType.GetConstructor(new[] { typeof(TRepository) });
            Service = (TService)serviceCtor.Invoke(new object[] { RepositoryMock.Object });
        }

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

        public virtual void AddTest_GoodFlow(TDomain entity)
        {
            RepositoryMock.Setup(
                repository => repository.Add(entity));

            Service.Add(entity);

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.Add(entity), Times.Once);
                });
        }

        public virtual void AddRangeTest_GoodFlow(IEnumerable<TDomain> entities)
        {
            RepositoryMock.Setup(
                repository => repository.AddRange(entities));

            Service.AddRange(entities);

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.AddRange(entities), Times.Once);
            });

        }

        public virtual void Update(TDomain entity)
        {
            RepositoryMock.Setup(
                repository => repository.Update(entity));

            Service.Update(entity);

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.Update(entity), Times.Once);
            });
        }

        public virtual void Remove(TDomain entity)
        {
            RepositoryMock.Setup(
                repository => repository.Remove(entity));

            Service.Remove(entity);

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.Remove(entity), Times.Once);
            });
        }

        public virtual async Task RemoveAsync(int id)
        {
            RepositoryMock.Setup(
                repository => repository.RemoveAsync(id));

            await Service.RemoveAsync(id);

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.RemoveAsync(id), Times.Once);
            });
        }

        public virtual async Task GetAll(List<TDomain> entities, int amountToTest)
        {
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
