using _3_Repositories.Tests.DataGenerators.Base;
using Bogus;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3_Repositories.Tests.DataGenerators
{
    public class CollaboratorDataGenerator : FakeDataGenerator<Collaborator>
    {
        public CollaboratorDataGenerator()
        {
            _faker = new Faker<Collaborator>()
                .RuleFor(collaborator => collaborator.FullName, faker => faker.Name.FullName())
                .RuleFor(collaborator => collaborator.Role, faker => faker.Name.FirstName());
        }
    }
}
