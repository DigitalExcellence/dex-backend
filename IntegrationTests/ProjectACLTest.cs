using API.Resources;
using IntegrationTests.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class ProjectACLTest : BaseACLTest
    {
        public ProjectACLTest()
        {
            this.CreateResource = new ProjectResource
            {
                Name = "TOTOTOTOTOTOTOTOMATENPLUKKERS",
                Description = "test",
                ShortDescription = "ttt",
                Uri = "testuri"
            };

            this.UpdateResource = new ProjectResource
            {
                Name = "DINGDONG",
                Description = "WOH",
                ShortDescription = "UPD",
                Uri = "UPDATEDURI"
            };

            this.Endpoint = "api/Project";
            this.ExpectedCreatedResultType = typeof(ProjectResultResource);
            this.ExpectedGetAllCreatedResultType = typeof(ProjectResultsResource);
            this.ExpectedGetResultType = typeof(ProjectResourceResult);
            this.ExpectedUpdatedResultType = typeof(ProjectResourceResult);

            this.CheckCreatedProperties = new string[] { "Name", "ShortDescription" };
            this.CheckUpdatedProperties = new string[] { "Name", "Description", "ShortDescription", "Uri" };
        }


    }
}
