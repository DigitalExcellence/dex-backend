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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{
    /// <summary>
    /// The file service interface.
    /// </summary>
    public interface IFileService : IService<File>
    {
        /// <summary>
        /// Fetches the file object that mathces with the fileId asynchronous.
        /// </summary>
        /// <param name="fileId">the file identifier.</param>
        /// <returns>File object.</returns>
        Task<List<File>> GetFileByFileIdAsync(int fileId);
        /// <summary>
        /// Fetches all file objects asynchronous.
        /// </summary>
        /// <returns>A List of file objects.</returns>
        Task<List<File>> GetFilesAsync();
        /// <summary>
        /// Uploads file information to the database (this is not the actual file).
        /// </summary>
        /// <param name="entity">The file object.</param>
        void UploadSingleFile(File entity);

    }

    public class FileService : Service<File>, IFileService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="FileService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public FileService(IFileRepository repository) : base(repository) { }

        /// <summary>
        /// 
        /// </summary>
        protected new IFileRepository Repository => (IFileRepository) base.Repository;

        /// <summary>
        /// Fetches the file object that mathces with the fileId asynchronous.
        /// </summary>
        /// <param name="fileId">the file identifier.</param>
        /// <returns>File object.</returns>
        public async Task<List<File>> GetFileByFileIdAsync(int fileId)
        {
            return await Repository.GetFileByFileIdAsync(fileId).ConfigureAwait(false);
        }

        /// <summary>
        /// Fetches all file objects asynchronous.
        /// </summary>
        /// <returns>A List of file objects.</returns>
        public async Task<List<File>> GetFilesAsync()
        {
            return await Repository.GetFilesAsync();
        }

        /// <summary>
        /// Uploads file information to the database (this is not the actual file).
        /// </summary>
        /// <param name="entity">The file object.</param>
        public void UploadSingleFile(File entity)
        {
            Repository.Add(entity);
            Repository.Save();
        }
    }

}
