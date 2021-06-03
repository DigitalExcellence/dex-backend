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

        private UpdateTest update;
        private GetTest getUpdated;

        public CreateAndUpdatedCheckAllCollection()
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
