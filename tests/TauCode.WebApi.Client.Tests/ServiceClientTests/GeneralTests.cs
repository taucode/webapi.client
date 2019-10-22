using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TauCode.WebApi.Client.Tests.App.Dto;

namespace TauCode.WebApi.Client.Tests.ServiceClientTests
{
    [TestFixture]
    public class GeneralTests : ServiceClientTestBase
    {
        [Test]
        public void Constructor_ValidArgument_RunsOk()
        {
            // Arrange

            // Act
            var serviceClient = new ServiceClient(this.HttpClient);

            // Assert
            Assert.That(serviceClient.HttpClient, Is.SameAs(this.HttpClient));
        }

        [Test]
        public void Constructor_ArgumentIsNull_ThrowsArgumentNullException()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => new ServiceClient(null));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("httpClient"));
        }

        [Test]
        public async Task SendAsync_ValidArguments_ReturnsValidResponse()
        {
            // Arrange
            var name = "olia";
            var salary = 14.88m;
            var bornAt = DateTime.Parse("1980-01-02T03:04:05");

            // Act
            var message = await ServiceClient.SendAsync(
                HttpMethod.Get,
                "get-from-route/{name}/{salary}/{bornAt}",
                segments: new
                {
                    name,
                    salary,
                    bornAt,
                });

            // Assert
            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var json = await message.Content.ReadAsStringAsync();
            var person = JsonConvert.DeserializeObject<PersonDto>(json);

            Assert.That(person.Name, Is.EqualTo(name));
            Assert.That(person.Salary, Is.EqualTo(salary));
            Assert.That(person.BornAt, Is.EqualTo(bornAt));
        }
    }
}
