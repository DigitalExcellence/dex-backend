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
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace API.Extensions
{

    /// <summary>
    ///     Attribute for allowed file extensions
    /// </summary>
    public class AllowedExtensionsAttribute : ValidationAttribute
    {

        private readonly string[] extensions;

        /// <summary>
        ///     Constructor for allowed extensions
        /// </summary>
        /// <param name="extensions"> array of extensions </param>
        public AllowedExtensionsAttribute(string[] extensions)
        {
            this.extensions = extensions;
        }

        /// <summary>
        ///     Method which checks if extensions are allowed
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
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
        ///     Error message
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessage()
        {
            return "This file extension is not allowed!";
        }

        /// <summary>
        ///     Error message
        /// </summary>
        /// <returns></returns>
        public string FileIsNullError()
        {
            return "File is null";
        }

    }

}
