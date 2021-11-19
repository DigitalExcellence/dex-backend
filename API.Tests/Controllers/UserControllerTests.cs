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
using System;

namespace API.Tests.Controllers
{
    public class UserControllerTests : BaseTests
    {
        [Theory]
        [InlineData(UserRole.RegisteredUser)]
        [InlineData(UserRole.Admin)]
        [InlineData(UserRole.DataOfficer)]
        [InlineData(UserRole.PrUser)]
        [InlineData(UserRole.Alumni)]
        public async Task GetCurrentUser_Returns_Correct_User(UserRole role)
        {
            // Arrange
            await AuthenticateAs(role);

            // Act
            HttpResponseMessage response = await TestClient.GetAsync("user");
            int responseUserIdentityId = Convert.ToInt32(JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync()).IdentityId);

            // Assert
            responseUserIdentityId.Should().Be((int)role);
        }
    }
}
