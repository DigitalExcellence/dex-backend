using _3_Repositories.Tests.DataGenerators.Base;
using Bogus;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3_Repositories.Tests.DataGenerators
{
    public class UserDataGenerator : FakeDataGenerator<User>
    {
        public UserDataGenerator()
        {
            _faker = new Faker<User>()
                .RuleFor(user => user.Name, faker => faker.Name.FirstName())
                .RuleFor(user => user.Email, faker => faker.Internet.Email())
                .RuleFor(user => user.IdentityId, faker => faker.Random.Int().ToString());
        }
    }
}
