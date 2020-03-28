using System;
using System.Linq;
using API.Resources;
using AutoMapper;
using Bogus;
using Models;
using Services.Services;

namespace API.Helpers
{
    /// <summary>
    /// Class for helpers to seed data into the database
    /// </summary>
    public class Seed
    {
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Prepare for seed
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="userService"></param>
        /// <param name="projectService"></param>
        public Seed(IMapper mapper, IUserService userService, IProjectService projectService)
        {
            _mapper = mapper;
            _userService = userService;
            _projectService = projectService;
        }

        /// <summary>
        /// Seed random users into the database using fake date from Bogus
        /// </summary>
        public void SeedUsers()
        {
            if (_userService.GetAll().Result.Count() > 14) return;
            for (var i = 0; i < 30; i++)
            {
                var userResourceToFake = new Faker<UserResource>()
                    .RuleFor(s => s.Name, f => f.Name.FirstName())
                    .RuleFor(s => s.Email, f => f.Internet.Email());

                var userResource = userResourceToFake.Generate();

                var user =
                    _mapper.Map<UserResource, User>(userResource);
                user.IdentityId = (i + 2).ToString();
                if (_userService.GetAll().Result.Count() > 14) return;
                _userService.Add(user);
                _userService.Save();
            }
        }

        /// <summary>
        /// Seed random projects into the database using fake date from Bogus
        /// </summary>
        public void SeedProjects()
        {
            if (_projectService.GetAll().Result.Count() > 14) return;
            for (var i = 0; i < 30; i++)
            {
                var projectResourceToFake = new Faker<ProjectResource>()
                    .RuleFor(s => s.UserId, f => f.Random.Number(1, 9))
                    .RuleFor(s => s.Uri, f => f.Internet.Url())
                    .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                    .RuleFor(s => s.Description, f => f.Lorem.Letter(150))
                    .RuleFor(s => s.ShortDescription, f => f.Lorem.Letter(30));
                var projectResource = projectResourceToFake.Generate();

                var project =
                    _mapper.Map<ProjectResource, Project>(projectResource);
                project.Created = DateTime.Now.AddDays(-2);
                project.Updated = DateTime.Now;
                if (_projectService.GetAll().Result.Count() > 14) return;
                _projectService.Add(project);
                _projectService.Save();
            }
        }
    }
}