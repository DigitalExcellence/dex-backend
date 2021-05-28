using IntegrationTests.Test.Base;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Base
{
    public abstract class BaseACLTestCollection : BaseTestCollection
    {
        public int CreatedId;
        public string Endpoint;

        [Fact]
        public virtual async Task ACLTest()
        {
            foreach(BaseTest test in Tests){
                await test.Execute();
            }
        }
    }
}
