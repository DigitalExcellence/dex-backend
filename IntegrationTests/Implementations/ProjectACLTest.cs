using API.Resources;
using IntegrationTests.TestCollection;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Implementations
{
    public class ProjectACLTest : CreateAndUpdatedCheckAllCollection
    {
        public ProjectACLTest()
        {
            CreateEndpointResult = typeof(ProjectResultResource);
            GetEndpointResult = typeof(ProjectResourceResult);
            CreateResource = new ProjectResource
            {
                Name = "TOTOTOTOTOTOTOTOMATENPLUKKERS",
                Description = "test",
                ShortDescription = "ttt",
                Uri = "testuri"
            };
            CreateCheckProperties = new string[] { "Name", "ShortDescription" };
            GetAllEndpointResult = typeof(ProjectResultsResource);
            UpdateCheckProperties = new string[] { "Name", "Description", "ShortDescription", "Uri" };
            UpdateResource = new ProjectResource
            {
                Name = "DINGDONG",
                Description = "WOH",
                ShortDescription = "UPD",
                Uri = "UPDATEDURI"
            };
            UpdateEndpointResult = typeof(ProjectResourceResult);

            Endpoint = "api/Project";
        }

        [Fact]
        public override async Task Execute()
        {
            await base.Execute();
        }
    }
}
