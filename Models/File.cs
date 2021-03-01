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

using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{

    public class File
    {

        public File() { }

        public File(string path, string name, User uploader, DateTime uploadDateTime)
        {
            Path = path;
            Name = name;
            Uploader = uploader;
            UploadDateTime = uploadDateTime;
        }

        public int Id { get; set; }

        [Required]
        public string Path { get; set; }

        public DateTime UploadDateTime { get; set; }

        public string Name { get; set; }

        [Required]
        public User Uploader { get; set; }

    }

}
