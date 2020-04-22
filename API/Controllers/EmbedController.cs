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

using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Defaults;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{

    /// <summary>
    /// Embedded iframe controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class EmbedController : ControllerBase
    {

        private readonly IEmbedService embedService;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbedController"/> class.
        /// </summary>
        /// <param name="embedService">The embed service.</param>
        /// <param name="mapper">The mapper.</param>
        public EmbedController(IEmbedService embedService, IMapper mapper)
        {
            this.embedService = embedService;
            this.mapper = mapper;
        }


        /// <summary>
        /// Gets all embedded projects.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = nameof(Defaults.Scopes.EmbedRead))]
        public async Task<IActionResult> GetAllEmbeddedProjects()
        {
            IEnumerable<EmbeddedProject> embeddedProjects = await embedService.GetEmbeddedProjectsAsync();

            if(!embeddedProjects.Any())
            {
                return NotFound();
            }
            return Ok(mapper.Map<IEnumerable<EmbeddedProject>, IEnumerable<EmbeddedProjectResourceResult>>(embeddedProjects));

        }

        /// <summary>
        /// Gets the embedded project.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        public async Task<IActionResult> GetEmbeddedProject(string guid)
        {

            EmbeddedProject embeddedProject = await embedService.FindAsync(new Guid(guid));
            if(embeddedProject == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<Project, ProjectResourceResult>(embeddedProject.Project));
        }

        /// <summary>
        ///     Creates a embedded project
        /// </summary>
        /// <param name="embedResource"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = nameof(Defaults.Scopes.EmbedWrite))]
        public async Task<IActionResult> CreateEmbeddedProject(EmbeddedProjectResource embedResource)
        {
            if(embedResource == null)
            {
                return BadRequest("embed is null");
            }
            EmbeddedProject embeddedProject = mapper.Map<EmbeddedProjectResource, EmbeddedProject>(embedResource);
            // Do user validation about the rights of the project

            //Ensure we have a non existing Guid
            Guid guid;
            while(true)
            {
                guid = Guid.NewGuid();
                if(await embedService.IsNonExistingGuid(guid))
                {
                    break;
                }
            }
            embeddedProject.Guid = guid;

            try
            {
                embedService.Add(embeddedProject);
                embedService.Save();
                return Created(nameof(CreateEmbeddedProject), mapper.Map<EmbeddedProject, EmbeddedProjectResourceResult>(embeddedProject));
            } catch
            {
                return BadRequest("Could not Create the Embedded project");
            }
        }

        /// <summary>
        /// Deletes the highlight.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        [HttpDelete("{guid}")]
        [Authorize(Policy = nameof(Defaults.Scopes.EmbedWrite))]
        public async Task<IActionResult> DeleteHighlight(string guid)
        {
            EmbeddedProject embeddedProject = await embedService.FindAsync(new Guid(guid));
            if( embeddedProject == null)
            {
                return NotFound();
            }

            //TODO validate permissions.
            await embedService.RemoveAsync(embeddedProject.Id);
            embedService.Save();
            return Ok();
        }

    }

}
