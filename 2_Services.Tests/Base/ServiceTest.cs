using FluentAssertions;
using Moq;
using NUnit.Framework;
using Repositories.Base;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace _2_Services.Tests.Base
{
    public abstract class ServiceTest<TDomain, TService, TRepository>
        where TDomain : class 
        where TService: class, IService<TDomain>
        where TRepository: class, IRepository<TDomain>
    {
        protected TService _service;
        protected Mock<TRepository> _repositoryMock;

        [SetUp]
        public void Initialize() 
        {
            // Mock the repository
            _repositoryMock = new Mock<TRepository>();

            // Create the service with reflection
            Type serviceType = typeof(TService);
            ConstructorInfo serviceCtor = serviceType.GetConstructor(new[] { typeof(TRepository) });
            _service = (TService)serviceCtor.Invoke(new object[] { _repositoryMock.Object });
        }

        public virtual async Task FindAsyncTest_GoodFlow(TDomain entity)
        {
            _repositoryMock.Setup(
                repository => repository.FindAsync(1))
                .Returns(
                    Task.FromResult(entity)
                );

            TDomain retrievedEntity = await _service.FindAsync(1);

            Assert.DoesNotThrow(() => {
                _repositoryMock.Verify(repository => repository.FindAsync(1), Times.Once);
            });

            Assert.AreEqual(entity, retrievedEntity);
        }

        public virtual void AddTest_GoodFlow(TDomain entity)
        {
            _repositoryMock.Setup(
                repository => repository.Add(entity));

            _service.Add(entity);

            Assert.DoesNotThrow(() => {
                _repositoryMock.Verify(repository => repository.Add(entity), Times.Once);
                });
        }

        public virtual void AddRangeTest_GoodFlow(IEnumerable<TDomain> entities)
        {
            _repositoryMock.Setup(
                repository => repository.AddRange(entities));

            _service.AddRange(entities);

            Assert.DoesNotThrow(() => {
                _repositoryMock.Verify(repository => repository.AddRange(entities), Times.Once);
            });

        }

        public virtual void Update(TDomain entity)
        {
            _repositoryMock.Setup(
                repository => repository.Update(entity));

            _service.Update(entity);

            Assert.DoesNotThrow(() => {
                _repositoryMock.Verify(repository => repository.Update(entity), Times.Once);
            });
        }

        public virtual void Remove(TDomain entity)
        {
            _repositoryMock.Setup(
                repository => repository.Remove(entity));

            _service.Remove(entity);

            Assert.DoesNotThrow(() => {
                _repositoryMock.Verify(repository => repository.Remove(entity), Times.Once);
            });
        }

        public virtual async Task RemoveAsync(int id)
        {
            _repositoryMock.Setup(
                repository => repository.RemoveAsync(id));

            await _service.RemoveAsync(id);

            Assert.DoesNotThrow(() => {
                _repositoryMock.Verify(repository => repository.RemoveAsync(id), Times.Once);
            });
        }

        public virtual async Task GetAll(List<TDomain> entities, int amountToTest)
        {
            _repositoryMock.Setup(
                repository => repository.GetAll())
                .Returns(
                    Task.FromResult((IEnumerable<TDomain>)entities)
                );

            List<TDomain> retrievedList = (List<TDomain>)await _service.GetAll();

            Assert.DoesNotThrow(() => {
                _repositoryMock.Verify(repository => repository.GetAll(), Times.Once);
            });

            Assert.AreEqual(amountToTest, retrievedList.Count);
        }

        public virtual void Save()
        {
            _repositoryMock.Setup(
                repository => repository.Save());

            _service.Save();

            Assert.DoesNotThrow(() => {
                _repositoryMock.Verify(repository => repository.Save(), Times.Once);
            });
        }

    }
}
