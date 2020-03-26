using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Resources;
using AutoMapper;
using Bogus;
using Models;
using Services.Services;

namespace API.Helpers
{
    public class Seed
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialize a new instance of ProjectController
        /// </summary>
        /// <param name="projectService"></param>
        /// <param name="mapper"></param>
        public Seed(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }
        public async void SeedProjects()
        {
            if (_projectService.GetAll().Result.Count() <= 14)
            {
                for (int i = 0; i < 30; i++)
                {
                    var projectResourceToFake = new Faker<ProjectResource>()
                        .RuleFor(s => s.UserId, f => f.Random.Number(1, 2))
                        .RuleFor(s => s.Uri, f => f.Internet.Url())
                        .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                        .RuleFor(s => s.Description, f => f.Lorem.Letter(150))
                        .RuleFor(s => s.ShortDescription, f => f.Lorem.Letter(30));

                    var projectResource = projectResourceToFake.Generate();

                    Project project =
                        _mapper.Map<ProjectResource, Project>(projectResource);
                    project.Created = DateTime.Now.AddDays(-2);
                    project.Updated = DateTime.Now;
                    _projectService.Add(project);
                }
            }
        }
    }
}
