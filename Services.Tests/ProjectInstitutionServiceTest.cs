using Models;
using NUnit.Framework;
using Repositories;
using Services.Services;
using Services.Tests.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Tests
{
    public class ProjectInstitutionServiceTest : ServiceTest<ProjectInstitution, IProjectInstitutionService, IProjectInstitutionRepository>
    {

        [Test]
        public override void AddRangeTest_GoodFlow(IEnumerable<ProjectInstitution> entities)
        {
            base.AddRangeTest_GoodFlow(entities);
        }

        [Test]
        public override void AddTest_GoodFlow(ProjectInstitution entity)
        {
            base.AddTest_GoodFlow(entity);
        }

        [Test]
        public override Task FindAsyncTest_GoodFlow(ProjectInstitution entity)
        {
            return base.FindAsyncTest_GoodFlow(entity);
        }

        [Test]
        public override Task GetAll(List<ProjectInstitution> entities)
        {
            return base.GetAll(entities);
        }

        [Test]
        public override void Remove(ProjectInstitution entity)
        {
            base.Remove(entity);
        }

        [Test]
        public override Task RemoveAsync(int id)
        {
            return base.RemoveAsync(id);
        }

        [Test]
        public override void Save()
        {
            base.Save();
        }

        [Test]
        public override void Update(ProjectInstitution entity)
        {
            base.Update(entity);
        }
    }
}
