using API.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Base
{
    public abstract class BaseACLTest : BaseTest
    {
        protected string Endpoint;
        protected object CreateResource;
        protected Type ExpectedCreatedResultType;
        protected Type ExpectedGetAllCreatedResultType;

        [Fact]
        public virtual async Task ACLTest()
        {
            int id = await ACLTask_Create();
            await ACLTask_GetAllCreated(id);
        }

        private async Task<int> ACLTask_Create()
        {
            var client = Connection.Client;
            var content = HttpHelper.GetHttpContent(CreateResource);
            var response = await client.PostAsync(Endpoint, content);

            response.EnsureSuccessStatusCode();

            var responseObj = await HttpHelper.GetFromResponse(response, ExpectedCreatedResultType);

            Type t = responseObj.GetType();
            PropertyInfo[] properties = t.GetProperties();
            return (int) properties.First(p => p.Name == "Id").GetValue(responseObj);
        }

        private async Task ACLTask_GetAllCreated(int id)
        {
            var client = Connection.Client;
            var response = await client.GetAsync(Endpoint);

            response.EnsureSuccessStatusCode();

            var responseObj = await HttpHelper.GetFromResponse(response, ExpectedGetAllCreatedResultType);

            dynamic[] results = (dynamic[]) responseObj.GetType().GetProperty("Results").GetValue(responseObj);
            int count = results.Where(d => d.Id == id).Count();

            Assert.True(count >= 1);
        }
    }
}
