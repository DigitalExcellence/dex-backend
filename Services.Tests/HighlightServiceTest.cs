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
    public class HighlightServiceTest : ServiceTest<Highlight, HighlightService, IHighlightRepository>
    {
        protected new IHighlightService Service => (IHighlightService) base.Service;

        /// <summary>
        /// Test whether the repository method is called and no changes have been applied to the object
        /// </summary>
        /// <param name="highlights">A set of 10 highlights which contain a project.</param>
        /// <returns></returns>
        [Test]
        public async Task GetHighlightsAsync_GoodFlow([HighlightDataSource(10)] List<Highlight> highlights)
        {

            RepositoryMock.Setup(
                repository => repository.GetHighlightsAsync())
                .Returns(
                    Task.FromResult(highlights)
                );

            List<Highlight> retrievedHighlights = await Service.GetHighlightsAsync();

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.GetHighlightsAsync(), Times.Once);
            });

            Assert.AreEqual(highlights, retrievedHighlights);
            Assert.AreEqual(10, retrievedHighlights.Count);
        }

        /// <summary>
        /// Test whether the repository method is called and no changes have been applied to the object assuming no highlights were provided
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetHighlightsAsync_NoHighlights()
        {
            List<Highlight> highlights = new List<Highlight>();
            RepositoryMock.Setup(
                repository => repository.GetHighlightsAsync())
                .Returns(
                    Task.FromResult(highlights)
                );

            List<Highlight> retrievedHighlights = await Service.GetHighlightsAsync();

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.GetHighlightsAsync(), Times.Once);
            });

            Assert.AreEqual(highlights, retrievedHighlights);
            Assert.AreEqual(0, retrievedHighlights.Count);

        }


        /// <summary>
        /// Test whether the repository method is called if no project is provided and no changes have been applied to the object assuming no highlights were provided
        /// </summary>
        /// <param name="highlight">A single highlight with no project bound to it.</param>
        /// <returns></returns>
        [Test]
        public async Task GetHighlightsAsync_HighlightWithoutProject([HighlightDataSource] Highlight highlight)
        {
            List<Highlight> highlights = new List<Highlight>();
            highlights.Add(highlight);
            RepositoryMock.Setup(
                repository => repository.GetHighlightsAsync())
                .Returns(
                    Task.FromResult(highlights)
                );

            List<Highlight> retrievedHighlights = await Service.GetHighlightsAsync();

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.GetHighlightsAsync(), Times.Once);
            });
            Assert.AreEqual(highlights, retrievedHighlights);
            Assert.AreEqual(1, retrievedHighlights.Count);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void AddRangeTest_GoodFlow([HighlightDataSource(10)]IEnumerable<Highlight> entities)
        {
            base.AddRangeTest_GoodFlow(entities);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void AddTest_GoodFlow([HighlightDataSource]Highlight entity)
        {
            base.AddTest_GoodFlow(entity);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override Task FindAsyncTest_GoodFlow([HighlightDataSource]Highlight entity)
        {
            return base.FindAsyncTest_GoodFlow(entity);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override Task GetAll([HighlightDataSource(10)]List<Highlight> entities)
        {
            return base.GetAll(entities);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void Remove([HighlightDataSource]Highlight entity)
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
        public override void Update([HighlightDataSource]Highlight entity)
        {
            base.Update(entity);
        }
    }
}
