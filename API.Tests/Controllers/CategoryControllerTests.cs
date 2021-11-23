using API.Tests.Base;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using API.Tests.Enums;
using API.Tests.Helpers;
using Models;
using API.Resources;
using Newtonsoft.Json;

namespace API.Tests.Controllers
{
    public class CategoryControllerTests : BaseTests
    {
        [Theory]
        [InlineData(UserRole.RegisteredUser, HttpStatusCode.Forbidden)]
        [InlineData(UserRole.Admin, HttpStatusCode.Created)]
        [InlineData(UserRole.DataOfficer, HttpStatusCode.Forbidden)]
        [InlineData(UserRole.PrUser, HttpStatusCode.Forbidden)]
        [InlineData(UserRole.Alumni, HttpStatusCode.Forbidden)]
        public async Task CreateCategory_Returns_Expected_Result_For_All_Roles(UserRole role, HttpStatusCode expectedResult)
        {
            // Arrange
            await AuthenticateAs(role);

            // Act
            HttpResponseMessage response = await TestClient.PostAsJsonAsync("category", SeedUtility.RandomCategory());

            // Assert
            response.StatusCode.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(UserRole.RegisteredUser, HttpStatusCode.Forbidden)]
        [InlineData(UserRole.Admin, HttpStatusCode.OK)]
        [InlineData(UserRole.DataOfficer, HttpStatusCode.Forbidden)]
        [InlineData(UserRole.PrUser, HttpStatusCode.Forbidden)]
        [InlineData(UserRole.Alumni, HttpStatusCode.Forbidden)]
        public async Task DeleteCategory_Returns_Expected_Result_For_All_Roles(UserRole role, HttpStatusCode expectedResult)
        {
            // Arrange
            await AuthenticateAs(role);
            HttpResponseMessage addCategoryResponse = await TestClient.PostAsJsonAsync("category", SeedUtility.RandomCategory());
            string responseContent = await addCategoryResponse.Content.ReadAsStringAsync();
            CategoryOutput categoryToDelete = JsonConvert.DeserializeObject<CategoryOutput>(responseContent);

            // Act
            HttpResponseMessage response = await TestClient.DeleteAsync($"category/{categoryToDelete.Id}");

            // Assert
            response.StatusCode.Should().Be(expectedResult);
        }
    }
}
