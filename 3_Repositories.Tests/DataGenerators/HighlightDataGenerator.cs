using Bogus;
using Models;
using Repositories.Tests.DataGenerators;
using Repositories.Tests.DataGenerators.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Tests.DataGenerators
{
    public class HighlightDataGenerator : FakeDataGenerator<Highlight>
    {
        public HighlightDataGenerator()
        {
            Faker = new Faker<Highlight>()
                .RuleFor(highlight => highlight.StartDate, faker => faker.Date.Past())
                .RuleFor(highlight => highlight.EndDate, faker => faker.Date.Future());
        }
    }
}
