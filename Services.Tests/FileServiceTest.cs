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
using NUnit.Framework;
using Repositories;
using Repositories.Tests.DataSources;
using Services.Services;
using Services.Tests.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Tests
{

    /// <summary>
    ///     FileServiceTest
    /// </summary>
    /// <seealso cref="IFileRepository" />
    [TestFixture]
    public class FileServiceTest : ServiceTest<File, FileService, IFileRepository>
    {

        /// <summary>
        ///     Gets the service.
        /// </summary>
        /// <value>
        ///     The service.
        /// </value>
        public new IFileService Service => base.Service;

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void AddRangeTest_GoodFlow([FileDataSource(100)] IEnumerable<File> entities)
        {
            base.AddRangeTest_GoodFlow(entities);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void AddTest_GoodFlow([FileDataSource] File entity)
        {
            base.AddTest_GoodFlow(entity);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override Task GetAll([FileDataSource(100)] List<File> entities)
        {
            return base.GetAll(entities);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void Remove([FileDataSource] File entity)
        {
            base.Remove(entity);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public Task RemoveAsync()
        {
            return base.RemoveAsync(1);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void Save()
        {
            base.Save();
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void Update([FileDataSource] File entity)
        {
            base.Update(entity);
        }

    }

}
