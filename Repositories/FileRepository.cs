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

using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Repositories
{

    public interface IFileRepository : IRepository<File>
    {

        Task<List<File>> GetFilesAsync();
        Task<List<File>> GetFileByFileIdAsync(int fileId);
        

    }

    public class FileRepository : Repository<File>, IFileRepository
    {

        public FileRepository(DbContext dbContext) : base(dbContext) { }

        public async Task<List<File>> GetFileByFileIdAsync(int fileId)
        {
            return await GetDbSet<File>()
                         .Where(s => s.Id == fileId)
                         .ToListAsync();
        }

        public async Task<List<File>> GetFilesAsync()
        {
            return await GetDbSet<File>()
                                .ToListAsync();
            
        }
    }
}
