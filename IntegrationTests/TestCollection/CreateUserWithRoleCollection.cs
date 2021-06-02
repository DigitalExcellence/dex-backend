using API.Resources;
using IntegrationTests.Test;
using System.Threading.Tasks;

namespace IntegrationTests.TestCollection
{
    public class CreateUserWithRoleCollection : CreateAndCheckCollection
    {
        protected int RoleId;

        public CreateUserWithRoleCollection()
        {
            CreateEndpointResult = typeof(UserResourceResult);
            GetEndpointResult = typeof(UserResourceResult);
            CreateCheckProperties = new string[] { "IdentityId", "Name", "Email" };
            Endpoint = "api/User";
        }

        public override async Task Execute()
        {
            await base.Execute();

            var putTest = new PutTestWithoutBody(this, typeof(UserResourceResult));
            putTest.Endpoint = $"api/Role/setRole?userId={CreatedId}&roleId={RoleId}";
            await putTest.Execute();

            IdentityIdSupplier.Collection.Add(new object[] { CreatedId });
        }
    }
}
