using API.Resources;
using IntegrationTests.TestCollection;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Implementations.A_Warmup.A0_Users
{
    [Collection("Sequential")]
    public class A0010_CreateDataOfficer : CreateUserWithRoleCollection
    {
        public A0010_CreateDataOfficer()
        {
            RoleId = 3;
            //Endpoint = "api/Project";
            CreateResource = new UserResource()
            {
                Name = "Berta data officer",
                Email = "berta@gmail.com",
                IdentityId = IdentityIdSupplier.DataOfficerIdentityId,
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
