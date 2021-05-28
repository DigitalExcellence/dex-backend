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
        protected object UpdateResource;
        protected string[] CheckCreatedProperties;
        protected string[] CheckUpdatedProperties;
        protected Type ExpectedCreatedResultType;
        protected Type ExpectedUpdatedResultType;
        protected Type ExpectedGetResultType;
        protected Type ExpectedGetAllCreatedResultType;
        protected bool DoGetAll = true;

        [Fact]
        public virtual async Task ACLTest()
        {
            int id = await Create();
            if(DoGetAll)
            {
                await GetAllCreated(id);
            }

            await GetCreated(id);
            await Update(id);
            await GetUpdated(id);
        }

        private async Task<int> Create()
        {
            // Arrange
            var client = Connection.Client;
            var content = HttpHelper.GetHttpContent(CreateResource);

            // Act
            var response = await client.PostAsync(Endpoint, content);

            // Assert
            dynamic responseObj = await HttpHelper.GetFromResponse(response, ExpectedCreatedResultType);
            response.EnsureSuccessStatusCode();
            return (int) responseObj.Id;
        }

        private async Task GetAllCreated(int id)
        {
            // Arrange
            var client = Connection.Client;

            // Act
            var response = await client.GetAsync(Endpoint);

            // Assert
            dynamic responseObj = await HttpHelper.GetFromResponse(response, ExpectedGetAllCreatedResultType);
            dynamic[] results = (dynamic[]) responseObj.Results;
            int count = results.Where(d => d.Id == id).Count();

            response.EnsureSuccessStatusCode();
            Assert.True(count >= 1);
        }

        private async Task GetCreated(int id)
        {
            // Arrange
            var client = Connection.Client;

            // Act
            var response = await client.GetAsync(Endpoint + $"/{id}");

            // Assert
            dynamic responseObj = await HttpHelper.GetFromResponse(response, ExpectedGetResultType);
            response.EnsureSuccessStatusCode();
            Assert.True((int) responseObj.Id == id);

            foreach(string property in CheckCreatedProperties)
            {
                Assert.Equal(GetProperty(CreateResource, property), GetProperty(responseObj, property));
            }
        }

        private async Task Update(int id)
        {
            // Arrange
            var client = Connection.Client;
            var content = HttpHelper.GetHttpContent(UpdateResource);

            // Act
            var response = await client.PutAsync(Endpoint + $"/{id}", content);

            // Assert
            dynamic responseObj = await HttpHelper.GetFromResponse(response, ExpectedUpdatedResultType);
            response.EnsureSuccessStatusCode();
            Assert.Equal<int>(responseObj.Id, id);
        }

        private async Task GetUpdated(int id)
        {
            // Arrange
            var client = Connection.Client;

            // Act
            var response = await client.GetAsync(Endpoint + $"/{id}");

            // Assert
            dynamic responseObj = await HttpHelper.GetFromResponse(response, ExpectedGetResultType);
            response.EnsureSuccessStatusCode();
            Assert.True((int) responseObj.Id == id);

            foreach(string property in CheckUpdatedProperties){
                Assert.Equal(GetProperty(UpdateResource, property), GetProperty(responseObj, property));
            }
        }

        private dynamic GetProperty(dynamic obj, string property)
        {
            return obj.GetType().GetProperty(property).GetValue(obj);
        }
    }
}
