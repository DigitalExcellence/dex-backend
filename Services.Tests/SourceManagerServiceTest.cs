using Models;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Services.Services;
using Services.Sources;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Services.Tests
{
    /// <summary>
    /// The source manager service test.
    /// PLEASE UPDATE WHEN NEW SOURCES ARE ADDED.
    /// </summary>
    [TestFixture]
    public class SourceManagerServiceTest
    {
        /// <summary>
        /// The source manager service
        /// </summary>
        private SourceManagerService sourceManagerService;
        /// <summary>
        /// The git lab mock
        /// </summary>
        private Mock<IGitLabSource> gitLabMock;
        /// <summary>
        /// The git hub mock
        /// </summary>
        private Mock<IGitHubSource> gitHubMock;

        /// <summary>
        /// Initializes the source before every test run.
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            // Mock the repository
            gitLabMock = new Mock<IGitLabSource>();
            gitHubMock = new Mock<IGitHubSource>();

            sourceManagerService = new SourceManagerService(gitLabMock.Object,gitHubMock.Object);
        }

        /// <summary>
        /// Checks if the uri matches the source. the source project is returned.
        /// </summary>
        [Test]
        public void FetchProject_goodflow_matches_gitlab()
        {
            Project project = new Project();
            project.Name = "test project";
            project.Description = "readme content";
            project.ShortDescription = "project readme";
            project.Uri = "website url";

            gitLabMock.Setup(r => r.ProjectURIMatches(It.IsAny<Uri>()))
                      .Returns(true);
            gitLabMock.Setup(r => r.GetProjectInformation(It.IsAny<Uri>()))
                      .Returns(project);

            Project projectresult = sourceManagerService.FetchProject(new Uri("http://example.com"));

            Assert.AreEqual(project,projectresult);
        }

        /// <summary>
        /// Checks if null is returned when no projects match.
        /// </summary>
        [Test]
        public void FetchProject_badflow_no_uri_matches()
        {
            gitLabMock.Setup(r => r.ProjectURIMatches(It.IsAny<Uri>()))
                      .Returns(false);
            gitLabMock.Setup(r => r.GetProjectInformation(It.IsAny<Uri>()))
                      .Returns(new Project());

            Project project = sourceManagerService.FetchProject(new Uri("http://example.com"));
            Assert.IsNull(project);
        }

    }
}
