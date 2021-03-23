/*
* Digital Excellence Copyright (C) 2020 Brend Smits
* 
* This program is free software: you can redistribute it and/or modify 
* it under the terms of the GNU Lesser General Public License as published 
* by the Free Software Foundation version 3 of the License.
* 
* This program is distributed in the hope that it will be useful, 
* but WITHOUT ANY WARRANTY; without even the implied warranty 
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
* See the GNU Lesser General Public License for more details.
* 
* You can find a copy of the GNU Lesser General Public License 
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using Models;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Services.Sources;
using Services.Sources.Resources;
using Services.Tests.Base;
using System;
using System.Net;

namespace Services.Tests
{

    /// <summary>
    ///     Gitlab source test
    /// </summary>
    [TestFixture]
    public class GitLabSourceTest : SourceTest<GitLabSource>
    {

        /// <summary>
        ///     The source
        /// </summary>
        protected new GitLabSource Source => base.Source;

        /// <summary>
        ///     Test if the projecturimatches function matches all the different urls and with corect metadata.
        /// </summary>
        /// <returns></returns>
        [Test]
        public void ProjectURIMatches_goodflow()
        {
            string response =
                "<meta content=\"object\" property=\"og:type\">\r\n<meta content=\"GitLab\" property=\"og:site_name\">\r\n<meta content=\"Hari Sekhon / DevOps-Bash-tools\" property=\"og:title\">\r\n";
            RestClientFactoryMock.Setup(restClientFactory => restClientFactory.Create(It.IsAny<Uri>()))
                                 .Returns(MockRestClient(HttpStatusCode.OK, response));

            Uri requestUri = new Uri("http://domain.com/group/project");
            Assert.IsTrue(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("https://domain.com/group/project");
            Assert.IsTrue(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("http://www.domain.com/group/project");
            Assert.IsTrue(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("https://www.domain.com/group/project");
            Assert.IsTrue(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("domain.com/group/project", UriKind.Relative);
            Assert.IsTrue(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("www.domain.com/group/project", UriKind.Relative);
            Assert.IsTrue(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("http://domain.com:123/group/project");
            Assert.IsTrue(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("https://domain.com:123/group/project");
            Assert.IsTrue(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("http://1.2.3.4/group/project");
            Assert.IsTrue(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("https://1.2.3.4/group/project");
            Assert.IsTrue(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("http://1.2.3.4:123/group/project");
            Assert.IsTrue(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("https://1.2.3.4:123/group/project");
            Assert.IsTrue(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("1.2.3.4/group/project", UriKind.Relative);
            Assert.IsTrue(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("1.2.3.4:123/group/project", UriKind.Relative);
            Assert.IsTrue(Source.ProjectURIMatches(requestUri));
        }

        /// <summary>
        ///     Tests if the project url is for gitlab with valid urls but the page does not contain the right metadata.
        /// </summary>
        [Test]
        public void ProjectURIMatches_urlvalid_but_metadata_false()
        {
            string response =
                "<meta content=\"object\" property=\"og:type\">\r\n<meta content=\"GitHub\" property=\"og:site_name\">\r\n<meta content=\"Hari Sekhon / DevOps-Bash-tools\" property=\"og:title\">\r\n";
            RestClientFactoryMock.Setup(restClientFactory => restClientFactory.Create(It.IsAny<Uri>()))
                                 .Returns(MockRestClient(HttpStatusCode.OK, response));
            Uri requestUri = new Uri("http://domain.com/group/project");
            Assert.IsFalse(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("https://domain.com/group/project");
            Assert.IsFalse(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("http://www.domain.com/group/project");
            Assert.IsFalse(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("https://www.domain.com/group/project");
            Assert.IsFalse(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("domain.com/group/project", UriKind.Relative);
            Assert.IsFalse(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("www.domain.com/group/project", UriKind.Relative);
            Assert.IsFalse(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("http://domain.com:123/group/project");
            Assert.IsFalse(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("https://domain.com:123/group/project");
            Assert.IsFalse(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("http://1.2.3.4/group/project");
            Assert.IsFalse(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("https://1.2.3.4/group/project");
            Assert.IsFalse(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("http://1.2.3.4:123/group/project");
            Assert.IsFalse(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("https://1.2.3.4:123/group/project");
            Assert.IsFalse(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("1.2.3.4/group/project", UriKind.Relative);
            Assert.IsFalse(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("1.2.3.4:123/group/project", UriKind.Relative);
            Assert.IsFalse(Source.ProjectURIMatches(requestUri));
        }

        /// <summary>
        ///     Tests if the project url is for gitlab with invalid urls.
        /// </summary>
        [Test]
        public void ProjectURIMatches_url_invalid()
        {
            string response =
                "<meta content=\"object\" property=\"og:type\">\r\n<meta content=\"GitHub\" property=\"og:site_name\">\r\n<meta content=\"Hari Sekhon / DevOps-Bash-tools\" property=\"og:title\">\r\n";
            RestClientFactoryMock.Setup(restClientFactory => restClientFactory.Create(It.IsAny<Uri>()))
                                 .Returns(MockRestClient(HttpStatusCode.OK, response));
            Uri requestUri = new Uri("http://example.com/group");
            Assert.IsFalse(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("ftp://example.com/group");
            Assert.IsFalse(Source.ProjectURIMatches(requestUri));

            requestUri = new Uri("gopher://example.com/group");
            Assert.IsFalse(Source.ProjectURIMatches(requestUri));
        }

        /// <summary>
        ///     Tests if the search feature is still not implemented.
        ///     when it is please update the unit tests.
        /// </summary>
        [Test]
        public void Search_notImplementedFlow()
        {
            Assert.Throws<NotImplementedException>(delegate { Source.Search(""); });
        }

        /// <summary>
        ///     Tests if the get source feature is still not implemented.
        ///     when it is please update the unit tests.
        /// </summary>
        [Test]
        public void GetSource_notImplementedFlow()
        {
            Assert.Throws<NotImplementedException>(delegate { Source.GetSource(new Uri("https://example.com")); });
        }

        /// <summary>
        ///     Fetches the repository information from the api.
        /// </summary>
        [Test]
        public void FetchRepo_goodflow()
        {
            GitLabResourceResult resource = new GitLabResourceResult
                                            {
                                                Name = "repo name",
                                                ReadmeUrl = "readme url",
                                                WebUrl = "repo url"
                                            };

            string response = JsonConvert.SerializeObject(resource);
            RestClientFactoryMock.Setup(restClientFactory => restClientFactory.Create(It.IsAny<Uri>()))
                                 .Returns(MockRestClient(HttpStatusCode.OK, response));


            GitLabResourceResult resourceResult = Source.FetchRepo(new Uri("http://example.com"));
            Assert.AreEqual(JsonConvert.SerializeObject(resource), JsonConvert.SerializeObject(resourceResult));
        }

        /// <summary>
        ///     Fetches the repository information from the api. but something happend and it returned badrequest with content
        ///     null.
        /// </summary>
        [Test]
        public void FetchRepo_badflow()
        {
            RestClientFactoryMock.Setup(restClientFactory => restClientFactory.Create(It.IsAny<Uri>()))
                                 .Returns(MockRestClient(HttpStatusCode.BadRequest, null));

            GitLabResourceResult gitLabResourceResult = Source.FetchRepo(new Uri("http://example.com"));
            Assert.IsNull(gitLabResourceResult);
        }

        /// <summary>
        ///     Tries to fetch the readme content.
        /// </summary>
        [Test]
        public void FetchReadme_goodflow()
        {
            string readmeContent = "This project has an amazing readme";
            RestClientFactoryMock.Setup(restClientFactory => restClientFactory.Create(It.IsAny<Uri>()))
                                 .Returns(MockRestClient(HttpStatusCode.OK, readmeContent));

            string readme = Source.FetchReadme("http://example.com");
            Assert.AreEqual(readmeContent, readme);
        }

        /// <summary>
        ///     Tries to fetch the readme which returns a bad request.
        /// </summary>
        [Test]
        public void FetchReadme_badflow()
        {
            RestClientFactoryMock.Setup(restClientFactory => restClientFactory.Create(It.IsAny<Uri>()))
                                 .Returns(MockRestClient(HttpStatusCode.BadRequest, null));

            string readme = Source.FetchReadme("http://example.com");
            Assert.IsNull(readme);
        }

        /// <summary>
        ///     Tests the get project information flow best case scenario.
        ///     Expects the project values to be the same as the gitlabresourceresult model.
        /// </summary>
        [Test]
        public void GetProjectInformation_goodflow()
        {
            // Prepare
            Uri projectUri = new Uri("http://example.com/group/project");
            GitLabResourceResult gitLabResourceResult = new GitLabResourceResult();
            gitLabResourceResult.Name = "repo name";
            gitLabResourceResult.Description = "repo description";
            gitLabResourceResult.WebUrl = "web url";
            gitLabResourceResult.ReadmeUrl = "http://example.com/readme";
            string data = JsonConvert.SerializeObject(gitLabResourceResult);
            string readmeContent = "This is a readme";


            RestClientFactoryMock.Setup(restClientFactory =>
                                            restClientFactory.Create(
                                                new Uri("http://example.com/api/v4/projects/group%2Fproject")))
                                 .Returns(MockRestClient(HttpStatusCode.OK, data));
            RestClientFactoryMock.Setup(restClientFactory =>
                                            restClientFactory.Create(new Uri(gitLabResourceResult.ReadmeUrl)))
                                 .Returns(MockRestClient(HttpStatusCode.OK, readmeContent));

            // Ask
            Project project = Source.GetProjectInformation(projectUri);

            // Assert
            Assert.AreEqual(project.Name, gitLabResourceResult.Name);
            Assert.AreEqual(project.ShortDescription, gitLabResourceResult.Description);
            Assert.AreEqual(project.Uri, gitLabResourceResult.WebUrl);
            Assert.AreEqual(project.Description, readmeContent);
        }

        /// <summary>
        ///     This test checks what happens when fetch repo(the main gather information function fails)
        ///     expects an empty project.
        /// </summary>
        [Test]
        public void GetProjectInformation_badflow_no_fetch_repo()
        {
            // Prepare
            RestClientFactoryMock.Setup(restClientFactory =>
                                            restClientFactory.Create(
                                                new Uri("http://example.com/api/v4/projects/group%2Fproject")))
                                 .Returns(MockRestClient(HttpStatusCode.BadRequest, null));

            // Ask
            Project project = Source.GetProjectInformation(new Uri("http://example.com/group/project"));

            // Assert
            Assert.AreEqual(JsonConvert.SerializeObject(new Project()), JsonConvert.SerializeObject(project));
        }

        /// <summary>
        ///     This test checks what happens when fetch readme fails
        ///     expects a filled project except for the description field..
        /// </summary>
        [Test]
        public void GetProjectInformation_badflow_no_readme()
        {
            // Prepare
            Uri projectUri = new Uri("http://example.com/group/project");
            GitLabResourceResult gitLabResourceResult = new GitLabResourceResult();
            gitLabResourceResult.Name = "repo name";
            gitLabResourceResult.Description = "repo description";
            gitLabResourceResult.WebUrl = "web url";
            string data = JsonConvert.SerializeObject(gitLabResourceResult);

            RestClientFactoryMock.Setup(restClientFactory =>
                                            restClientFactory.Create(
                                                new Uri("http://example.com/api/v4/projects/group%2Fproject")))
                                 .Returns(MockRestClient(HttpStatusCode.OK, data));

            // Ask
            Project project = Source.GetProjectInformation(projectUri);

            // Assert
            Assert.AreEqual(project.Name, gitLabResourceResult.Name);
            Assert.AreEqual(project.ShortDescription, gitLabResourceResult.Description);
            Assert.AreEqual(project.Uri, gitLabResourceResult.WebUrl);
            Assert.IsNull(project.Description);
        }

    }

}
