using API.Tests.Base;
using API.Resources;
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

namespace API.Tests.Controllers
{
    public class ProjectControllerTests : BaseTests
    {
        [Theory]
        [InlineData(UserRole.Admin, HttpStatusCode.OK)]
        public async Task GetProject_Returns_Expected_Result_For_All_Roles(
            UserRole role,
            HttpStatusCode expectedResult)
        {
            // Arrange
            await AuthenticateAs((int) role);
            
            

            // Act
            HttpResponseMessage response = await TestClient2.GetAsync("https://localhost:5005/.well-known/openid-configuration");

            // Assert
            response.StatusCode.Should()
                    .Be(expectedResult);
        }


        //[Theory]
        //[InlineData(UserRole.Admin, HttpStatusCode.Created)]
        //[InlineData(UserRole.Alumni, HttpStatusCode.Created)]
        //[InlineData(UserRole.DataOfficer, HttpStatusCode.Created)]
        //[InlineData(UserRole.PrUser, HttpStatusCode.Created)]
        //[InlineData(UserRole.RegisteredUser, HttpStatusCode.Created)]
        //public async Task CreateProject_Returns_Expected_Result_For_All_Roles(
        //    UserRole role,
        //    HttpStatusCode expectedResult)
        //{
        //    // Arrange
        //    await AuthenticateAs((int) role);

        //    Faker<Project> projectToFake = new Faker<Project>()
        //                                   .RuleFor(p => p.UserId, 1)
        //                                   .RuleFor(p => p.Uri, f => f.Internet.Url())
        //                                   .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        //                                   .RuleFor(p => p.Description, f => f.Lorem.Sentences(10))
        //                                   .RuleFor(p => p.ShortDescription, f => f.Lorem.Sentences(1));
        //    Project project = projectToFake.Generate();
        //    project.Created = DateTime.Now.AddDays(-2);
        //    project.Updated = DateTime.Now;

        //    // Act
        //    HttpResponseMessage response = await TestClient.PostAsJsonAsync("project", project);

        //    // Assert
        //    response.StatusCode.Should()
        //            .Be(expectedResult);
        //}


        //[Theory]
        //[InlineData(UserRole.Admin, HttpStatusCode.OK)]
        //[InlineData(UserRole.Alumni, HttpStatusCode.OK)]
        //[InlineData(UserRole.DataOfficer, HttpStatusCode.OK)]
        //[InlineData(UserRole.PrUser, HttpStatusCode.OK)]
        //[InlineData(UserRole.RegisteredUser, HttpStatusCode.OK)]
        //public async Task DeleteProject_Returns_Expected_Result_For_All_Roles(
        //    UserRole role,
        //    HttpStatusCode expectedResult)
        //{
        //    // Arrange
        //    await AuthenticateAs((int) role);

        //    Faker<Project> projectToFake = new Faker<Project>()
        //                                   .RuleFor(p => p.UserId, 1)
        //                                   .RuleFor(p => p.Uri, f => f.Internet.Url())
        //                                   .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        //                                   .RuleFor(p => p.Description, f => f.Lorem.Sentences(10))
        //                                   .RuleFor(p => p.ShortDescription, f => f.Lorem.Sentences(1));
        //    Project project = projectToFake.Generate();
        //    project.Created = DateTime.Now.AddDays(-2);
        //    project.Updated = DateTime.Now;

        //    // Act
        //    HttpResponseMessage addProjectResponse = await TestClient.PostAsJsonAsync("project", project);
        //    var responseContent = await addProjectResponse.Content.ReadAsStringAsync();
        //    var projectToDelete = JsonConvert.DeserializeObject<Project>(responseContent);

        //    HttpResponseMessage response = await TestClient.DeleteAsync($"project/{projectToDelete.Id}");

        //    // Assert
        //    response.StatusCode.Should()
        //            .Be(expectedResult);
        //}


        //[Theory]
        //[InlineData(UserRole.Admin, HttpStatusCode.OK)]
        //[InlineData(UserRole.Alumni, HttpStatusCode.Unauthorized)]
        //[InlineData(UserRole.DataOfficer, HttpStatusCode.Unauthorized)]
        //[InlineData(UserRole.PrUser, HttpStatusCode.Unauthorized)]
        //public async Task DeleteProject_Returns_Expected_Result_For_Admin(
        //    UserRole role,
        //    HttpStatusCode expectedResult)
        //{
        //    // Arrange
        //    await AuthenticateAs((int) UserRole.RegisteredUser);

        //    Faker<Project> projectToFake = new Faker<Project>()
        //                                   .RuleFor(p => p.UserId, 1)
        //                                   .RuleFor(p => p.Uri, f => f.Internet.Url())
        //                                   .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        //                                   .RuleFor(p => p.Description, f => f.Lorem.Sentences(10))
        //                                   .RuleFor(p => p.ShortDescription, f => f.Lorem.Sentences(1));
        //    Project project = projectToFake.Generate();
        //    project.Created = DateTime.Now.AddDays(-2);
        //    project.Updated = DateTime.Now;

        //    // Act
        //    HttpResponseMessage addProjectResponse = await TestClient.PostAsJsonAsync("project", project);
        //    string responseContent = await addProjectResponse.Content.ReadAsStringAsync();


        //    await AuthenticateAs((int) role);
        //    Project projectToDelete = JsonConvert.DeserializeObject<Project>(responseContent);

        //    HttpResponseMessage response = await TestClient.DeleteAsync($"project/{projectToDelete.Id}");

        //    // Assert
        //    response.StatusCode.Should()
        //            .Be(expectedResult);
        //}

        //[Theory]
        //[InlineData(UserRole.RegisteredUser, HttpStatusCode.OK)]
        //[InlineData(UserRole.Admin, HttpStatusCode.OK)]
        //[InlineData(UserRole.Alumni, HttpStatusCode.Unauthorized)]
        //[InlineData(UserRole.DataOfficer, HttpStatusCode.Unauthorized)]
        //[InlineData(UserRole.PrUser, HttpStatusCode.Unauthorized)]
        //public async Task UpdateProject_Returns_Expected_Result_For_Admin(
        //    UserRole role,
        //    HttpStatusCode expectedResult)
        //{
        //    // Arrange
        //    await AuthenticateAs((int) UserRole.RegisteredUser);

        //    Faker<Project> projectToFake = new Faker<Project>()
        //                                   .RuleFor(p => p.UserId, 1)
        //                                   .RuleFor(p => p.Uri, f => f.Internet.Url())
        //                                   .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        //                                   .RuleFor(p => p.Description, f => f.Lorem.Sentences(10))
        //                                   .RuleFor(p => p.ShortDescription, f => f.Lorem.Sentences(1));
        //    Project project = projectToFake.Generate();
        //    project.Created = DateTime.Now.AddDays(-2);
        //    project.Updated = DateTime.Now;

        //    // Act
        //    HttpResponseMessage addProjectResponse = await TestClient.PostAsJsonAsync("project", project);
        //    string responseContent = await addProjectResponse.Content.ReadAsStringAsync();


        //    await AuthenticateAs((int) role);
        //    Project projectToUpdate = JsonConvert.DeserializeObject<Project>(responseContent);

        //    HttpResponseMessage response = await TestClient.PutAsJsonAsync($"project/{projectToUpdate.Id}", projectToUpdate);

        //    // Assert
        //    response.StatusCode.Should()
        //            .Be(expectedResult);
        //}
    }
}
