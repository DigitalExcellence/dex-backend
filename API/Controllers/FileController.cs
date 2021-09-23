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
using API.HelperClasses;
using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Defaults;
using Models.Exceptions;
using Services.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using File = Models.File;

namespace API.Controllers
{

    /// <summary>
    ///     This class is responsible for handling HTTP requests that are related
    ///     to file uploading, for example creating, retrieving or deleting.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        private readonly IFileService fileService;
        private readonly IFileUploader fileUploader;
        private readonly IMapper mapper;
        private readonly IUserService userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileController" /> class.
        /// </summary>
        /// <param name="fileService">The file service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="userService">The User service</param>
        /// <param name="fileUploader">The file uploader extension</param>
        public FileController(IFileService fileService,
                              IMapper mapper,
                              IFileUploader fileUploader,
                              IUserService userService)
        {
            this.fileService = fileService;
            this.mapper = mapper;
            this.userService = userService;
            this.fileUploader = fileUploader;
        }

        /// <summary>
        ///     This method is responsible for retrieving all files
        /// </summary>
        /// <returns>A response and list of files.</returns>
        /// <response code="200">This endpoint returns all projects.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<FileOutput>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetFilesAsync()
        {
            IEnumerable<File> files = await fileService.GetAll();

            return Ok(mapper.Map<IEnumerable<File>, IEnumerable<FileOutput>>(files));
        }

        /// <summary>
        ///     This method is responsible for uploading a file
        /// </summary>
        /// <returns>This methods return status code 200 </returns>
        /// <response code="200">This endpoint returns all files.</response>
        /// <response code="400">The 400 bad request is returned when a file is null.</response>
        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        [ControllerAttributes.AllowedFileExtensions(new string[] { ".jpeg", ".png", ".jpg", ".gif" })]
        [ControllerAttributes.MaxFileSize(2097152)]
        [ProducesResponseType(typeof(FileOutput), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UploadSingleFile([FromForm] FileInput fileResource)
        {
            if(fileResource.File == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed posting file.",
                    Detail = "File is null.",
                    Instance = "ACD46F17-A239-4353-92A5-0B81AA0A96E9"
                };
                return BadRequest(problem);
            }
            try
            {
                DateTime uploadDateTime = DateTime.Now;
                int fileExtPos = fileResource.File.FileName.LastIndexOf(".");
                string extension = fileResource.File.FileName.Substring(fileExtPos);
                string newFileName = Guid.NewGuid() + extension;
                User user = await HttpContext.GetContextUser(userService)
                                             .ConfigureAwait(false);
                File file = new File(newFileName, newFileName, user, uploadDateTime);

                await fileUploader.CopyFileToDirectory(fileResource.File, newFileName);

                await fileService.AddAsync(file);
                fileService.Save();

                return Ok(mapper.Map<File, FileOutput>(file));
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
        ///     Find file by id
        /// </summary>
        /// <param name="fileId"></param>
        /// <response code="200">This endpoint returns one single file.</response>
        /// <returns> File </returns>
        [HttpGet("{fileId}")]
        [ProducesResponseType(typeof(FileOutput), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
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

            return Ok(mapper.Map<File, FileOutput>(file));
        }

        /// <summary>
        ///     Deletes single file
        /// </summary>
        /// <param name="fileId"></param>
        /// <response code="200">This endpoint deletes one single file.</response>
        /// <response code="404">The 404 Not Found response is returned when the file was not found.</response>
        /// <response code="401">The 401 Not Authorized response is returned when the user does not have the right credentials.</response>
        /// <returns></returns>
        [HttpDelete("{fileId}")]
        [Authorize]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
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

            bool isAllowed = userService.UserHasScope(user.IdentityId, nameof(Defaults.Scopes.FileWrite));
            if(!(file.Uploader.Id.Equals(user.Id) || isAllowed))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Not authorized.",
                    Detail = "You do not have the required permissions to delete this file.",
                    Instance = "88967A6F-B168-44E2-A8E7-E9EBD555940E"
                };
                return Unauthorized(problem);
            }

            try
            {
                await fileService.RemoveAsync(fileId)
                                 .ConfigureAwait(false);
                fileService.Save();
                fileUploader.DeleteFileFromDirectory(file);
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
