using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TauCode.WebApi.Client.Exceptions;
using TauCode.WebApi.Client.Tests.App.Dto;

namespace TauCode.WebApi.Client.Tests
{
    [TestFixture]
    public class ServiceClientTests
    {
        #region Fields

        private HttpClient _httpClient;
        private IServiceClient _serviceClient;

        #endregion

        #region Set Up & Tear Down

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var factory = new Factory();
            _httpClient = factory.CreateClient();
            _serviceClient = new ServiceClient(_httpClient);
        }


        [SetUp]
        public void SetUp()
        {
        }

        #endregion

        #region Constructor Tests

        [Test]
        public void Constructor_ValidArgument_RunsOk()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void Constructor_ArgumentIsNull_ThrowsArgumentNullException()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        #endregion

        #region SendAsync Tests

        [Test]
        public async Task SendAsync_ValidArguments_ReturnsValidResponse()
        {
            // Arrange
            var name = "olia";
            var salary = 14.88m;
            var bornAt = DateTime.Parse("1980-01-02T03:04:05");

            // Act
            var message = await _serviceClient.SendAsync(
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

        #endregion

        #region GetAsync Tests

        [Test]
        public void GetAsync_NotFoundGeneric_ThrowHttpServiceClientException()
        {
            // Arrange
            var name = "olia";
            var salary = 14.88m;
            var bornAt = DateTime.Parse("1980-01-02T03:04:05");

            // Act
            var ex = Assert.ThrowsAsync<HttpServiceClientException>(async () =>
                await _serviceClient.GetAsync<PersonDto>(
                    "not-existing-route/{name}/{salary}/{bornAt}",
                    segments: new
                    {
                        name,
                        salary,
                        bornAt,
                    }));

            // Assert
            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(ex.Message, Is.Empty);
        }

        #endregion
    }
}
