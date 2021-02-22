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
using Repositories;
using Services.Base;

namespace Services.Services
{

    /// <summary>
    ///     The file service interface.
    /// </summary>
    public interface IFileService : IService<File> { }

    /// <summary>
    ///     This is the file service
    /// </summary>
    public class FileService : Service<File>, IFileService
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public FileService(IFileRepository repository) : base(repository) { }

        /// <summary>
        /// </summary>
        protected new IFileRepository Repository => (IFileRepository) base.Repository;

    }

}
