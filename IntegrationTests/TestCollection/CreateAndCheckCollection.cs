using IntegrationTests.Base;
using IntegrationTests.Test;
using System;
using System.Threading.Tasks;

namespace IntegrationTests.TestCollection
{
    /// <summary>
    /// Collection with Create & Get Created tests
    /// 
    /// </summary>
    public abstract class CreateAndCheckCollection : BaseTestCollection
    {
        protected Type CreateEndpointResult;
        protected Type GetEndpointResult;
        protected dynamic CreateResource;
        protected string[] CreateCheckProperties;

        private CreateTest create;
        private GetTest getCreated;

        public CreateAndCheckCollection()
        {
            this.create = new CreateTest(this, CreateEndpointResult, CreateResource);
            this.getCreated = new GetTest(this, GetEndpointResult, CreateCheckProperties, CreateResource);
        }


        public override async Task Execute()
        {
            await create.Execute();
            await getCreated.Execute();
        }
    }
}
