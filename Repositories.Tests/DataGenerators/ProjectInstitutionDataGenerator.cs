using Bogus;
using Models;
using Repositories.Tests.DataGenerators.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Tests.DataGenerators
{
    public class ProjectInstitutionDataGenerator : FakeDataGenerator<ProjectInstitution>
    {
        private ProjectDataGenerator projectGenerator = new ProjectDataGenerator();
        private InstitutionDataGenerator institutionGenerator = new InstitutionDataGenerator();

        public ProjectInstitutionDataGenerator()
        {
            Faker = new Faker<ProjectInstitution>()
                .RuleFor(option => option.Project, _ => projectGenerator.Generate())
                .RuleFor(option => option.Institution, _ => institutionGenerator.Generate());
        }
    }
}
