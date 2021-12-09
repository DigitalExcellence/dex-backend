using Bogus;
using Models;

namespace Services.Tests.Helpers
{

    public class ProjectGeneratorHelper
    {

        private Project _project = new Project();


        public ProjectLike GetOldLike()
        {
            Faker <ProjectLike> faker = new Faker<ProjectLike>();
            faker.RuleFor(l => l.Date, f => f.Date.Past());
            return faker.Generate();
        }

        public ProjectLike GetRecentLike()
        {
            Faker <ProjectLike> faker = new Faker<ProjectLike>();
            faker.RuleFor(l => l.Date, f => f.Date.Recent(2));
            return faker.Generate();
        }

    }

}
