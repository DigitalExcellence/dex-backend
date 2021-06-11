using API.Resources;
using IntegrationTests.Data.DataSource;
using Models;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Implementations.B_Tests
{
    [Collection("Sequential")]
    public class B010_ProjectACLTest
    {
        public B010_ProjectACLTest()
        {

        }

        [Theory]
        [ClassData(typeof(ProjectDataSourceAttribute))]
        //[ClassData(typeof(IdentityIdSupplier))]
        public void Test([ProjectDataSource] Project project)
        {
            var id = project.Id;
            //IdentityId = identityId;
            //await base.Execute();
        }
    }
}
