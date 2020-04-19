using Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Repositories.Base;
using Repositories.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Repositories.Tests.Base
{
    public abstract class RepositoryTest<TDomain, TRepository> 
        where TDomain : class
        where TRepository : class, IRepository<TDomain>
    {
        protected ApplicationDbContext DbContext;
        protected TRepository Repository;

        protected Task SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        [SetUp]
        public virtual void Initialize()
        {
            DbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            Type repositoryType = typeof(TRepository);
            ConstructorInfo repositoryCtor = repositoryType.GetConstructor(new[] { typeof(ApplicationDbContext) });
            Repository = (TRepository)repositoryCtor.Invoke(new object[] { DbContext });
        }

        public virtual async Task FindAsyncTest_GoodFlow(TDomain entity)
        {
            DbContext.Add(entity);
            await SaveChangesAsync();

            Type type = entity.GetType();
            int id = (int)type.GetProperty("Id").GetValue(entity);

            TDomain retrieved = await Repository.FindAsync(id);

            foreach (PropertyInfo prop in type.GetProperties())
            {
                prop.GetValue(entity).Should().BeEquivalentTo(prop.GetValue(retrieved));
            }
        }

        public virtual async Task FindAsyncTest_BadFlow_NotExists(TDomain entity)
        {
            await DbContext.AddAsync(entity);
            await DbContext.SaveChangesAsync();

            Assert.IsNull(await Repository.FindAsync(-1));
        }

        public virtual async Task AddAsyncTest_GoodFlow(TDomain entity)
        {
            Repository.Add(entity);
            await SaveChangesAsync();

            int id = (int)entity.GetType().GetProperty("Id").GetValue(entity);
            Repository.Invoking(async r => await r.FindAsync(id)).Should().NotBeNull();
        }

        [Test]
        public virtual void AddTest_BadFlow_Null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                Repository.Add(null);
                await SaveChangesAsync();
            });

        }

        public virtual async Task AddRangeTest_GoodFlow(List<TDomain> entities)
        {
            Type type = typeof(TDomain);

            Repository.AddRange(entities);
            await SaveChangesAsync();

            foreach (TDomain entity in entities)
            {
                int id = (int)type.GetProperty("Id").GetValue(entity);
                Repository.Invoking(async r => await r.FindAsync(id)).Should().NotBeNull();
            }
        }

        [Test]
        public virtual void AddRangeTest_BadFlow_EmptyList()
        {
            List<TDomain> entities = new List<TDomain>();

            Repository.Invoking(r => r.AddRange(entities)).Should().NotThrow();
        }

        [Test]
        public virtual void AddRangeTest_BadFlow_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Repository.AddRange(null);
            });
        }

        public virtual async Task UpdateTest_GoodFlow(TDomain entity, TDomain updateEntity)
        {
            TDomain copy = entity.CloneObject<TDomain>();
            Repository.Add(entity);
            await SaveChangesAsync();

            Type type = entity.GetType();
            int id = (int)type.GetProperty("Id").GetValue(entity);

            Repository.Update(entity);
            await SaveChangesAsync();
            Assert.AreEqual(entity, await Repository.FindAsync(id));
            Assert.AreNotEqual(copy, await Repository.FindAsync(id));
        }

        public virtual async Task UpdateTest_BadFlow_NotExists(TDomain entity, TDomain updateEntity)
        {
            Repository.Add(entity);
            await SaveChangesAsync();

            entity.GetType().GetProperty("Id").SetValue(updateEntity, -1);

            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                Repository.Update(updateEntity);
                await SaveChangesAsync();
            });
        }

        public virtual async Task UpdateTest_BadFlow_Null(TDomain entity)
        {
            Repository.Add(entity);
            await SaveChangesAsync();

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                Repository.Update(null);
                await SaveChangesAsync();
            });
        }

        public virtual async Task RemoveAsyncTest_GoodFlow(TDomain entity)
        {
            Repository.Add(entity);
            await SaveChangesAsync();
            await RemoveAsyncTest(entity);
        }

        protected async Task RemoveAsyncTest(TDomain entity)
        {
            int id = (int)entity.GetType().GetProperty("Id").GetValue(entity);

            await Repository.RemoveAsync(id);
            await SaveChangesAsync();

            Assert.NotNull(Repository.FindAsync(id));
        }

        public virtual async Task RemoveAsyncTest_BadFlow_NotExists(TDomain entity)
        {
            Repository.Add(entity);
            await SaveChangesAsync();

            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await Repository.RemoveAsync(-1);
            });
        }

        public virtual async Task GetAllAsyncTest_GoodFlow(List<TDomain> entities, int amountToTest)
        {
            await DbContext.AddRangeAsync(entities);
            await SaveChangesAsync();

            List<TDomain> retrievedEntities = (List<TDomain>)await Repository.GetAll();
            Assert.AreEqual(amountToTest, retrievedEntities.Count);
        }

        public virtual async Task GetAllAsyncTest_Badflow_Empty()
        {
            List<TDomain> entities = new List<TDomain>();
            await DbContext.AddRangeAsync(entities);
            await SaveChangesAsync();
            
            List<TDomain> retrievedEntities = (List<TDomain>)await Repository.GetAll();
            Assert.AreEqual(0, retrievedEntities.Count);
        }
    }
}
