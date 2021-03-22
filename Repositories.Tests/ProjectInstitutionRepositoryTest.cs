using FluentAssertions;
using Models;
using NUnit.Framework;
using Repositories.Tests.Base;
using Repositories.Tests.DataSources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Tests
{
    public class ProjectInstitutionRepositoryTest : RepositoryTest<ProjectInstitution, ProjectInstitutionRepository>
    {
        [Test]
        public override Task AddAsyncTest_GoodFlow([ProjectInstitutionDataSource] ProjectInstitution entity)
        {
            return base.AddAsyncTest_GoodFlow(entity);
        }

        [Test]
        public override void AddRangeTest_BadFlow_EmptyList()
        {
            base.AddRangeTest_BadFlow_EmptyList();
        }

        [Test]
        public override void AddRangeTest_BadFlow_Null()
        {
            base.AddRangeTest_BadFlow_Null();
        }

        [Test]
        public override Task AddRangeTest_GoodFlow([ProjectInstitutionDataSource(30)] List<ProjectInstitution> entities)
        {
            return base.AddRangeTest_GoodFlow(entities);
        }

        [Test]
        public override void AddTest_BadFlow_Null()
        {
            base.AddTest_BadFlow_Null();
        }

        [Test]
        public override Task FindAsyncTest_BadFlow_NotExists([ProjectInstitutionDataSource] ProjectInstitution entity)
        {
            return base.FindAsyncTest_BadFlow_NotExists(entity);
        }

        [Test]
        public override Task FindAsyncTest_GoodFlow([ProjectInstitutionDataSource] ProjectInstitution entity)
        {
            return base.FindAsyncTest_GoodFlow(entity);
        }

        [Test]
        public override Task GetAllAsyncTest_Badflow_Empty()
        {
            return base.GetAllAsyncTest_Badflow_Empty();
        }

        [Test]
        public override Task GetAllAsyncTest_GoodFlow([ProjectInstitutionDataSource(100)]List<ProjectInstitution> entities)
        {
            return base.GetAllAsyncTest_GoodFlow(entities);
        }

        [Test]
        public override Task RemoveAsyncTest_BadFlow_NotExists([ProjectInstitutionDataSource] ProjectInstitution entity)
        {
            return base.RemoveAsyncTest_BadFlow_NotExists(entity);
        }

        [Test]
        public override Task RemoveAsyncTest_GoodFlow([ProjectInstitutionDataSource] ProjectInstitution entity)
        {
            return base.RemoveAsyncTest_GoodFlow(entity);
        }

        [Test]
        public override Task UpdateTest_BadFlow_NotExists([ProjectInstitutionDataSource] ProjectInstitution entity, [ProjectInstitutionDataSource] ProjectInstitution updateEntity)
        {
            return base.UpdateTest_BadFlow_NotExists(entity, updateEntity);
        }

        [Test]
        public override Task UpdateTest_BadFlow_Null([ProjectInstitutionDataSource] ProjectInstitution entity)
        {
            return base.UpdateTest_BadFlow_Null(entity);
        }

        [Test]
        public override Task UpdateTest_GoodFlow([ProjectInstitutionDataSource] ProjectInstitution entity, [ProjectInstitutionDataSource] ProjectInstitution updateEntity)
        {
            return base.UpdateTest_GoodFlow(entity, updateEntity);
        }

        [Test]
        public async Task FindByInstitutionIdAndProjectIdTest([ProjectInstitutionDataSource] ProjectInstitution entity)
        {
            DbContext.Add(entity);
            await DbContext.SaveChangesAsync();

            ProjectInstitution projectInstitution = Repository.FindByInstitutionIdAndProjectId(entity.Project.Id, entity.Institution.Id);

            projectInstitution.Should().NotBeNull();
        }

        [Test]
        public async Task IsInstitutionLinkedToProjectTest([ProjectInstitutionDataSource] ProjectInstitution entity)
        {
            DbContext.Add(entity);
            await DbContext.SaveChangesAsync();

            bool isLinked = Repository.InstitutionIsLinkedToProject(entity.Project.Id, entity.Institution.Id);

            isLinked.Should().BeTrue();
        }

        [Test]
        public async Task RemoveByProjectIdAndInstitutionIdTest([ProjectInstitutionDataSource] ProjectInstitution entity)
        {
            DbContext.Add(entity);
            await DbContext.SaveChangesAsync();

            Repository.RemoveByProjectIdAndInstitutionId(entity.Project.Id, entity.Institution.Id);
            Repository.Save();

            ProjectInstitution projectInstitution = Repository.FindByInstitutionIdAndProjectId(entity.Project.Id, entity.Institution.Id);

            projectInstitution.Should().BeNull();
        }
    }
}
