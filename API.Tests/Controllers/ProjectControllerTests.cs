using API.Tests.Base;
using Bogus;
using FluentAssertions;
using Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using API.Tests.Enums;
using Microsoft.Extensions.DependencyInjection;
using API.Tests.Helpers;

namespace API.Tests.Controllers
{
    public class ProjectControllerTests : BaseTests
    {
        [Theory]
        [InlineData(UserRole.Admin, HttpStatusCode.OK)]
        [InlineData(UserRole.Alumni, HttpStatusCode.OK)]
        [InlineData(UserRole.DataOfficer, HttpStatusCode.OK)]
        [InlineData(UserRole.PrUser, HttpStatusCode.OK)]
        [InlineData(UserRole.RegisteredUser, HttpStatusCode.OK)]
        public async Task GetAllProjects_Returns_Expected_Result_For_All_Roles(UserRole role, HttpStatusCode expectedResult)
        {
            // Arrange
            await AuthenticateAs(role);

            // Act
            HttpResponseMessage response = await TestClient.GetAsync("project");

            // Assert
            response.StatusCode.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(UserRole.Admin, HttpStatusCode.Created)]
        [InlineData(UserRole.DataOfficer, HttpStatusCode.Created)]
        [InlineData(UserRole.PrUser, HttpStatusCode.Created)]
        [InlineData(UserRole.RegisteredUser, HttpStatusCode.Created)]
        public async Task CreateProject_Returns_Expected_Result_For_All_Roles(UserRole role, HttpStatusCode expectedResult)
        {
            // Arrange
            await AuthenticateAs(role);

            // Act
            HttpResponseMessage response = await TestClient.PostAsJsonAsync("project", SeedUtility.RandomProject());
            
            // Assert
            response.StatusCode.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(UserRole.Admin, HttpStatusCode.OK)]
        [InlineData(UserRole.DataOfficer, HttpStatusCode.OK)]
        [InlineData(UserRole.PrUser, HttpStatusCode.OK)]
        [InlineData(UserRole.RegisteredUser, HttpStatusCode.OK)]
        public async Task DeleteProject_Returns_Expected_Result_For_All_Roles(UserRole role, HttpStatusCode expectedResult)
        {
            // Arrange
            await AuthenticateAs(role);

            HttpResponseMessage addProjectResponse = await TestClient.PostAsJsonAsync("project", SeedUtility.RandomProject());
            string responseContent = await addProjectResponse.Content.ReadAsStringAsync();
            Project projectToDelete = JsonConvert.DeserializeObject<Project>(responseContent);

            // Act
            HttpResponseMessage response = await TestClient.DeleteAsync($"project/{projectToDelete.Id}");

            // Assert
            response.StatusCode.Should().Be(expectedResult);           
        }

        //[Theory]
        //[InlineData(UserRole.RegisteredUser, HttpStatusCode.OK)]
        //[InlineData(UserRole.Admin, HttpStatusCode.OK)]
        //[InlineData(UserRole.DataOfficer, HttpStatusCode.OK)]
        //[InlineData(UserRole.PrUser, HttpStatusCode.OK)]
        //public async Task UpdateProject_Returns_Expected_Result_For_All_Roles(UserRole role, HttpStatusCode expectedResult)
        //{
        //    // Arrange
        //    await AuthenticateAs(role);

        //    HttpResponseMessage addProjectResponse = await TestClient.PostAsJsonAsync("project", SeedUtility.RandomProject());
        //    string responseContent = await addProjectResponse.Content.ReadAsStringAsync();
        //    Project projectToUpdate = JsonConvert.DeserializeObject<Project>(responseContent);

        //    // Act
        //    HttpResponseMessage response = await TestClient.PutAsJsonAsync($"project/{projectToUpdate.Id}", projectToUpdate);

        //    // Assert
        //    response.StatusCode.Should().Be(expectedResult);
        //}

        [Theory]
        [InlineData(UserRole.RegisteredUser, HttpStatusCode.Forbidden)]
        [InlineData(UserRole.Admin, HttpStatusCode.OK)]
        [InlineData(UserRole.DataOfficer, HttpStatusCode.Forbidden)]
        [InlineData(UserRole.PrUser, HttpStatusCode.Forbidden)]
        [InlineData(UserRole.Alumni, HttpStatusCode.Forbidden)]
        public async Task CreateCategory_Returns_Expected_Result_For_All_Roles(UserRole role, HttpStatusCode expectedResult)
        {
            // Arrange
            await AuthenticateAs(role);

            // Act
            HttpResponseMessage response = await TestClient.PostAsJsonAsync("category", SeedUtility.RandomCategoryName());

            // Assert
            response.StatusCode.Should().Be(expectedResult);
        }
    }
}
