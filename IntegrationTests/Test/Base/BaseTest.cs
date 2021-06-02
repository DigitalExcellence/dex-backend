using IntegrationTests.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Test.Base
{
    public abstract class BaseTest
    {
        public string Endpoint { get; set; }

        protected BaseTestCollection Collection;
        protected Type ExpectedResultType;

        protected BaseTest(BaseTestCollection collection, Type expectedResultType)
        {
            Collection = collection;
            ExpectedResultType = expectedResultType;
            Endpoint = collection.Endpoint;
        }

        public abstract Task Execute();
    }
}
