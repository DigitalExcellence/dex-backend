using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

    }

    /// <summary>
    /// Class which is responsible for uploading files
    /// </summary>
    public class FileUploader : IFileUploader
    {

        private static readonly string UploadPath = Directory.GetCurrentDirectory() + "FileFolder";

        /// <summary>
        /// Uploads single file
        /// </summary>
        /// <param name="file"></param>
        /// <returns> path of file location </returns>
        public async Task<string> UploadSingleFile(IFormFile file)
        {
            try
            {
                await using Stream stream = new FileStream(UploadPath + file.Name, FileMode.Create);
                await file.CopyToAsync(stream);
                return UploadPath + file.Name;
            } catch(Exception e)
            {
                Log.Logger.Error(e, "Unexpected error");
                throw e;
            }
        }

    }
}
