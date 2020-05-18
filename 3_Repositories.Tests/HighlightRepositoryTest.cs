using Models;
using NUnit.Framework;
using Repositories.Tests.Base;
using Repositories.Tests.DataSources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Tests
{
    [TestFixture]
    public class HighlightRepositoryTest : RepositoryTest<Highlight, HighlightRepository>
    {
        protected new IHighlightRepository Repository => (IHighlightRepository) base.Repository;

        /// <summary>
        /// Test if highlights are retrieved correctly
        /// </summary>
        /// <param name="highlights"></param>
        /// <returns>A set of 10 highlights, all containing a project.</returns>
        [Test]
        public async Task GetAllWithUserAsyncTest_GoodFlow(
            [HighlightDataSource(10)]List<Highlight> highlights, [ProjectDataSource(10)] List<Project> projects)
        {
            foreach(Highlight highlight in highlights)
            {
                highlight.Project = projects[highlights.IndexOf(highlight)];
                highlight.ProjectId = highlight.Project.Id;
            }

            // Seed database
            DbContext.AddRange(highlights);
            await DbContext.SaveChangesAsync();

            // Test
            List<Highlight> retrieved = await Repository.GetHighlightsAsync();
            Assert.AreEqual(10, retrieved.Count);
        }

        /// <summary>
        /// Test if no highlights are retrieved when the database is empty
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetAllWithUserAsyncTest_NoHighlights()
        {
            List<Highlight> highlights = new List<Highlight>();

            // Seed database
            DbContext.AddRange(highlights);
            await DbContext.SaveChangesAsync();

            // Test
            List<Highlight> retrieved = await Repository.GetHighlightsAsync();
            Assert.AreEqual(0, retrieved.Count);
        }


        /// <summary>
        /// Test if no highlights are retrieved when no project is provided.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetAllWithUserAsyncTest_NoProjectInHighlight([HighlightDataSource] Highlight highlight)
        {
            // Seed database
            DbContext.Add(highlight);
            await DbContext.SaveChangesAsync();

            // Test
            List<Highlight> retrieved = await Repository.GetHighlightsAsync();
            Assert.AreEqual(0, retrieved.Count);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task AddAsyncTest_GoodFlow([HighlightDataSource]Highlight entity)
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
        public override Task AddRangeTest_GoodFlow([HighlightDataSource(10)]List<Highlight> entities)
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
        public override Task FindAsyncTest_BadFlow_NotExists([HighlightDataSource]Highlight entity)
        {
            return base.FindAsyncTest_BadFlow_NotExists(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task FindAsyncTest_GoodFlow([HighlightDataSource]Highlight entity)
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
        public override Task GetAllAsyncTest_GoodFlow([HighlightDataSource(10)]List<Highlight> entities)
        {
            return base.GetAllAsyncTest_GoodFlow(entities);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task RemoveAsyncTest_BadFlow_NotExists([HighlightDataSource]Highlight entity)
        {
            return base.RemoveAsyncTest_BadFlow_NotExists(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task RemoveAsyncTest_GoodFlow([HighlightDataSource]Highlight entity)
        {
            return base.RemoveAsyncTest_GoodFlow(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task UpdateTest_BadFlow_NotExists([HighlightDataSource]Highlight entity, [HighlightDataSource]Highlight updateEntity)
        {
            return base.UpdateTest_BadFlow_NotExists(entity, updateEntity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task UpdateTest_BadFlow_Null([HighlightDataSource]Highlight entity)
        {
            return base.UpdateTest_BadFlow_Null(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task UpdateTest_GoodFlow([HighlightDataSource]Highlight entity, [HighlightDataSource]Highlight updateEntity)
        {
            return base.UpdateTest_GoodFlow(entity, updateEntity);
        }
    }
}
