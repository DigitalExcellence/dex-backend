using Bogus;
using Models;
using Repositories.Tests.DataGenerators.Base;

namespace Repositories.Tests.DataGenerators
{

    public class FileDataGenerator : FakeDataGenerator<File>
    {

        /// <summary>
        ///     Initializes the projectDataGenerator
        ///     and define dataGenerator options
        /// </summary>
        public FileDataGenerator()
        {
            Faker<User> fakeUser = new Faker<User>()
                                   .RuleFor(user => user.Name, faker => faker.Name.FirstName())
                                   .RuleFor(user => user.Email, faker => faker.Internet.Email())
                                   .RuleFor(user => user.IdentityId,
                                            faker => faker.Random.Int()
                                                          .ToString());

            Faker = new Faker<File>()
                    .RuleFor(p => p.Name, faker => faker.Name.FindName())
                    .RuleFor(p => p.Path, faker => faker.System.FilePath())
                    .RuleFor(p => p.UploadDateTime, faker => faker.Date.Past())
                    .RuleFor(p => p.Uploader, fakeUser);
        }

    }

}
