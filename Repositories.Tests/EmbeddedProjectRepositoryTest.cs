using Models;
using NuGet.Frameworks;
using NUnit.Framework;
using Repositories.Tests.Base;
using Repositories.Tests.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Tests
{
    /// <summary>
    /// EmbeddedProjectRepositoryTest
    /// </summary>
    /// <seealso cref="Repositories.Tests.Base.RepositoryTest{Models.EmbeddedProject, Repositories.EmbedRepository}" />
    [TestFixture]
    public class EmbeddedProjectRepositoryTest : RepositoryTest<EmbeddedProject, EmbedRepository>
    {
        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        protected new IEmbedRepository Repository => (IEmbedRepository) base.Repository;

        /// <summary>
        /// Determines whether [is non existing unique identifier asynchronous unique identifier exists] [the specified project].
        /// </summary>
        /// <param name="project">The project.</param>
        [Test]
        public async Task IsNonExistingGuidAsync_guid_exists([EmbeddedDataSource]EmbeddedProject project)
        {
            // Seed
            DbContext.Add(project);
            await DbContext.SaveChangesAsync();

            // Test
            bool guidExists = await Repository.IsNonExistingGuidAsync(project.Guid);
            Assert.IsFalse(guidExists);
        }

        /// <summary>
        /// Determines whether /[is non existing unique identifier asynchronous unique identifier non existing] [the specified project].
        /// </summary>
        /// <param name="project">The project.</param>
        [Test]
        public async Task IsNonExistingGuidAsync_guid_non_existing([EmbeddedDataSource]EmbeddedProject project)
        {
            // Seed
            DbContext.Add(project);
            await DbContext.SaveChangesAsync();
            // Get random seed
            Guid guid;
            do
            {
                guid = Guid.NewGuid();

            } while(project.Guid == guid);
            // Test
            bool guidExists = await Repository.IsNonExistingGuidAsync(guid);
            Assert.IsTrue(guidExists);
        }

        /// <summary>
        /// Gets the embedded projects goodflow.
        /// </summary>
        /// <param name="embeddedProjects">The embedded projects.</param>
        [Test]
        public async Task GetEmbeddedProjects_goodflow([EmbeddedDataSource(100)]List<EmbeddedProject> embeddedProjects)
        {
            // Seeding
            DbContext.AddRange(embeddedProjects);
            await DbContext.SaveChangesAsync();

            // Test
            IEnumerable<EmbeddedProject> retrieved = await Repository.GetEmbeddedProjectsAsync();
            List<EmbeddedProject> retrievedList = retrieved.ToList();
            Assert.AreEqual(100, retrievedList.Count);
            for(int i = 0; i < 100; i++)
            {
                Assert.AreEqual(embeddedProjects[i],retrievedList[i]);
            }
        }

        /// <summary>
        /// Gets the embedded projects no projects.
        /// </summary>
        [Test]
        public async Task GetEmbeddedProjects_NoProjects()
        {
            // Test
            IEnumerable<EmbeddedProject> embeddedProjectList = await Repository.GetEmbeddedProjectsAsync();
            Assert.IsNotNull(embeddedProjectList);
            Assert.IsFalse(embeddedProjectList.Any());
        }

        /// <summary>
        /// Gets the embedded project goodflow.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="embeddedProjects">The embedded projects.</param>
        [Test]
        public async Task GetEmbeddedProject_goodflow([EmbeddedDataSource]EmbeddedProject project, [EmbeddedDataSource(100)]List<EmbeddedProject> embeddedProjects)
        {
            // Seed
            DbContext.Add(project);
            DbContext.AddRange(embeddedProjects);
            await DbContext.SaveChangesAsync();

            // Test
            EmbeddedProject actualProject = await Repository.GetEmbeddedProjectAsync(project.Guid);
            Assert.AreEqual(project, actualProject);
        }

        /// <summary>
        /// Gets the embedded projects no project.
        /// </summary>
        /// <param name="project">The project.</param>
        [Test]
        public async Task GetEmbeddedProjects_noProject([EmbeddedDataSource]EmbeddedProject project)
        {
            // Test
            Assert.IsNull( await Repository.GetEmbeddedProjectAsync(project.Guid));
        }
    }

}
