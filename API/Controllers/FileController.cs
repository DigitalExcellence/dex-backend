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
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Defaults;
using Services.Services;
using System;
using System.IO;
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> UploadSingleFile([FromForm] FileResource fileResource)
        {
            if(fileResource.File == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed posting file.",
                    Detail = "File is null.",
                    Instance = "ACD46F17-A239-4353-92A5-0B81AA0A96E9"
                };
                return NotFound(problem);
            }
            try
            {
                DateTime uploadDateTime = DateTime.Now;
                int fileExtPos = fileResource.File.FileName.LastIndexOf(".");
                string extension = fileResource.File.FileName.Substring(fileExtPos);
                string newFileName = fileUploader.RemoveSpecialCharacters(fileResource.File.FileName.Remove(fileExtPos) + Guid.NewGuid() + extension);
                string path = await fileUploader.UploadSingleFile(fileResource.File, newFileName);
                User user = await HttpContext.GetContextUser(userService)
                                             .ConfigureAwait(false);

                File file = new File(path, newFileName, user, uploadDateTime);
                fileService.UploadSingleFile(file);

                return Ok(mapper.Map<File, FileResourceResult>(file));
            } catch(FileExistException fileExistException)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = fileExistException.Message,
                                             Detail = "Please rename filename.",
                                             Instance = "D902F8C6-23FF-4506-B272-C757BD709464"
                };
                return BadRequest(problem);
            }
        }

        /// <summary>
        /// Find file by id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns> File </returns>
        [HttpGet("{fileId}")]
        public async Task<IActionResult> GetSingleFile(int fileId)
        {
            File file = await fileService.FindAsync(fileId);

            if(file == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "File could not be found.",
                    Detail = "File could not be found.",
                    Instance = "875B6402-D771-45EC-AB56-3DE0CDD446D6"
                };
                return NotFound(problem);
            }

            return Ok(file);
        }

        /// <summary>
        /// Deletes single file
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpDelete("{fileId}")]
        [Authorize]
        public async Task<IActionResult> DeleteSingleFile(int fileId)
        {
            File file = await fileService.FindAsync(fileId);
            User user = await HttpContext.GetContextUser(userService)
                                         .ConfigureAwait(false);

            if(file == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "File was not found.",
                                             Detail = "File was not found.",
                                             Instance = "9D3830A2-E7D1-4610-A147-1D43BFB8DDBC"
                };
                return NotFound(problem);
            }

            if(!file.Uploader.Id.Equals(user.Id))
            {
                if(!userService.UserHasScope(user.IdentityId, Defaults.Roles.Administrator))
                {
                    ProblemDetails problem = new ProblemDetails
                                             {
                                                 Title = "Not authorized.",
                                                 Detail = "You are not the uploader of this file.",
                                                 Instance = "88967A6F-B168-44E2-A8E7-E9EBD555940E"
                                             };
                    return Unauthorized(problem);
                }
            }

            try
            {
                await fileService.RemoveAsync(fileId)
                                    .ConfigureAwait(false);
                fileService.Save();
                fileUploader.DeleteFile(file);
                return Ok();
            } catch(FileNotFoundException)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "File could not be deleted because the path does not exist.",
                                             Detail = "File could not be found.",
                                             Instance = "436349B4-50D9-49FD-8618-82367BEB7941"
                                         };

                return NotFound(problem);
            } 
            
        }

    }
}
