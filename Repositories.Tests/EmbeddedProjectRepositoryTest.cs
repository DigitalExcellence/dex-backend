using Models;
using Models.Defaults;
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
        /// Determines whether [is non existing unique identifier asynchronous unique identifier exists] [the specified embeddedProject].
        /// </summary>
        /// <param name="project">The embeddedProject that will be seeded.</param>
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
        /// Determines whether /[is non existing unique identifier asynchronous unique identifier non existing] [the specified embeddedProject].
        /// </summary>
        /// <param name="project">The embeddedProject that will be seeded.</param>
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
        /// <param name="embeddedProjects">The embeddedProject that will be seeded.</param>
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
        /// Gets the embedded embeddedProject goodflow.
        /// </summary>
        /// <param name="project">The embeddedProject that will be seeded.</param>
        /// <param name="embeddedProjects">The embeddedProject that will be seeded.</param>
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
        /// Gets the embedded projects no embeddedProject.
        /// </summary>
        /// <param name="project">The embeddedProject that will be seeded.</param>
        [Test]
        public async Task GetEmbeddedProjects_noProject([EmbeddedDataSource]EmbeddedProject project)
        {
            // Test
            Assert.IsNull( await Repository.GetEmbeddedProjectAsync(project.Guid));
        }

        /// <summary>
        /// test if the function findasync finds an embedded project and adheres to the IsPublic flag meaning if the flag is false the email field should be redacted.
        /// </summary>
        /// <param name="project">The embeddedProject that will be seeded.</param>
        [Test]
        public async Task FindAsync_User_IsPublic_True([EmbeddedDataSource]EmbeddedProject project)
        {
            project.User.IsPublic = true;
            // Seed
            DbContext.Add(project);
            await DbContext.SaveChangesAsync();

            // Test
            EmbeddedProject actualProject = await Repository.FindAsync(project.Id);
            Assert.AreEqual(project.User.Email, actualProject.User.Email);
        }

        /// <summary>
        /// test if the function findasync finds an embedded project and adheres to the IsPublic flag meaning if the flag is false the email field should be redacted.
        /// </summary>
        /// <param name="project">The embeddedProject that will be seeded.</param>
        [Test]
        public async Task FindAsync_User_IsPublic_False([EmbeddedDataSource]EmbeddedProject project)
        {
            project.User.IsPublic = false;
            // Seed
            DbContext.Add(project);
            await DbContext.SaveChangesAsync();

            // Test
            EmbeddedProject actualProject = await Repository.FindAsync(project.Id);
            Assert.AreEqual( actualProject.User.Email, Defaults.Privacy.RedactedEmail);
        }

        /// <summary>
        /// test if the function getEmbeddedProjectasync finds an embedded project by the guid and adheres to the IsPublic flag meaning if the flag is false the email field should be redacted.
        /// </summary>
        /// <param name="project">The embeddedProject that will be seeded.</param>
        [Test]
        public async Task GetEmbeddedProjectAsync_User_IsPublic_True([EmbeddedDataSource]EmbeddedProject project)
        {
            project.User.IsPublic = true;
            // Seed
            DbContext.Add(project);
            await DbContext.SaveChangesAsync();

            // Test
            EmbeddedProject actualProject = await Repository.GetEmbeddedProjectAsync(project.Guid);
            Assert.AreEqual(actualProject.User.Email, project.User.Email);
        }

        /// <summary>
        /// test if the function getEmbeddedProjectasync finds an embedded project by the guid and adheres to the IsPublic flag meaning if the flag is false the email field should be redacted.
        /// </summary>
        /// <param name="project">The project that will be seeded.</param>
        [Test]
        public async Task GetEmbeddedProjectAsync_User_IsPublic_False([EmbeddedDataSource]EmbeddedProject project)
        {
            project.User.IsPublic = false;
            // Seed
            DbContext.Add(project);
            await DbContext.SaveChangesAsync();

            // Test
            EmbeddedProject actualProject = await Repository.GetEmbeddedProjectAsync(project.Guid);
            Assert.AreEqual(actualProject.User.Email, Defaults.Privacy.RedactedEmail);
        }

        /// <summary>
        /// test if the function getEmbeddedProjectasync finds an embedded project by the guid and adheres to the IsPublic flag meaning if the flag is false the email field should be redacted. on the project user.
        /// </summary>
        /// <param name="embeddedProject">The embeddedProject that will be seeded.</param>
        /// <param name="project">The project that will be seeded.</param>
        /// <param name="user">The user that will be seeded.</param>
        [Test]
        public async Task GetEmbeddedProjectAsync_ProjectUser_IsPublic_True([EmbeddedDataSource]EmbeddedProject embeddedProject, [ProjectDataSource]Project project, [UserDataSource]User user)
        {
            user.IsPublic = true;
            project.User = user;
            embeddedProject.Project = project;
            // Seed
            DbContext.Add(user);
            DbContext.Add(project);
            DbContext.Add(embeddedProject);
            await DbContext.SaveChangesAsync();

            // Test
            EmbeddedProject actualProject = await Repository.GetEmbeddedProjectAsync(embeddedProject.Guid);
            Assert.AreEqual(actualProject.Project.User.Email, embeddedProject.Project.User.Email);
        }

        /// <summary>
        /// test if the function getEmbeddedProjectasync finds an embedded project by the guid and adheres to the IsPublic flag meaning if the flag is false the email field should be redacted. on the project user.
        /// </summary>
        /// <param name="embeddedProject">The embedded project that will be seeded.</param>
        /// <param name="project">The project that will be seeded.</param>
        /// <param name="user">The user that will be seeded.</param>
        [Test]
        public async Task GetEmbeddedProjectAsync_ProjectUser_IsPublic_False([EmbeddedDataSource]EmbeddedProject embeddedProject, [ProjectDataSource]Project project, [UserDataSource]User user)
        {
            user.IsPublic = false;
            project.User = user;
            embeddedProject.Project = project;
            // Seed
            DbContext.Add(user);
            DbContext.Add(project);
            DbContext.Add(embeddedProject);
            await DbContext.SaveChangesAsync();

            // Test
            EmbeddedProject actualProject = await Repository.GetEmbeddedProjectAsync(embeddedProject.Guid);
            Assert.AreEqual(actualProject.Project.User.Email, Defaults.Privacy.RedactedEmail);
        }


    }

}
