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

using Moq;
using NUnit.Framework;
using Services.ExternalDataProviders;

namespace Services.Tests.ExternalDataProviders
{

    [TestFixture]
    public class DataProviderServiceTest
    {

        private Mock<IDataProviderLoader> loaderMock;

        /// <summary>
        ///     Initialize runs before every test
        ///     Mock the repository given as generic in TRepository
        ///     Initialize the service with reflection
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            // Mock the repository
            loaderMock = new Mock<IDataProviderLoader>();
        }



    }

}
