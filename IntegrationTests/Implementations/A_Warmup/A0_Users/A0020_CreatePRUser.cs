using API.Resources;
using IntegrationTests.TestCollection;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Implementations.A_Warmup.A0_Users
{
    [Collection("Sequential")]
    public class A0020_CreatePRUser : CreateUserWithRoleCollection
    {
        public A0020_CreatePRUser()
        {
            RoleId = 2;
            CreateResource = new UserResource()
            {
                Name = "BerendPR",
                Email = "berend@gmail.com",
                IdentityId = IdentityIdSupplier.PRUserIdentityId,
                InstitutionId = 1
            };
        }

        [Fact]
        public override async Task Execute()
        {
            await base.Execute();
        }
    }
}
