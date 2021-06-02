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

        public virtual async Task Execute()
        {
            await new CreateTest(this, CreateEndpointResult, CreateResource).Execute();
            await new GetTest(this, GetEndpointResult, CreateCheckProperties, CreateResource).Execute();
        }
    }
}
