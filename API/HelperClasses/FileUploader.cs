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

using Microsoft.AspNetCore.Http;
using Models.Defaults;
using Models.Exceptions;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using File = Models.File;

namespace API.HelperClasses
{


    /// <summary>
    /// Interface for file uploader
    /// </summary>
    public interface IFileUploader
    {
        /// <summary>
        /// Uploads single file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <returns> path of file location </returns>
        Task<string> CopyFileToDirectory(IFormFile file, string fileName);

        /// <summary>
        /// Method deletes the file from the file server
        /// </summary>
        /// <param name="file"></param>
        void DeleteFileFromDirectory(File file);
    }

    /// <summary>
    /// Class which is responsible for uploading files
    /// </summary>
    public class FileUploader : IFileUploader
    {

        private readonly string uploadPath;

        /// <summary>
        /// File Uploader
        /// </summary>
        public FileUploader()
        {
            uploadPath = Defaults.Path.FilePath;
        }

        /// <summary>
        /// Uploads single file
        /// </summary>
        /// <param name="file"> File to upload </param>
        /// <param name="fileName"> Name of file </param> 
        /// <returns> path of file location </returns>
        public async Task<string> CopyFileToDirectory(IFormFile file, string fileName)
        {
            try
            {
                if(System.IO.File.Exists(Path.Combine(uploadPath + fileName))) throw new FileExistException(fileName);
                await using Stream sourceStream = file.OpenReadStream();
                await using FileStream destinationStream = System.IO.File.Create(Path.Combine(uploadPath, fileName));
                await sourceStream.CopyToAsync(destinationStream);

                return fileName;
            } catch(Exception e)
            {
                Log.Logger.Error(e, "Unexpected error");
                throw e;
            }
        }

        /// <summary>
        /// Method deletes the file from the file server
        /// </summary>
        /// <param name="file"></param>
        /// <returns> Bool which tells if file is deleted successfully or not </returns>
        public void DeleteFileFromDirectory(File file)
        {
            if(System.IO.File.Exists(Path.Combine(uploadPath, file.Name)))
            {
                System.IO.File.Delete(Path.Combine(uploadPath, file.Name));
                return;
            }

            throw new FileNotFoundException(file.Name);
        }

    }
}
