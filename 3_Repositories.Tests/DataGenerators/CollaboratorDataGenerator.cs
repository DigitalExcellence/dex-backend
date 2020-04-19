using Bogus;
using Models;
using Repositories.Tests.DataGenerators.Base;

namespace Repositories.Tests.DataGenerators
{
    public class CollaboratorDataGenerator : FakeDataGenerator<Collaborator>
    {
        public CollaboratorDataGenerator()
        {
            Faker = new Faker<Collaborator>()
                .RuleFor(collaborator => collaborator.FullName, faker => faker.Name.FullName())
                .RuleFor(collaborator => collaborator.Role, faker => faker.Name.FirstName());
        }
    }
}
