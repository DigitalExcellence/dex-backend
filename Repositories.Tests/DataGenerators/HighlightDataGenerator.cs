using Bogus;
using Models;
using Repositories.Tests.DataGenerators.Base;
using System;

namespace Repositories.Tests.DataGenerators
{
    /// <summary>
    /// FakeDataGenerator for the highlights
    /// </summary>
    public class HighlightDataGenerator : FakeDataGenerator<Highlight>
    {
        /// <summary>
        /// Initializes the highlightDataGenerator
        /// and define dataGenerator options
        /// </summary>
        public HighlightDataGenerator()
        {
            Faker = new Faker<Highlight>()
                .RuleFor(highlight => highlight.StartDate, faker => faker.Date.Past())
                .RuleFor(highlight => highlight.EndDate, faker => faker.Date.Future());
        }
    }
}
