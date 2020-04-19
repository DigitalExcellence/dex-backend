using Bogus;
using System.Collections.Generic;

namespace Repositories.Tests.DataGenerators.Base
{
    public class FakeDataGenerator<T> : IFakeDataGenerator<T> where T : class
    {
        protected Faker<T> Faker;

        public FakeDataGenerator()
        {
            Faker = new Faker<T>();
        }

        public T Generate()
        {
            return Faker.Generate();
        }

        public IEnumerable<T> GenerateRange(int amount)
        {
            return Faker.Generate(amount);
        }
    }
}
