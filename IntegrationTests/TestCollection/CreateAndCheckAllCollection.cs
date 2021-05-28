using IntegrationTests.Test;
using System;
using System.Threading.Tasks;

namespace IntegrationTests.TestCollection
{
    public abstract class CreateAndCheckAllCollection : CreateAndCheckCollection
    {
        protected Type GetAllEndpointResult;

        public override async Task Execute()
        {
            await base.Execute();
            await GetAllContains();
        }

        public async Task GetAllContains()
        {
            await new GetAllContainsTest(this, GetAllEndpointResult).Execute();
        }
    }
}
