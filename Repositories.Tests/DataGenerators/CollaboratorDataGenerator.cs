using Bogus;
using Models;
using Repositories.Tests.DataGenerators.Base;

namespace Repositories.Tests.DataGenerators
{
    /// <summary>
    /// FakeDataGenerator for the collaborators
    /// </summary>
    public class CollaboratorDataGenerator : FakeDataGenerator<Collaborator>
    {
        /// <summary>
        /// Initializes the collaboratorDataGenerator
        /// and define dataGenerator options
        /// </summary>
        public CollaboratorDataGenerator()
        {
            Faker = new Faker<Collaborator>()
                .RuleFor(collaborator => collaborator.FullName, faker => faker.Name.FullName())
                .RuleFor(collaborator => collaborator.Role, faker => faker.Name.FirstName());
        }
    }
}
