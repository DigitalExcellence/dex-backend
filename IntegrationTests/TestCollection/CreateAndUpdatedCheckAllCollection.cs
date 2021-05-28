using IntegrationTests.Test;
using System;
using System.Threading.Tasks;

namespace IntegrationTests.TestCollection
{
    public abstract class CreateAndUpdatedCheckAllCollection : CreateAndCheckAllCollection
    {
        protected string[] UpdateCheckProperties;
        protected dynamic UpdateResource;
        protected Type UpdateEndpointResult;

        public override async Task Execute()
        {
            await base.Execute();
            await Update();
            await GetUpdated();
        }

        public async Task Update()
        {
            await new UpdateTest(this, UpdateEndpointResult, UpdateResource).Execute();
        }

        public async Task GetUpdated()
        {
            await new GetTest(this, GetEndpointResult, UpdateCheckProperties, UpdateResource).Execute();
        }
    }
}
