using NUnit.Framework;

namespace TauCode.WebApi.Client.Tests;

[TestFixture]
public abstract class ServiceClientTestBase
{
    protected HttpClient HttpClient;
    protected IServiceClient ServiceClient;
    private Factory _factory;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new Factory();
        this.HttpClient = _factory.CreateClient();
        this.ServiceClient = new ServiceClient(HttpClient);
    }

    [OneTimeTearDown]
    public void SetUp()
    {
        this.HttpClient.Dispose();
        _factory.Dispose();
    }
}