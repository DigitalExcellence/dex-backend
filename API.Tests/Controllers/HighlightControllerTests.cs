using API.Tests.Base;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using API.Tests.Enums;
using API.Tests.Helpers;
using Newtonsoft.Json;
using Models;

namespace API.Tests.Controllers
{
    public class HighlightControllerTests : BaseTests
    {
        [Theory]
        [InlineData(UserRole.RegisteredUser, HttpStatusCode.Forbidden)]
        [InlineData(UserRole.Admin, HttpStatusCode.Created)]
        [InlineData(UserRole.DataOfficer, HttpStatusCode.Forbidden)]
        [InlineData(UserRole.PrUser, HttpStatusCode.Created)]
        [InlineData(UserRole.Alumni, HttpStatusCode.Forbidden)]
        public async Task CreateHighlight_Returns_Expected_Result_For_All_Roles(UserRole role, HttpStatusCode expectedResult)
        {
            // Arrange
            await AuthenticateAs(role);
            Project project = SeedUtility.RandomProject();
            await TestClient.PostAsJsonAsync("project", project);

            // Act
            HttpResponseMessage response = await TestClient.PostAsJsonAsync("highlight", project);

            // Assert
            response.StatusCode.Should().Be(expectedResult);
        }
    }
}
