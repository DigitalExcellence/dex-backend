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

using NUnit.Framework;
using Services.Sources;
using Services.Tests.Base;
using System;

namespace Services.Tests
{

    /// <summary>
    ///     The github source tests
    /// </summary>
    public class GitHubSourceTest : SourceTest<GitHubSource>
    {

        /// <summary>
        ///     The source
        /// </summary>
        protected new GitHubSource Source => base.Source;

        /// <summary>
        ///     Tests if the get source feature is still not implemented.
        ///     when it is please update the unit tests.
        /// </summary>
        [Test]
        public void GetSource_notImplementedFlow()
        {
            Assert.Throws<NotImplementedException>(delegate { Source.GetSource(new Uri("http://example.com")); });
        }

        /// <summary>
        ///     Tests if the get project information feature is still not implemented.
        ///     when it is please update the unit tests.
        /// </summary>
        [Test]
        public void GetProjectInformation_notImplementedFlow()
        {
            Assert.Throws<NotImplementedException>(delegate
            {
                Source.GetProjectInformation(new Uri("http://example.com"));
            });
        }

        /// <summary>
        ///     Tests if the project uri matches feature is still not implemented.
        ///     when it is please update the unit tests.
        /// </summary>
        [Test]
        public void ProjectUriMatches_notImplementedFlow()
        {
            Assert.Throws<NotImplementedException>(
                delegate { Source.ProjectURIMatches(new Uri("http://example.com")); });
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

    }

}
