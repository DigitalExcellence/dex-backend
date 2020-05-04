using Bogus;
using Models;
using Repositories.Tests.DataGenerators.Base;
using System;

namespace Repositories.Tests.DataGenerators
{
    public class ProjectDataGenerator : FakeDataGenerator<Project>
    {
        public ProjectDataGenerator()
        {
            Faker = new Faker<Project>()
                .RuleFor(project => project.Name, faker => faker.Name.FirstName())
                .RuleFor(project => project.ShortDescription, faker => faker.Lorem.Words(10).ToString())
                .RuleFor(project => project.Description, faker => faker.Lorem.Words(40).ToString())
                .RuleFor(project => project.Uri, faker => faker.Internet.Url())
                .RuleFor(project => project.Created, faker => faker.Date.Past())
                .RuleFor(project => project.Updated, DateTime.Now);
        }
    }
}
