using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    /// <summary>
    /// Attribute for allowed file extensions
    /// </summary>
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] extensions;

        /// <summary>
        /// Constructor for allowed extensions
        /// </summary>
        /// <param name="extensions"> array of extensions </param>
        public AllowedExtensionsAttribute(string[] extensions)
        {
            this.extensions = extensions;
        }

        /// <summary>
        /// Method which checks if extensions are allowed
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            IFormFile file = value as IFormFile;
            if(file == null)
            {
                return new ValidationResult(FileIsNullError());
            }

            string extension = Path.GetExtension(file.FileName);
            
            if(!extensions.Contains(extension.ToLower()))
            {
                return new ValidationResult(GetErrorMessage());
            }
            

            return ValidationResult.Success;
        }

        /// <summary>
        ///  Error message
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessage()
        {
            return $"This file extension is not allowed!";
        }

        /// <summary>
        /// Error message
        /// </summary>
        /// <returns></returns>
        public string FileIsNullError()
        {
            return "File is null";
        }
    }
}
