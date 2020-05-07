using Bogus;
using Models;
using Repositories.Tests.DataGenerators.Base;

namespace Repositories.Tests.DataGenerators
{
    /// <summary>
    /// FakeDataGenerator for the users
    /// </summary>
    public class UserDataGenerator : FakeDataGenerator<User>
    {
        /// <summary>
        /// Initializes the userDataGenerator
        /// and define dataGenerator options
        /// </summary>
        public UserDataGenerator()
        {
            Faker = new Faker<User>()
                .RuleFor(user => user.Name, faker => faker.Name.FirstName())
                .RuleFor(user => user.Email, faker => faker.Internet.Email())
                .RuleFor(user => user.IdentityId, faker => faker.Random.Int().ToString());
        }
    }
}
