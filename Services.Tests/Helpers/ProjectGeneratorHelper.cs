using Bogus;
using Bogus.DataSets;
using Models;
using System;
using System.Collections.Generic;

namespace Services.Tests.Helpers
{

    public class ProjectGeneratorHelper
    {

        private Project _project = new Project();


        public ProjectLike GetOldLike()
        {
            Faker <ProjectLike> faker = new Faker<ProjectLike>();
            faker.RuleFor(l => l.Date, f => f.Date.Past());
            return faker.Generate();
        }

        public ProjectLike GetRecentLike()
        {
            Faker <ProjectLike> faker = new Faker<ProjectLike>();
            faker.RuleFor(l => l.Date, f => f.Date.Recent(2));
            return faker.Generate();
        }


        public Project GetInactiveProject(int multiplier = 1)
        {
            Faker<Project>  faker = new Faker<Project>();

            faker.RuleFor(p => p.Name, f => f.Company.CompanyName());
            faker.RuleFor(p => p.Description, f => f.Lorem.Paragraph(2));
            faker.RuleFor(p => p.Created, f => f.Date.Past(3 * multiplier));
            faker.RuleFor(p => p.Updated, f => f.Date.Past(2 * multiplier));

            Project project = faker.Generate();

            project.Categories = new List<ProjectCategory>();
            project.CallToActions = new List<CallToAction>();
            project.Images = new List<File>();

            project.Likes = new List<ProjectLike>()
                            {
                                this.GetOldLike(),
                                this.GetOldLike(),
                                this.GetOldLike(),
                                this.GetOldLike(),
                                this.GetOldLike()
                            };

            for(int i = 0; i < multiplier; i++)
            {
                project.Likes.Add(GetRecentLike());
            }

            return project;
        }


        public Project GetActiveProject(int multiplier = 1)
        {
            Faker<Project>  faker = new Faker<Project>();

            faker.RuleFor(p => p.Name, f => f.Company.CompanyName());
            faker.RuleFor(p => p.Description, f => f.Lorem.Paragraph(2));
            faker.RuleFor(p => p.Created, f => f.Date.Recent(2 * multiplier));
            faker.RuleFor(p => p.Updated, f => f.Date.Past(2 * multiplier));
            faker.RuleFor(p => p.Uri, f => f.Internet.Url());

            Project project = faker.Generate();

            project.Categories = new List<ProjectCategory>()
                                 {
                                     new ProjectCategory(),
                                     new ProjectCategory()
                                 };

            project.Likes = new List<ProjectLike>()
                            {
                                this.GetOldLike(),
                                this.GetRecentLike(),
                                this.GetRecentLike(),
                                this.GetRecentLike(),
                                this.GetRecentLike(),
                                this.GetRecentLike(),
                            };


            project.Collaborators = new List<Collaborator>()
                                    {
                                        new Collaborator(),
                                        new Collaborator()
                                    };

            project.CallToActions = new List<CallToAction>()
                                    {
                                        new CallToAction(),
                                        new CallToAction(),
                                        new CallToAction(),
                                        new CallToAction(),
                                    };

            project.Images = new List<File>()
                             {
                                 new File(),
                                 new File(),
                                 new File()
                             };

            for(int i = 0; i < multiplier; i++)
            {
                project.Likes.Add(GetRecentLike());
                project.Collaborators.Add(new Collaborator());
            }


            return project;
        }

    }

}
