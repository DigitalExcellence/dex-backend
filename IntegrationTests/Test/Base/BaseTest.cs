using IntegrationTests.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Test.Base
{
    public abstract class BaseTest
    {
        protected BaseACLTestCollection Collection;
        protected Type ExpectedResultType;

        protected BaseTest(BaseACLTestCollection collection, Type expectedResultType)
        {
            Collection = collection;
            ExpectedResultType = expectedResultType;
        }

        public abstract Task Execute();
    }
}
