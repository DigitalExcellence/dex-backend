using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File = Models.File;

namespace API.Extensions
{
    /// <summary>
    /// Interface for file uploader
    /// </summary>
    public interface IFileUploader
    {
        /// <summary>
        /// Method that uploads the file to the file server
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<string> UploadSingleFile(IFormFile file);
        bool DeleteFile(File file);

    }

    /// <summary>
    /// Class which is responsible for uploading files
    /// </summary>
    public class FileUploader : IFileUploader
    {

        private static readonly string UploadPath = Directory.GetCurrentDirectory() + "\\files\\";

        /// <summary>
        /// Uploads single file
        /// </summary>
        /// <param name="file"></param>
        /// <returns> path of file location </returns>
        public async Task<string> UploadSingleFile(IFormFile file)
        {
            try
            {
                using(Stream sourceStream = file.OpenReadStream())
                {
                    using(FileStream destinationStream = System.IO.File.Create(UploadPath + file.FileName))
                    {
                        await sourceStream.CopyToAsync(destinationStream);
                    }
                }

                return UploadPath + file.FileName;
            } catch(Exception e)
            {
                Log.Logger.Error(e, "Unexpected error");
                throw e;
            }
        }

        public bool DeleteFile(File file)
        {
            if(System.IO.File.Exists(Path.Combine(UploadPath, file.Name)))
            {
                System.IO.File.Delete(Path.Combine(UploadPath, file.Name));
                return true;
            }

            return false;
        }

    }
}
