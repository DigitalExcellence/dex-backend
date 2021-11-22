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
using API.Resources;

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
            HttpResponseMessage postProjectResponse = await TestClient.PostAsJsonAsync("project", project);


            string responseContent = await postProjectResponse.Content.ReadAsStringAsync();
            Project projectToHighlight = JsonConvert.DeserializeObject<Project>(responseContent);

            // Act
            HighlightInput highlightInput = new HighlightInput{ ProjectId = projectToHighlight.Id, Description = projectToHighlight.Description };
            HttpResponseMessage response = await TestClient.PostAsJsonAsync("highlight", highlightInput);

            // Assert
            response.StatusCode.Should().Be(expectedResult);
        }


        [Theory]
        [InlineData(UserRole.RegisteredUser, HttpStatusCode.Forbidden)]
        [InlineData(UserRole.Admin, HttpStatusCode.OK)]
        [InlineData(UserRole.DataOfficer, HttpStatusCode.Forbidden)]
        [InlineData(UserRole.PrUser, HttpStatusCode.OK)]
        [InlineData(UserRole.Alumni, HttpStatusCode.Forbidden)]
        public async Task DeleteHighlight_Returns_Expected_Result_For_All_Roles(UserRole role, HttpStatusCode expectedResult)
        {
            // Arrange
            await AuthenticateAs(role);

            Project project = SeedUtility.RandomProject();
            HttpResponseMessage postProjectResponse = await TestClient.PostAsJsonAsync("project", project);


            string responseContent = await postProjectResponse.Content.ReadAsStringAsync();
            Project projectToHighlight = JsonConvert.DeserializeObject<Project>(responseContent);


            HighlightInput highlightInput = new HighlightInput { ProjectId = projectToHighlight.Id, Description = projectToHighlight.Description };
            HttpResponseMessage response = await TestClient.PostAsJsonAsync($"highlight", highlightInput);

            string highlightResponse = await response.Content.ReadAsStringAsync();
            HighlightOutput highlightOutput = JsonConvert.DeserializeObject<HighlightOutput>(highlightResponse);
           

            // Act
            HttpResponseMessage finalResponse = await TestClient.DeleteAsync($"highlight/{highlightOutput.Id}");
            

            // Assert
            finalResponse.StatusCode.Should().Be(expectedResult);
        }
    }
}
