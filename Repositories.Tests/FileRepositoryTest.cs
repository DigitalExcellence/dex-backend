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


    }
}
