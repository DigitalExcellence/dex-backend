using FluentAssertions;
using Models;
using NUnit.Framework;
using Repositories.Tests.Base;
using Repositories.Tests.DataSources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Tests
{
    /// <summary>
    /// InstitutionRepositoryTest
    /// </summary>
    /// <seealso cref="RepositoryTest{Institution, InstitutionRepository}" />
    [TestFixture]
    public class InstitutionRepositoryTest : RepositoryTest<Institution, InstitutionRepository>
    {

        /// <summary>
        /// Gets the repository
        /// </summary>
        /// <value>
        /// The repository
        /// </value>
        protected new IInstitutionRepository Repository => (IInstitutionRepository) base.Repository;

        /// <summary>
        /// This method tests the GetInstitutionsAsync method in a good flow scenario.
        /// </summary>
        /// <param name="institutions">The institutions stored, generated to mock the institutions from the database.</param>
        /// <returns>This method will return a passing result for the test.</returns>
        [Test]
        public async Task GetInstitutionsAsync_Goodflow([InstitutionDataSource(100)]IEnumerable<Institution> institutions)
        {
            // Arrange
            DbContext.AddRange(institutions);
            await DbContext.SaveChangesAsync();

            // Act
            IEnumerable<Institution> retrievedInstitutions = await Repository.GetInstitutionsAsync();

            // Assert
            retrievedInstitutions.Count().Should().Be(100);
            retrievedInstitutions.Should().BeEquivalentTo(institutions);
        }

        /// <summary>
        /// This method tests the GetInstitutionsAsync method whenever there are no institutions stored.
        /// </summary>
        /// <returns>This method will return a passing result for the test.</returns>
        [Test]
        public async Task GetInstitutionsAsync_NoInstitutions()
        {
            // Arrange

            // Act
            IEnumerable<Institution> retrievedInstitutions = await Repository.GetInstitutionsAsync();

            // Assert
            retrievedInstitutions.Should().NotBeNull();
            retrievedInstitutions.Should().BeEmpty();
        }

    }

}
