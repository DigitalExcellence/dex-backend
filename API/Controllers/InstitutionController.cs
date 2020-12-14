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
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Defaults;
using Serilog;
using Services.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{

    /// <summary>
    /// This class is responsible for handling HTTP requests that are related
    /// to the institutions, for example creating, retrieving, updating or deleting.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionController : ControllerBase
    {

        private readonly IInstitutionService institutionService;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstitutionController"/> class.
        /// </summary>
        /// <param name="institutionService">The institution service which is used to communicate with the logic layer.</param>
        /// <param name="mapper">The mapper which is used to convert the resources to the models to the resource results.</param>
        public InstitutionController(IInstitutionService institutionService, IMapper mapper)
        {
            this.institutionService = institutionService;
            this.mapper = mapper;
        }

        /// <summary>
        /// This method is responsible for retrieving all the institutions.
        /// </summary>
        /// <returns>This method returns a list of institution resource results.</returns>
        /// <response code="200">This endpoint returns a list of institutions.</response>
        /// <response code="404">The 404 Not Found status code is returned when no institutions are found.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InstitutionResourceResult>), (int) HttpStatusCode.OK)]
        [Authorize(Policy = nameof(Defaults.Scopes.InstitutionRead))]
        public async Task<IActionResult> GetAllInstitutions()
        {
            IEnumerable<Institution> institutions = await institutionService.GetInstitutionsAsync();

            if(!institutions.Any())
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "No Institutions found",
                    Detail = "There where no institutions scopes found.",
                    Instance = "7736D583-3263-4946-BF48-35299812AA56"
                };
                return NotFound(problem);
            }

            IEnumerable<InstitutionResourceResult> model =
                mapper.Map<IEnumerable<Institution>, IEnumerable<InstitutionResourceResult>>(institutions);
            return Ok(model);
        }

        /// <summary>
        /// This method is responsible for retrieving a single institution by id.
        /// </summary>
        /// <param name="id">The unique identifier which is used for searching the institution.</param>
        /// <returns>This method returns the institution resource result.</returns>
        /// <response code="200">This endpoint returns an institution with the specified id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the id is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when no institution could be
        /// found with the specified id.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InstitutionResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        [Authorize(Policy = nameof(Defaults.Scopes.InstitutionRead))]
        public async Task<IActionResult> GetInstitution(int id)
        {
            if(id <= 0)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Invalid id specified.",
                    Detail = "The specified id is invalid.",
                    Instance = "17DE6E26-6759-423D-A33B-8CEC38F158A3"
                };
                return BadRequest(problem);
            }

            Institution institution = await institutionService.FindAsync(id);
            if(institution == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the institution.",
                    Detail = "The database does not contain an institution with the specified id.",
                    Instance = "89378B11-601A-4512-BFF9-F02FC510DF03"
                };
                return NotFound(problem);
            }

            InstitutionResourceResult model = mapper.Map<Institution, InstitutionResourceResult>(institution);
            return Ok(model);
        }

        /// <summary>
        /// This method is responsible for creating an institution.
        /// </summary>
        /// <param name="institutionResource">The institution resource which is used to create the institution.</param>
        /// <returns>This method returns the created institution resource result.</returns>
        /// <response code="201">This endpoint returns the created institution.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified
        /// resource is invalid or the institution could not be saved to the database.</response>
        [HttpPost]
        [Authorize(Policy = nameof(Defaults.Scopes.InstitutionWrite))]
        [ProducesResponseType(typeof(InstitutionResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public IActionResult CreateInstitution(InstitutionResource institutionResource)
        {
            if(institutionResource == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed creating the institution.",
                    Detail = "The institution resource is null.",
                    Instance = "E80F9611-EE07-4FF0-8D53-7693CE1AE26E"
                };
                return BadRequest(problem);
            }

            Institution institution = mapper.Map<InstitutionResource, Institution>(institutionResource);

            try
            {
                institutionService.Add(institution);
                institutionService.Save();
                InstitutionResourceResult model = mapper.Map<Institution, InstitutionResourceResult>(institution);
                return Created(nameof(CreateInstitution), model);
            }
            catch(DbUpdateException e)
            {
                Log.Logger.Error(e, "Database exception");

                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed Saving the institution.",
                    Detail = "Failed saving the institution to the database.",
                    Instance = "20C197B7-24E1-4112-8999-6BB3DFD03FB6"
                };
                return BadRequest(problem);
            }
        }

        /// <summary>
        /// This method is responsible for updating the institution.
        /// </summary>
        /// <param name="institutionId">The institution identifier which is used to find the institution.</param>
        /// <param name="institutionResource">The institution resource which is used to update the institution.</param>
        /// <returns>This method returns the updated institution resource result.</returns>
        /// <response code="200">This endpoint returns the updated institution.</response>
        /// <response code="404">The 404 Not Found status code is returned when no institution is found with the specified institution id.</response>
        [HttpPut("{institutionId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.InstitutionWrite))]
        [ProducesResponseType(typeof(InstitutionResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateInstitution(int institutionId,
                                                           [FromBody] InstitutionResource institutionResource)
        {
            Institution institution = await institutionService.FindAsync(institutionId);
            if(institution == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the institution.",
                    Detail = "The database does not contain an institution with that id.",
                    Instance = "FA51F980-D114-44B3-85BD-9419B58D68F2"
                };
                return NotFound(problem);
            }

            mapper.Map(institutionResource, institution);

            institutionService.Update(institution);
            institutionService.Save();

            return Ok(mapper.Map<Institution, InstitutionResourceResult>(institution));
        }

        /// <summary>
        /// This method is responsible for deleting the institution by the identifier.
        /// </summary>
        /// <param name="institutionId">The institution identifier which is used to find the institution.</param>
        /// <returns>This method returns status code 200.</returns>
        /// <response code="200">This endpoint returns status code 200. The institution is deleted.</response>
        /// <response code="404">The 404 Not Found status code is returned when no institution is found with the specified id.</response>
        [HttpDelete("{institutionId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.InstitutionWrite))]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteInstitution(int institutionId)
        {
            Institution institution = await institutionService.FindAsync(institutionId);
            if(institution == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the institution.",
                    Detail = "The database does not contain an institution with that id.",
                    Instance = "9981200A-B22C-42B3-8035-5D3A6B9696C8"
                };
                return NotFound(problem);
            }

            await institutionService.RemoveAsync(institutionId);
            institutionService.Save();
            return Ok();

        }

    }

}
