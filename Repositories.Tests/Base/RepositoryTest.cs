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
    /// <summary>
    /// Base test class which should be inherited from when creating unittests for the repositories.
    /// By inheriting, all the default tests are included in the new test class.
    /// YOU SHOULD OVERRIDE THE DEFAULT TEST TO ADD THE [Test] attribute.
    /// If you do not override the tests and add the [Test] attribute, the default tests will not be triggered.
    /// </summary>
    /// <typeparam name="TDomain">Modelclass which is used to test</typeparam>
    /// <typeparam name="TRepository">Repository which should be tested</typeparam>
    public abstract class RepositoryTest<TDomain, TRepository>
        where TDomain : class
        where TRepository : class, IRepository<TDomain>
    {
        protected ApplicationDbContext DbContext;
        protected TRepository Repository;

        /// <summary>
        /// Initialize runs before every test
        /// Initialize the repository with reflection
        /// </summary>
        [SetUp]
        public virtual void Initialize()
        {
            DbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            Type repositoryType = typeof(TRepository);
            ConstructorInfo repositoryCtor = repositoryType.GetConstructor(new[] { typeof(ApplicationDbContext) });
            Repository = (TRepository)repositoryCtor.Invoke(new object[] { DbContext });
        }

        /// <summary>
        /// Add the given entity to the database
        /// EF Core will give the entity automatically an id
        /// Check which id the entity has and use this id to retrieve it from the database again
        /// Check if all the properties of the retrieved entity match to the properties of the original entity
        /// </summary>
        /// <param name="entity">The entity which is used as data to test</param>
        /// <returns></returns>
        public virtual async Task FindAsyncTest_GoodFlow(TDomain entity)
        {
            DbContext.Add(entity);
            await DbContext.SaveChangesAsync();

            Type type = entity.GetType();
            PropertyInfo property = type.GetProperty("Id");
            int id;
            if(property == null)
            {
                throw new Exception("Id property does not exist");
            }
            else
            {
                id = (int)property.GetValue(entity);
            }

            TDomain retrieved = await Repository.FindAsync(id);

            foreach (PropertyInfo prop in type.GetProperties())
            {
                prop.GetValue(entity).Should().BeEquivalentTo(prop.GetValue(retrieved));
            }
        }

        /// <summary>
        /// Add the given entity to the database
        /// Try to retrieve an entity from the database with a key that doesn't exist
        /// Check if the error is handled correctly by returning null
        /// </summary>
        /// <param name="entity">The entity which is used as data to test</param>
        /// <returns></returns>
        public virtual async Task FindAsyncTest_BadFlow_NotExists(TDomain entity)
        {
            await DbContext.AddAsync(entity);
            await DbContext.SaveChangesAsync();

            Assert.IsNull(await Repository.FindAsync(-1));
        }

        /// <summary>
        /// Add the given entity to the database
        /// Check if the return type is not null
        /// </summary>
        /// <param name="entity">The entity which is used as data to test</param>
        /// <returns></returns>
        public virtual async Task AddAsyncTest_GoodFlow(TDomain entity)
        {
            Repository.Add(entity);
            await DbContext.SaveChangesAsync();

            PropertyInfo property = entity.GetType().GetProperty("Id");
            int id;
            if(property == null)
            {
                throw new Exception("Id property does not exist");
            } else
            {
                id = (int) property.GetValue(entity);
            }

            Repository.Invoking(async r => await r.FindAsync(id)).Should().NotBeNull();
        }

        /// <summary>
        /// Check if the error is handled correctly when trying to add null to the database
        /// </summary>
        public virtual void AddTest_BadFlow_Null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                Repository.Add(null);
                await DbContext.SaveChangesAsync();
            });

        }

        /// <summary>
        /// Add a range of objects to the database
        /// Retrieve the items from the database again by their id
        /// and check if they are not null
        /// </summary>
        /// <param name="entities">The entitis which are used as data to test</param>
        /// <returns></returns>
        public virtual async Task AddRangeTest_GoodFlow(List<TDomain> entities)
        {
            Type type = typeof(TDomain);

            Repository.AddRange(entities);
            await DbContext.SaveChangesAsync();

            foreach (TDomain entity in entities)
            {
                PropertyInfo property = type.GetProperty("Id");
                int id;
                if(property == null)
                {
                    throw new Exception("Id property does not exist");
                } else
                {
                    id = (int) property.GetValue(entity);
                }
                Repository.Invoking(async r => await r.FindAsync(id)).Should().NotBeNull();
            }
        }

        /// <summary>
        /// Check if the error is handled correctly when adding an empty list to the database
        /// </summary>
        public virtual void AddRangeTest_BadFlow_EmptyList()
        {
            List<TDomain> entities = new List<TDomain>();

            Repository.Invoking(r => r.AddRange(entities)).Should().NotThrow();
        }

        /// <summary>
        /// Check if the error is handled correctly when adding a null list to the database
        /// </summary>
        public virtual void AddRangeTest_BadFlow_Null()
        {
            Assert.Throws<ArgumentNullException>(() => Repository.AddRange(null));
        }

        /// <summary>
        /// Add the first entity to the database
        /// Update the entity in the database with the second entity
        /// Retrieve the entity in the database and check if the entity is not equal to the first entity
        /// Also check if the database entity is equal to the second entity
        /// </summary>
        /// <param name="entity">The entity which is used as data to test</param>
        /// <param name="updateEntity">The entity which is used to update the data to test</param>
        /// <returns></returns>
        public virtual async Task UpdateTest_GoodFlow(TDomain entity, TDomain updateEntity)
        {
            TDomain copy = entity.CloneObject<TDomain>();
            Repository.Add(entity);
            await DbContext.SaveChangesAsync();

            Type type = entity.GetType();
            PropertyInfo property = type.GetProperty("Id");
            int id;
            if(property == null)
            {
                throw new Exception("Id property does not exist");
            } else
            {
                id = (int) property.GetValue(entity);
            }

            Repository.Update(entity);
            await DbContext.SaveChangesAsync();
            Assert.AreEqual(entity, await Repository.FindAsync(id));
            Assert.AreNotEqual(copy, await Repository.FindAsync(id));
        }

        /// <summary>
        /// Try to update an entity in the database which does not exist
        /// and check how the error is handled
        /// </summary>
        /// <param name="entity">The entity which is used as data to test</param>
        /// <param name="updateEntity">The entity which is used to update the data to test</param>
        /// <returns></returns>
        public virtual async Task UpdateTest_BadFlow_NotExists(TDomain entity, TDomain updateEntity)
        {
            Repository.Add(entity);
            await DbContext.SaveChangesAsync();

            Type type = entity.GetType();
            PropertyInfo property = type.GetProperty("Id");
            if(property == null)
            {
                throw new Exception("Id property does not exist");
            } else
            {
                property.SetValue(updateEntity, -1);
            }

            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                Repository.Update(updateEntity);
                await DbContext.SaveChangesAsync();
            });
        }

        /// <summary>
        /// Try to update the entity in the database with null
        /// and check how the error is handled
        /// </summary>
        /// <param name="entity">The entity which is used as data to test</param>
        /// <returns></returns>
        public virtual async Task UpdateTest_BadFlow_Null(TDomain entity)
        {
            Repository.Add(entity);
            await DbContext.SaveChangesAsync();

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                Repository.Update(null);
                await DbContext.SaveChangesAsync();
            });
        }

        /// <summary>
        /// Add the given entity to the database and try to remove it again with the id it got from EF Core.
        /// </summary>
        /// <param name="entity">The entity which is used as data to test</param>
        /// <returns></returns>
        public virtual async Task RemoveAsyncTest_GoodFlow(TDomain entity)
        {
            Repository.Add(entity);
            await DbContext.SaveChangesAsync();

            Type type = entity.GetType();
            PropertyInfo property = type.GetProperty("Id");
            int id;
            if(property == null)
            {
                throw new Exception("Id property does not exist");
            } else
            {
                id = (int) property.GetValue(entity);
            }
            await Repository.RemoveAsync(id);
            await DbContext.SaveChangesAsync();

            Assert.NotNull(Repository.FindAsync(id));
        }

        /// <summary>
        /// Try to remove an non existing entity from the database
        /// and check how the error is handled
        /// </summary>
        /// <param name="entity">The entity which is used as data to test</param>
        /// <returns></returns>
        public virtual async Task RemoveAsyncTest_BadFlow_NotExists(TDomain entity)
        {
            Repository.Add(entity);
            await DbContext.SaveChangesAsync();

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await Repository.RemoveAsync(-1));
        }

        /// <summary>
        /// Add the given list to the database and retrieve them again
        /// Check if the retrieved list is the same amount as the given list
        /// </summary>
        /// <param name="entities">The entity which is used as data to test</param>
        /// <returns></returns>
        public virtual async Task GetAllAsyncTest_GoodFlow(List<TDomain> entities)
        {
            int amountToTest = entities.Count;
            await DbContext.AddRangeAsync(entities);
            await DbContext.SaveChangesAsync();

            List<TDomain> retrievedEntities = (List<TDomain>)await Repository.GetAll();
            Assert.AreEqual(amountToTest, retrievedEntities.Count);
        }

        /// <summary>
        /// Try to retrieve a list from an empty database and check how the error is handled
        /// </summary>
        /// <returns></returns>
        public virtual async Task GetAllAsyncTest_Badflow_Empty()
        {
            // No seeding needed.

            List<TDomain> retrievedEntities = (List<TDomain>)await Repository.GetAll();
            Assert.AreEqual(0, retrievedEntities.Count);
        }
    }
}
