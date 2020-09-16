using Bogus;
using Models;
using Repositories.Tests.DataGenerators;
using Repositories.Tests.DataGenerators.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Tests.DataGenerators
{
    /// <summary>
    /// FakeDataGenerator for the highlights
    /// </summary>
    public class HighlightDataGenerator : FakeDataGenerator<Highlight>
    {
        /// <summary>
        /// Initializes the highlightDataGenerator
        /// and define dataGenerator options
        /// </summary>
        public HighlightDataGenerator()
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

            Faker = new Faker<Highlight>()
                    .RuleFor(highlight => highlight.Project, FakeProject)
                    .RuleFor(highlight => highlight.StartDate, faker => faker.Date.Past())
                    .RuleFor(highlight => highlight.EndDate, faker => faker.Date.Future());
        }
    }
}
