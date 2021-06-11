using IntegrationTests.Settings;
using System;
using System.Threading.Tasks;

namespace IntegrationTests.Test.Base
{
    public abstract class BaseTest
    {
        public string Endpoint { get; set; }

        protected RequestConfig RequestConfig;
        protected Type ExpectedResultType;

        protected BaseTest(RequestConfig requestConfig, Type expectedResultType)
        {
            ExpectedResultType = expectedResultType;
            RequestConfig = requestConfig;
        }

        public abstract Task Execute();
    }
}
