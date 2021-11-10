using _15_API.Tests.Base;
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
using _15_API.Tests.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace _15_API.Tests.Controllers
{
    public class ProjectControllerTests : BaseTests
    {
        [Theory]
        [InlineData(UserRole.Admin, HttpStatusCode.Created)]
        [InlineData(UserRole.Alumni, HttpStatusCode.Forbidden)]
        [InlineData(UserRole.DataOfficer, HttpStatusCode.Created)]
        [InlineData(UserRole.PrUser, HttpStatusCode.Created)]
        public async Task CreateProject_Returns_Expected_Result_For_All_Roles(
            UserRole role,
            HttpStatusCode expectedResult)
        {
            // Arrange
            await AuthenticateAs((int) role);

            Faker<Project> projectToFake = new Faker<Project>()
                                           .RuleFor(p => p.UserId, 1)
                                           .RuleFor(p => p.Uri, f => f.Internet.Url())
                                           .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                                           .RuleFor(p => p.Description, f => f.Lorem.Sentences(10))
                                           .RuleFor(p => p.ShortDescription, f => f.Lorem.Sentences(1));
            Project project = projectToFake.Generate();
            project.Created = DateTime.Now.AddDays(-2);
            project.Updated = DateTime.Now;

            // Act
            HttpResponseMessage response = await TestClient.PostAsJsonAsync("project", project);

            // Assert
            response.StatusCode.Should()
                    .Be(expectedResult);
        }
    }
}
