using IntegrationTests.Test;
using System;
using System.Threading.Tasks;

namespace IntegrationTests.TestCollection
{
    public abstract class CreateUpdateAndCheckAllCollection : CreateAndCheckAllCollection
    {
        protected string[] UpdateCheckProperties;
        protected dynamic UpdateResource;
        protected Type UpdateEndpointResult;

        private UpdateTest update;
        private GetTest getUpdated;

        public CreateUpdateAndCheckAllCollection()
        {
            this.update = new UpdateTest(this, UpdateEndpointResult, UpdateResource);
            this.getUpdated = new GetTest(this, GetEndpointResult, UpdateCheckProperties, UpdateResource);
        }

        public override async Task Execute()
        {
            await base.Execute();
            await update.Execute();
            await getUpdated.Execute();
        }
    }
}
