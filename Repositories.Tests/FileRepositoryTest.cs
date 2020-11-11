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
    public class FileRepositoryTest : RepositoryTest<File, FileRepository>
    {
        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        protected new IFileRepository Repository => (IFileRepository) base.Repository;

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task AddAsyncTest_GoodFlow([FileDataSource] File entity)
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
        public override Task AddRangeTest_GoodFlow([FileDataSource(5)] List<File> entities)
        {
            return base.AddRangeTest_GoodFlow(entities);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override void AddTest_BadFlow_Null()
        {
            base.AddTest_BadFlow_Null();
        }

    }
}
