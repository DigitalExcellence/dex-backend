using Bogus;
using Models;
using Repositories.Tests.DataGenerators.Base;
using System;

namespace Repositories.Tests.DataGenerators
{
    /// <summary>
    /// FakeDataGenerator for the projects
    /// </summary>
    public class EmbeddedProjectDataGenerator : FakeDataGenerator<EmbeddedProject>
    {
        /// <summary>
        /// Initializes the projectDataGenerator
        /// and define dataGenerator options
        /// </summary>
        public EmbeddedProjectDataGenerator()
        {
            Faker<User> ProjectUser = new Faker<User>()
                                   .RuleFor(user => user.Name, faker => faker.Name.FirstName())
                                   .RuleFor(user => user.Email, faker => faker.Internet.Email())
                                   .RuleFor(user => user.IdentityId, faker => faker.Random.Int().ToString());
            Faker<Project> FakeProject = new Faker<Project>()
                    .RuleFor(project => project.Name, faker => faker.Name.FirstName())
                    .RuleFor(project => project.ShortDescription, faker => faker.Lorem.Words(10).ToString())
                    .RuleFor(project => project.Description, faker => faker.Lorem.Words(40).ToString())
                    .RuleFor(project => project.Uri, faker => faker.Internet.Url())
                    .RuleFor(project => project.Created, faker => faker.Date.Past())
                    .RuleFor(project => project.Updated, DateTime.Now)
                    .RuleFor(project => project.User, ProjectUser);

            Faker<User> FakeUser = new Faker<User>()
                    .RuleFor(user => user.Name, faker => faker.Name.FirstName())
                    .RuleFor(user => user.Email, faker => faker.Internet.Email())
                    .RuleFor(user => user.IdentityId, faker => faker.Random.Int().ToString());

            Faker = new Faker<EmbeddedProject>()
                        .RuleFor(p => p.Guid, Guid.NewGuid())
                        .RuleFor(p => p.Project, FakeProject)
                        .RuleFor(p => p.User, FakeUser);
        }
    }
}
