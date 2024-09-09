
using Microsoft.AspNetCore.Mvc.Testing;

namespace TestProject.text
{
    public class UnitTest1: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _applicationFactory;

        public UnitTest1()
        {
            var factory= new WebApplicationFactory<Program>();
            _applicationFactory=factory;    
        }

        [Fact]
        public async void HomeControllerTest()
        {
            //areange
            var client = _applicationFactory.CreateClient();

            //act

            var response = await client.GetAsync("/Customer/Home/Index");
            int code= (int)response.StatusCode;

            //asert

            Assert.Equal(200, code);
        }
    }
}