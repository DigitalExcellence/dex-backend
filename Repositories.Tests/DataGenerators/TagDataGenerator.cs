using Bogus;
using Models;
using Repositories.Tests.DataGenerators.Base;
using System.Collections.Generic;

namespace Repositories.Tests.DataGenerators
{

    /// <summary>
    ///     FakeDataGenerator for the tags
    /// </summary>
    public class TagDataGenerator : FakeDataGenerator<Tag>
    {

        /// <summary>
        ///     Initializes the TagDataGenerator
        ///     and define dataGenerator options
        /// </summary>
        public TagDataGenerator()
        {
            Faker = new Faker<Tag>()
                    .RuleFor(tag => tag.Name, faker => faker.Hacker.Adjective());
        }

    }

}
