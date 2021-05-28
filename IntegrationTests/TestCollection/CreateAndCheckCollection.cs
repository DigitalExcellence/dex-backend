using IntegrationTests.Base;
using IntegrationTests.Test;
using System;
using System.Threading.Tasks;

namespace IntegrationTests.TestCollection
{
    public abstract class CreateAndCheckCollection : BaseACLTestCollection
    {
        protected Type CreateEndpointResult;
        protected Type GetEndpointResult;
        protected dynamic CreateResource;
        protected string[] CreateCheckProperties;

        public virtual async Task Execute()
        {
            await Create();
            await GetCreated();
        }

        public async Task Create()
        {
            await new CreateTest(this, CreateEndpointResult, CreateResource).Execute();
        }

        public async Task GetCreated()
        {
            await new GetTest(this, GetEndpointResult, CreateCheckProperties, CreateResource).Execute();
        }
    }
}
