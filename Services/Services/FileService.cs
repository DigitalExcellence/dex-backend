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

    public interface IFileService : IService<File>
    {

        Task<List<File>> GetFileByFileIdAsync(int fileId);
        Task<List<File>> GetFilesAsync();

        void UploadSingleFile(File entity);

    }

    public class FileService : Service<File>, IFileService
    {
        public FileService(IFileRepository repository) : base(repository) { }

        protected new IFileRepository Repository => (IFileRepository) base.Repository;

        public async Task<List<File>> GetFileByFileIdAsync(int fileId)
        {
            return await Repository.GetFileByFileIdAsync(fileId).ConfigureAwait(false);
        }

        public async Task<List<File>> GetFilesAsync()
        {
            return await Repository.GetFilesAsync();
        }

        public override void Add(File entity)
        {
            base.Add(entity);
        }

        public override void Update(File entity)
        {
            base.Update(entity);
        }

        public void UploadSingleFile(File entity)
        {
            Repository.Add(entity);
            Repository.Save();
        }



    }

}
