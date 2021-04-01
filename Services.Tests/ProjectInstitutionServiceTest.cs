using FluentAssertions;
using Models;
using Moq;
using NUnit.Framework;
using Repositories;
using Repositories.Tests.DataSources;
using Services.Services;
using Services.Tests.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Tests
{
    public class ProjectInstitutionServiceTest : ServiceTest<ProjectInstitution, ProjectInstitutionService, IProjectInstitutionRepository>
    {
        protected new IProjectInstitutionService Service => base.Service;


        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void AddRangeTest_GoodFlow([ProjectInstitutionDataSource(30)] IEnumerable<ProjectInstitution> entities)
        {
            base.AddRangeTest_GoodFlow(entities);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void AddTest_GoodFlow([ProjectInstitutionDataSource] ProjectInstitution entity)
        {
            base.AddTest_GoodFlow(entity);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override Task FindAsyncTest_GoodFlow([ProjectInstitutionDataSource] ProjectInstitution entity)
        {
            return base.FindAsyncTest_GoodFlow(entity);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override Task GetAll([ProjectInstitutionDataSource(100)] List<ProjectInstitution> entities)
        {
            return base.GetAll(entities);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void Remove([ProjectInstitutionDataSource] ProjectInstitution entity)
        {
            base.Remove(entity);
        }

        [Test]
        public Task RemoveAsync()
        {
            return base.RemoveAsync(1);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void Save()
        {
            base.Save();
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void Update([ProjectInstitutionDataSource] ProjectInstitution entity)
        {
            base.Update(entity);
        }
    }
}
