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

using API.Extensions;
using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Models;
using RestSharp;
using Services.Services;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using File = Models.File;

namespace API.Controllers
{
    /// <summary>
    ///     File controller for files
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        private readonly IFileService fileService;
        private readonly IMapper mapper;
        private readonly IProjectService projectService;
        private readonly IUserService userService;
        private readonly IFileUploader fileUploader;
        /// <summary>
        /// Initializes a new instance of the <see cref="FileController"/> class.
        /// </summary>
        /// <param name="fileService">The file service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="projectService">The Project service</param>
        /// <param name="userService">The User service</param>
        /// /// <param name="fileUploader">The file uploader extension</param>
        public FileController(IFileService fileService, IMapper mapper, IProjectService projectService, IFileUploader fileUploader, IUserService userService)
        {
            this.fileService = fileService;
            this.mapper = mapper;
            this.projectService = projectService;
            this.userService = userService;
            this.fileUploader = fileUploader;
        }

        /// <summary>
        /// Get all files
        /// </summary>
        /// <returns>A response and list of files.</returns>
        [HttpGet]
        public async Task<IActionResult> GetFilesAsync()
        {
            IEnumerable<File> files = await fileService.GetFilesAsync();
            if(!files.Any())
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting files.",
                                             Detail = "The database does not contain any files.",
                                             Instance = "47525791-57C4-4DE2-91B1-90086D893112"
                };
                return NotFound(problem);
            }

            return Ok(mapper.Map<IEnumerable<File>, IEnumerable<FileResourceResult>>(files));
        }

        /// <summary>
        /// Uploads a single file
        /// </summary>
        /// <returns>HTTP Response</returns>
        [HttpPost]
        public async Task<IActionResult> UploadSingleFile()
        {
            IFormFile fileItem = HttpContext.Request.Form.Files["File"];

            if(fileItem == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed posting file.",
                                             Detail = "File is null.",
                                             Instance = "ACD46F17-A239-4353-92A5-0B81AA0A96E9"
                                         };
                return NotFound(problem);
            }

            string path = await fileUploader.UploadSingleFile(fileItem);

            User user = await HttpContext.GetContextUser(userService).ConfigureAwait(false);
            File file = new File(path, fileItem.Name, user.Id);
            fileService.UploadSingleFile(file);

            return Ok(file);
        }

    }
}
