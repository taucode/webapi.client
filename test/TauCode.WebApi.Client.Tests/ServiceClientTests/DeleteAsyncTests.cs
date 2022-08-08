using NUnit.Framework;
using TauCode.WebApi.Client.Tests.AppHost.Dto;

namespace TauCode.WebApi.Client.Tests.ServiceClientTests;

[TestFixture]
public class DeleteAsyncTests : ServiceClientTestBase
{
    [Test]
    public async Task Delete_ValidArguments_ReturnsExpectedResponse()
    {
        // Arrange
        var code = "USD";
        var name = "U.S.Dollar";

        // Act
        var result = await this.ServiceClient.DeleteAsync<CodeDto>(
            "api/delete-by-code/{code}",
            new
            {
                code
            },
            new
            {
                name
            });

        // Assert
        Assert.That(result.Code, Is.EqualTo(code));
        Assert.That(result.Name, Is.EqualTo(name));
    }
}