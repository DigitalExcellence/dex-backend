using IntegrationTests.Test;
using System;
using System.Threading.Tasks;

namespace IntegrationTests.TestCollection
{
    public abstract class CreateAndCheckAllCollection : CreateAndCheckCollection
    {
        protected Type GetAllEndpointResult;
        private GetAllContainsTest getAll;

        public CreateAndCheckAllCollection()
        {
            this.getAll = new GetAllContainsTest(this, GetAllEndpointResult);
        }

        public override async Task Execute()
        {
            await base.Execute();
            await getAll.Execute();
        }
    }
}
