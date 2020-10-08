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
        protected new IFileService Service => (IFileService) base.Service;


    }
}
