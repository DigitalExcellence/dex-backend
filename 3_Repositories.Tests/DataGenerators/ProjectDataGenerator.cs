using _3_Repositories.Tests.DataGenerators.Base;
using Bogus;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3_Repositories.Tests.DataGenerators
{
    public class ProjectDataGenerator : FakeDataGenerator<Project>
    {
        public ProjectDataGenerator()
        {
            _faker = new Faker<Project>()
                .RuleFor(project => project.Name, faker => faker.Name.FirstName())
                .RuleFor(project => project.ShortDescription, faker => faker.Lorem.Words(10).ToString())
                .RuleFor(project => project.Description, faker => faker.Lorem.Words(40).ToString())
                .RuleFor(project => project.Uri, faker => faker.Internet.Url())
                .RuleFor(project => project.Created, faker => faker.Date.Past())
                .RuleFor(project => project.Created, DateTime.Now);
        }
    }
}
