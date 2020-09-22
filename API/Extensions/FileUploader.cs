using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
    /// File already exists exception
    /// </summary>
    [Serializable]
    public class FileExistException : Exception
    {
        /// <summary>
        /// File already exist constructor
        /// </summary>
        /// <param name="name"></param>
        public FileExistException(string name)
            : base(String.Format("File {0} already exists", name))
        { }
    }

    /// <summary>
    /// Interface for file uploader
    /// </summary>
    public interface IFileUploader
    {
        /// <summary>
        /// Uploads single file
        /// </summary>
        /// <param name="file"></param>
        /// <returns> path of file location </returns>
        Task<string> UploadSingleFile(IFormFile file);
        /// <summary>
        /// Method deletes the file from the file server
        /// </summary>
        /// <param name="file"></param>
        void DeleteFile(File file);

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
                if(!System.IO.File.Exists(UploadPath + file.FileName))
                {
                    await using(Stream sourceStream = file.OpenReadStream())
                    {
                        await using(FileStream destinationStream = System.IO.File.Create(UploadPath + file.FileName))
                        {
                            await sourceStream.CopyToAsync(destinationStream);
                        }
                    }

                    return UploadPath + file.FileName;
                }

                throw new FileExistException(file.FileName);
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
        /// <returns> Bool which tells if file is deleted succesfully or not </returns>
        public void DeleteFile(File file)
        {
            if(System.IO.File.Exists(Path.Combine(UploadPath, file.Name)))
            {
                System.IO.File.Delete(Path.Combine(UploadPath, file.Name));
                return;
            }
            throw new FileNotFoundException(file.Name);
        }

    }
}
