using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    /// <summary>
    /// Attribute fort maximum file size
    /// </summary>
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int maxFileSize;

        /// <summary>
        /// Constructor for maximum filesize attribute
        /// </summary>
        /// <param name="maxFileSize"></param>
        public MaxFileSizeAttribute(int maxFileSize)
        {
            this.maxFileSize = maxFileSize;
        }

        /// <summary>
        /// Methods which checks if file is not larger than allowed size
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

            if(file.Length > maxFileSize)
            {
                return new ValidationResult(GetErrorMessage());
            }
            
            return ValidationResult.Success;
        }

        /// <summary>
        /// Error messsage
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessage()
        {
            return $"Maximum allowed file size is { maxFileSize} bytes.";
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
