using _3_Repositories.Tests.DataGenerators.Base;
using _3_Repositories.Tests.Extensions;
using Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Repositories.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace _3_Repositories.Tests.Base
{
    public abstract class RepositoryTest<TDomain, TRepository> 
        where TDomain : class
        where TRepository : class, IRepository<TDomain>
    {
        protected ApplicationDbContext _dbContext;
        protected TRepository _repository;

        protected Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        [SetUp]
        public virtual void Initialize()
        {
            _dbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            Type repositoryType = typeof(TRepository);
            ConstructorInfo repositoryCtor = repositoryType.GetConstructor(new[] { typeof(ApplicationDbContext) });
            _repository = (TRepository)repositoryCtor.Invoke(new object[] { _dbContext });
        }

        public virtual async Task FindAsyncTest_GoodFlow(TDomain entity)
        {
            _dbContext.Add(entity);
            await SaveChangesAsync();

            Type type = entity.GetType();
            int id = (int)type.GetProperty("Id").GetValue(entity);

            TDomain retrieved = await _repository.FindAsync(id);

            foreach (PropertyInfo prop in type.GetProperties())
            {
                prop.GetValue(entity).Should().BeEquivalentTo(prop.GetValue(retrieved));
            }
        }

        public virtual async Task FindAsyncTest_BadFlow_NotExists(TDomain entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            Assert.IsNull(await _repository.FindAsync(-1));
        }

        public virtual async Task AddAsyncTest_GoodFlow(TDomain entity)
        {
            _repository.Add(entity);
            await SaveChangesAsync();

            int id = (int)entity.GetType().GetProperty("Id").GetValue(entity);
            _repository.Invoking(async r => await r.FindAsync(id)).Should().NotBeNull();
        }

        [Test]
        public virtual void AddTest_BadFlow_Null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                _repository.Add(null);
                await SaveChangesAsync();
            });

        }

        public virtual async Task AddRangeTest_GoodFlow(List<TDomain> entities)
        {
            Type type = typeof(TDomain);

            _repository.AddRange(entities);
            await SaveChangesAsync();

            foreach (TDomain entity in entities)
            {
                int id = (int)type.GetProperty("Id").GetValue(entity);
                _repository.Invoking(async r => await r.FindAsync(id)).Should().NotBeNull();
            }
        }

        [Test]
        public virtual void AddRangeTest_BadFlow_EmptyList()
        {
            List<TDomain> entities = new List<TDomain>();

            _repository.Invoking(r => r.AddRange(entities)).Should().NotThrow();
        }

        [Test]
        public virtual void AddRangeTest_BadFlow_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _repository.AddRange(null);
            });
        }

        public virtual async Task UpdateTest_GoodFlow(TDomain entity, TDomain updateEntity)
        {
            TDomain copy = entity.CloneObject<TDomain>();
            _repository.Add(entity);
            await SaveChangesAsync();

            Type type = entity.GetType();
            int id = (int)type.GetProperty("Id").GetValue(entity);

            _repository.Update(entity);
            await SaveChangesAsync();
            Assert.AreEqual(entity, await _repository.FindAsync(id));
            Assert.AreNotEqual(copy, await _repository.FindAsync(id));
        }

        public virtual async Task UpdateTest_BadFlow_NotExists(TDomain entity, TDomain updateEntity)
        {
            _repository.Add(entity);
            await SaveChangesAsync();

            entity.GetType().GetProperty("Id").SetValue(updateEntity, -1);

            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                _repository.Update(updateEntity);
                await SaveChangesAsync();
            });
        }

        public virtual async Task UpdateTest_BadFlow_Null(TDomain entity)
        {
            _repository.Add(entity);
            await SaveChangesAsync();

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                _repository.Update(null);
                await SaveChangesAsync();
            });
        }

        public virtual async Task RemoveAsyncTest_GoodFlow(TDomain entity)
        {
            _repository.Add(entity);
            await SaveChangesAsync();
            await RemoveAsyncTest(entity);
        }

        protected async Task RemoveAsyncTest(TDomain entity)
        {
            int id = (int)entity.GetType().GetProperty("Id").GetValue(entity);

            await _repository.RemoveAsync(id);
            await SaveChangesAsync();

            Assert.NotNull(_repository.FindAsync(id));
        }

        public virtual async Task RemoveAsyncTest_BadFlow_NotExists(TDomain entity)
        {
            _repository.Add(entity);
            await SaveChangesAsync();

            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _repository.RemoveAsync(-1);
            });
        }

        public virtual async Task GetAllAsyncTest_GoodFlow(List<TDomain> entities, int amountToTest)
        {
            await _dbContext.AddRangeAsync(entities);
            await SaveChangesAsync();

            List<TDomain> retrievedEntities = (List<TDomain>)await _repository.GetAll();
            Assert.AreEqual(amountToTest, retrievedEntities.Count);
        }

        public virtual async Task GetAllAsyncTest_Badflow_Empty()
        {
            List<TDomain> entities = new List<TDomain>();
            await _dbContext.AddRangeAsync(entities);
            await SaveChangesAsync();
            
            List<TDomain> retrievedEntities = (List<TDomain>)await _repository.GetAll();
            Assert.AreEqual(0, retrievedEntities.Count);
        }
    }
}