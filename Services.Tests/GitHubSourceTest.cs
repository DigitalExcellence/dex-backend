using NUnit.Framework;
using Services.Sources;
using Services.Tests.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Tests
{
    public class GitHubSourceTest : SourceTest<GitHubSource>
    {
        protected new GitHubSource Source => base.Source;

        /// <summary>
        /// Tests if the get source feature is still not implemented.
        /// when it is please update the unit tests.
        /// </summary>
        [Test]
        public void GetSource_notImplementedFlow()
        {
            Assert.Throws<NotImplementedException>(delegate { Source.GetSource(new Uri("http://example.com")); });
        }

        /// <summary>
        /// Tests if the get project information feature is still not implemented.
        /// when it is please update the unit tests.
        /// </summary>
        [Test]
        public void GetProjectInformation_notImplementedFlow()
        {
            Assert.Throws<NotImplementedException>(delegate { Source.GetProjectInformation(new Uri("http://example.com")); });
        }

        /// <summary>
        /// Tests if the project uri matches feature is still not implemented.
        /// when it is please update the unit tests.
        /// </summary>
        [Test]
        public void ProjectUriMatches_notImplementedFlow()
        {
            Assert.Throws<NotImplementedException>(delegate { Source.ProjectURIMatches(new Uri("http://example.com")); });
        }

        /// <summary>
        /// Tests if the search feature is still not implemented.
        /// when it is please update the unit tests.
        /// </summary>
        [Test]
        public void Search_notImplementedFlow()
        {
            Assert.Throws<NotImplementedException>(delegate { Source.Search(""); });
        }
    }
}
