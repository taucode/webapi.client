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
        public void GetAsync_NotFoundGeneric_ThrowsHttpServiceClientException()
        {
            // Arrange

            // Act
            var ex = Assert.ThrowsAsync<HttpServiceClientException>(async () =>
                await _serviceClient.GetAsync<PersonDto>(
                    "not-existing-route"));

            // Assert
            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(ex.Message, Is.Empty);
        }

        [Test]
        public void GetAsync_NotFoundGenericWithContent_ThrowsHttpServiceClientExceptionWithMessageEqualToContent()
        {
            // Arrange

            // Act
            var ex = Assert.ThrowsAsync<HttpServiceClientException>(async () =>
                await _serviceClient.GetAsync<PersonDto>("get-returns-notfound"));

            // Assert
            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            var content = ex.Message;
            var contentObject = JsonConvert.DeserializeObject<dynamic>(content);

            Assert.That((string)contentObject.firstProp, Is.EqualTo("first-prop"));
            Assert.That((string)contentObject.secondProp, Is.EqualTo("second-prop"));
        }

        [Test]
        public void GetAsync_NotFoundError_ThrowsNotFoundErrorServiceClientException()
        {
            // Arrange
            var desiredCode = "COULD_NOT_FIND";
            var desiredMessage = "I could not find what you wanted.";

            // Act
            var ex = Assert.ThrowsAsync<NotFoundErrorServiceClientException>(async () =>
                await _serviceClient.GetAsync<PersonDto>(
                    "get-returns-notfound-error",
                    queryParams: new
                    {
                        desiredCode,
                        desiredMessage
                    }));

            // Assert
            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(ex.Code, Is.EqualTo(desiredCode));
            Assert.That(ex.Message, Is.EqualTo(desiredMessage));
        }

        [Test]
        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.RequestTimeout)]
        [TestCase(HttpStatusCode.LengthRequired)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.GatewayTimeout)]
        public void GetAsync_NotSuccessfulStatusCodeWithContent_ThrowsHttpServiceClientException(HttpStatusCode desiredStatusCode)
        {
            // Arrange
            var desiredContent = "Here goes content.";

            // Act
            var ex = Assert.ThrowsAsync<HttpServiceClientException>(async () =>
                await _serviceClient.GetAsync<PersonDto>(
                    "get-returns-desired-generic-statuscode",
                    queryParams: new
                    {
                        desiredStatusCode,
                        desiredContent
                    }));

            // Assert
            Assert.That(ex.StatusCode, Is.EqualTo(desiredStatusCode));
            Assert.That(ex.Message, Is.EqualTo(desiredContent));
        }

        #endregion
    }
}
