using Bogus;
using System.Collections.Generic;

namespace IntegrationTests.Data.DataGenerator.Base
{

    /// <summary>
    ///     Base class to inherit the fakeDataGenerators from
    /// </summary>
    /// <typeparam name="T">Domain class from the Models</typeparam>
    public class FakeDataGenerator<T> : IFakeDataGenerator<T> where T : class
    {

        protected Faker<T> Faker;

        /// <summary>
        ///     Initializes fakeDataGenerator
        /// </summary>
        public FakeDataGenerator()
        {
            Faker = new Faker<T>();
        }

        /// <inheritdoc cref="IFakeDataGenerator{TDomain}" />
        public T Generate()
        {
            return Faker.Generate();
        }

        /// <inheritdoc cref="IFakeDataGenerator{TDomain}" />
        public IEnumerable<T> GenerateRange(int amount)
        {
            return Faker.Generate(amount);
        }

    }

}
