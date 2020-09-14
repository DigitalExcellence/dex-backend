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

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

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
        /// <summary>
        /// Initializes a new instance of the <see cref="FileController"/> class.
        /// </summary>
        /// <param name="fileService">The file service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="projectService">The Project service</param>
        /// <param name="userService">The User service</param>
        public FileController(IFileService fileService, IMapper mapper, IProjectService projectService, IUserService userService)
        {
            this.fileService = fileService;
            this.mapper = mapper;
            this.projectService = projectService;
            this.userService = userService;
        }
    }
}
