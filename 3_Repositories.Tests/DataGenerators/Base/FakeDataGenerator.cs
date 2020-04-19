using Bogus;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3_Repositories.Tests.DataGenerators.Base
{
    public class FakeDataGenerator<T> : IFakeDataGenerator<T> where T : class
    {
        protected Faker<T> _faker;

        public FakeDataGenerator()
        {
            _faker = new Faker<T>();
        }

        public T Generate()
        {
            return _faker.Generate();
        }

        public IEnumerable<T> GenerateRange(int amount)
        {
            return _faker.Generate(amount);
        }
    }
}
