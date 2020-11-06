using AngleSharp.Io.Dom;
using Models;
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
    /// <summary>
    /// FileServiceTest
    /// </summary>
    /// <seealso cref="IFileRepository" />
    [TestFixture]
    public class FileServiceTest : ServiceTest<File, FileService, IFileRepository>
    {

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        public new IFileService Service => (IFileService) base.Service;

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void AddRangeTest_GoodFlow([FileDataSource(100)] IEnumerable<File> entities)
        {
            base.AddRangeTest_GoodFlow(entities);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void AddTest_GoodFlow([FileDataSource] File entity)
        {
            base.AddTest_GoodFlow(entity);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override Task GetAll([FileDataSource(100)] List<File> entities)
        {
            return base.GetAll(entities);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void Remove([FileDataSource] File entity)
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
        public override void Update([FileDataSource] File entity)
        {
            base.Update(entity);
        }

    }
}
