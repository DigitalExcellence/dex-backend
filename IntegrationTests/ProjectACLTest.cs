using API.Resources;
using IntegrationTests.Base;
using IntegrationTests.Test;
using IntegrationTests.Test.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class ProjectACLTest : BaseACLTestCollection
    {
        public ProjectACLTest()
        {
            var createResource = new ProjectResource
            {
                Name = "TOTOTOTOTOTOTOTOMATENPLUKKERS",
                Description = "test",
                ShortDescription = "ttt",
                Uri = "testuri"
            };

            var updateResource = new ProjectResource
            {
                Name = "DINGDONG",
                Description = "WOH",
                ShortDescription = "UPD",
                Uri = "UPDATEDURI"
            };

            var createTest = new CreateTest(this, typeof(ProjectResultResource), createResource);
            var getAllContainsTest = new GetAllContainsTest(this, typeof(ProjectResultsResource));
            var getCreatedTest = new GetTest(this, typeof(ProjectResourceResult), new string[] { "Name", "ShortDescription" }, createResource);
            var updateTest = new UpdateTest(this, typeof(ProjectResultResource), updateResource);
            var getUpdatedTest = new GetTest(this, typeof(ProjectResourceResult), new string[] { "Name", "Description", "ShortDescription", "Uri" }, updateResource);

            Endpoint = "api/Project";
            Tests = new BaseTest[] { createTest, getAllContainsTest, getCreatedTest, updateTest, getUpdatedTest };
        }
    }
}
