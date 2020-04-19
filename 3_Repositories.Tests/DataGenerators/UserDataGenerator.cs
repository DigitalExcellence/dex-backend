using Bogus;
using Models;
using Repositories.Tests.DataGenerators.Base;

namespace Repositories.Tests.DataGenerators
{
    public class UserDataGenerator : FakeDataGenerator<User>
    {
        public UserDataGenerator()
        {
            Faker = new Faker<User>()
                .RuleFor(user => user.Name, faker => faker.Name.FirstName())
                .RuleFor(user => user.Email, faker => faker.Internet.Email())
                .RuleFor(user => user.IdentityId, faker => faker.Random.Int().ToString());
        }
    }
}
