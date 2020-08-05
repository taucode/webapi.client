using NUnit.Framework;
using System.Net.Http;

namespace TauCode.WebApi.Client.Tests
{
    [TestFixture]
    public abstract class ServiceClientTestBase
    {
        protected HttpClient HttpClient;
        protected IServiceClient ServiceClient;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var factory = new Factory();
            this.HttpClient = factory.CreateClient();
            this.ServiceClient = new ServiceClient(HttpClient);
        }

        [OneTimeTearDown]
        public void SetUp()
        {
            this.HttpClient.Dispose();
        }
    }
}
