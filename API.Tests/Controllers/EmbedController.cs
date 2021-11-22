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
    public class EmbedControllerTests : BaseTests
    {
        [Theory]
        [InlineData(UserRole.RegisteredUser, HttpStatusCode.Created)]
        [InlineData(UserRole.Admin, HttpStatusCode.Created)]
        [InlineData(UserRole.DataOfficer, HttpStatusCode.Created)]
        [InlineData(UserRole.PrUser, HttpStatusCode.Created)]
        //[InlineData(UserRole.Alumni, HttpStatusCode.Created)]
        public async Task CreateEmbed_Returns_Expected_Result_For_All_Roles(UserRole role, HttpStatusCode expectedResult)
        {
            // Arrange
            await AuthenticateAs(role);

            Project project = SeedUtility.RandomProject();
            HttpResponseMessage postProjectResponse = await TestClient.PostAsJsonAsync("project", project);

            string responseContent = await postProjectResponse.Content.ReadAsStringAsync();
            Project projectToEmbed = JsonConvert.DeserializeObject<Project>(responseContent);

            EmbeddedProjectInput embeddedProject = new EmbeddedProjectInput { ProjectId = projectToEmbed.Id };


            // Act
            HttpResponseMessage response = await TestClient.PostAsJsonAsync("embed", embeddedProject);


            // Assert
            response.StatusCode.Should().Be(expectedResult);
        }


        [Theory]
        [InlineData(UserRole.RegisteredUser, HttpStatusCode.Unauthorized)]
        [InlineData(UserRole.Admin, HttpStatusCode.Created)]
        [InlineData(UserRole.DataOfficer, HttpStatusCode.Unauthorized)]
        [InlineData(UserRole.PrUser, HttpStatusCode.Created)]
        [InlineData(UserRole.Alumni, HttpStatusCode.Unauthorized)]
        public async Task CreateEmbed_For_Different_User_Returns_Expected_Result_For_All_Roles2(UserRole role, HttpStatusCode expectedResult)
        {
            // Arrange
            await AuthenticateAs(UserRole.Admin);

            Project project = SeedUtility.RandomProject();
            HttpResponseMessage postProjectResponse = await TestClient.PostAsJsonAsync("project", project);

            string responseContent = await postProjectResponse.Content.ReadAsStringAsync();
            Project projectToEmbed = JsonConvert.DeserializeObject<Project>(responseContent);

            EmbeddedProjectInput embeddedProject = new EmbeddedProjectInput { ProjectId = projectToEmbed.Id };


            // Act
            //Project is not owned by current user so only admins & PR users should be able to embed
            await AuthenticateAs(role);
            HttpResponseMessage response = await TestClient.PostAsJsonAsync("embed", embeddedProject);


            // Assert
            response.StatusCode.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(UserRole.RegisteredUser, HttpStatusCode.Forbidden)]
        [InlineData(UserRole.Admin, HttpStatusCode.OK)]
        [InlineData(UserRole.DataOfficer, HttpStatusCode.Forbidden)]
        [InlineData(UserRole.PrUser, HttpStatusCode.OK)]
        [InlineData(UserRole.Alumni, HttpStatusCode.Forbidden)]
        public async Task GetEmbed_Returns_Expected_Result_For_All_Roles(UserRole role, HttpStatusCode expectedResult)
        {
            // Arrange
            await AuthenticateAs(role);
            


            // Act
            HttpResponseMessage response = await TestClient.GetAsync("embed");


            // Assert
            response.StatusCode.Should().Be(expectedResult);
        }


        //[Theory]
        //[InlineData(UserRole.RegisteredUser, HttpStatusCode.Created)]
        //[InlineData(UserRole.Admin, HttpStatusCode.Created)]
        //[InlineData(UserRole.DataOfficer, HttpStatusCode.Created)]
        //[InlineData(UserRole.PrUser, HttpStatusCode.Created)]
        //[InlineData(UserRole.Alumni, HttpStatusCode.Created)]
        //public async Task DeleteEmbed_Returns_Expected_Result_For_All_Roles(UserRole role, HttpStatusCode expectedResult)
        //{
        //    // Arrange
        //    await AuthenticateAs(role);

        //    Project project = SeedUtility.RandomProject();
        //    HttpResponseMessage postProjectResponse = await TestClient.PostAsJsonAsync("project", project);

        //    string responseContent = await postProjectResponse.Content.ReadAsStringAsync();
        //    ProjectOutput projectToEmbed = JsonConvert.DeserializeObject<ProjectOutput>(responseContent);

        //    EmbeddedProjectInput embeddedProject = new EmbeddedProjectInput { ProjectId = projectToEmbed.Id };
        //    HttpResponseMessage embeddedProjectResponse = await TestClient.PostAsJsonAsync("embed", embeddedProject);
        //    string embedResponseContent = await embeddedProjectResponse.Content.ReadAsStringAsync();
        //    EmbeddedProjectOutput embeddedProjectOutput= JsonConvert.DeserializeObject<EmbeddedProjectOutput>(embedResponseContent);


        //    // Act
        //    HttpResponseMessage response = await TestClient.DeleteAsync($"embed/{embeddedProjectOutput.Guid}");

        //    // Assert
        //    response.StatusCode.Should().Be(expectedResult);
        //}
    }
}
