using API.Tests.Base;
using FluentAssertions;
using Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using API.Tests.Enums;
using API.Tests.Helpers;
using API.Resources;
using API.InputOutput.Tag;
using System.Collections.Generic;

namespace API.Tests.Controllers
{
    public class ProjectControllerTests : BaseTests
    {
        [Theory]
        [InlineData(UserRole.Alumni,HttpStatusCode.OK)]
        [InlineData(UserRole.Admin, HttpStatusCode.OK)]
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

        [Theory]
        [InlineData(UserRole.RegisteredUser, HttpStatusCode.OK)]
        [InlineData(UserRole.Admin, HttpStatusCode.OK)]
        [InlineData(UserRole.DataOfficer, HttpStatusCode.OK)]
        [InlineData(UserRole.PrUser, HttpStatusCode.OK)]
        public async Task UpdateProject_Returns_Expected_Result_For_Admin(UserRole role, HttpStatusCode expectedResult)

        {
            // Arrange
            await AuthenticateAs(role);

            HttpResponseMessage addProjectResponse = await TestClient.PostAsJsonAsync("project", SeedUtility.RandomProject());
            string responseContent = await addProjectResponse.Content.ReadAsStringAsync();
            Project projectToUpdate = JsonConvert.DeserializeObject<Project>(responseContent);

            // Act
            HttpResponseMessage response = await TestClient.PutAsJsonAsync($"project/{projectToUpdate.Id}", projectToUpdate);

            // Assert
            response.StatusCode.Should().Be(expectedResult);
        }


        [Theory]
        [InlineData(UserRole.Admin, HttpStatusCode.OK)]
        [InlineData(UserRole.DataOfficer, HttpStatusCode.OK)]
        [InlineData(UserRole.PrUser, HttpStatusCode.OK)]
        [InlineData(UserRole.RegisteredUser, HttpStatusCode.OK)]
        public async Task CategorizeProject_Returns_Expected_Result_For_All_Roles(UserRole role, HttpStatusCode expectedResult)
        {
            // Arrange
            await AuthenticateAs(role);
            HttpResponseMessage postProjectResponse = await TestClient.PostAsJsonAsync("project", SeedUtility.RandomProject());
            string responseContent = await postProjectResponse.Content.ReadAsStringAsync();

            Project projectToCategorize = JsonConvert.DeserializeObject<Project>(responseContent);

            // Act
            HttpResponseMessage response = await TestClient.PostAsync($"project/category/{projectToCategorize.Id}/2", null);

            // Assert
            response.StatusCode.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(UserRole.Admin, HttpStatusCode.OK)]
        [InlineData(UserRole.DataOfficer, HttpStatusCode.OK)]
        [InlineData(UserRole.PrUser, HttpStatusCode.OK)]
        [InlineData(UserRole.RegisteredUser, HttpStatusCode.OK)]
        public async Task Update_Tag_Returns_Expected_Result_For_All_Roles(UserRole role, HttpStatusCode expectedResult)
        {
            // Arrange
            await AuthenticateAs(role);
            ProjectInput projectInput = SeedUtility.RandomProjectInput();
            projectInput.Tags = new List<TagInput>() {
                new TagInput() { Name = "java" }
            };
            HttpResponseMessage postProjectResponse = await TestClient.PostAsJsonAsync("project", projectInput);
            string responseContent = await postProjectResponse.Content.ReadAsStringAsync();

            int projectId = JsonConvert.DeserializeObject<Project>(responseContent).Id;
            
            ProjectInput project = JsonConvert.DeserializeObject<ProjectInput>(responseContent);

            project.Tags = new List<TagInput>() {
                new TagInput() { Name = "csharp" },
                new TagInput() { Name = "java" }
            };

            // Act
            HttpResponseMessage response = await TestClient.PutAsJsonAsync("project/" + projectId, projectInput);

            // Assert
            response.StatusCode.Should().Be(expectedResult);
        }


        [Theory]
        [InlineData(UserRole.Alumni, HttpStatusCode.OK)]
        [InlineData(UserRole.Admin, HttpStatusCode.OK)]
        [InlineData(UserRole.DataOfficer, HttpStatusCode.OK)]
        [InlineData(UserRole.PrUser, HttpStatusCode.OK)]
        [InlineData(UserRole.RegisteredUser, HttpStatusCode.OK)]
        public async Task GetProjects_By_Multiple_Ids_Returns_OK_Result_For_All_Roles(UserRole role, HttpStatusCode expectedResult)
        {
            // Arrange
            await AuthenticateAs(role);

            int[] existingProjectIds = new int[] { 1,2,3,10,15,20,30 };

            // Act
            HttpResponseMessage response = await TestClient.PostAsJsonAsync("project/multiple", existingProjectIds);

            // Assert
            response.StatusCode.Should().Be(expectedResult);
        }


        [Theory]
        [InlineData(UserRole.Alumni, 7)]
        [InlineData(UserRole.Admin, 7)]
        [InlineData(UserRole.DataOfficer, 7)]
        [InlineData(UserRole.PrUser, 7)]
        [InlineData(UserRole.RegisteredUser, 7)]
        public async Task GetProjects_By_Multiple_Ids_Returns_Correct_Count_Result_For_All_Roles(UserRole role, int expectedCount)
        {
            // Arrange
            await AuthenticateAs(role);

            int[] existingProjectIds = new int[] { 1, 2, 3, 10, 15, 20, 30 };

            // Act
            HttpResponseMessage response = await TestClient.PostAsJsonAsync("Project/multiple", existingProjectIds);
            string result = await response.Content.ReadAsStringAsync();

            // Assert
            if(response.IsSuccessStatusCode)
            {
                List<ProjectOutput> projectOutputs = JsonConvert.DeserializeObject<List<ProjectOutput>>(result);
                Assert.Equal(expectedCount, projectOutputs.Count);
            }

            Assert.True(false);
        }
    }
}
