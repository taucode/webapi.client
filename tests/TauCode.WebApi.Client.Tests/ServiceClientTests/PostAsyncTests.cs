using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;
using TauCode.WebApi.Client.Exceptions;
using TauCode.WebApi.Client.Tests.App.Dto;

namespace TauCode.WebApi.Client.Tests.ServiceClientTests
{
    [TestFixture]
    public class PostAsyncTests : ServiceClientTestBase
    {
        [Test]
        public async Task PostAsync_ValidArguments_ReturnsExpectedResponse()
        {
            // Arrange
            var name = "olia";
            var salary = 14.88m;
            var bornAt = DateTime.Parse("1980-01-02T03:04:05");

            var prefix = "hello";
            var a = 10;
            var b = "the";

            // Act
            var reversePerson = await this.ServiceClient.PostAsync<PersonDto>(
                "post-reverse-person/{prefix}",
                new
                {
                    prefix
                },
                new
                {
                    a,
                    b,
                },
                new PersonDto
                {
                    Name = name,
                    Salary = salary,
                    BornAt = bornAt,
                });

            // Assert
            Assert.That(reversePerson.Name, Is.EqualTo("ailo"));
            Assert.That(reversePerson.Salary, Is.EqualTo(-14.88m));
            Assert.That(reversePerson.BornAt, Is.EqualTo(DateTime.Parse("1970-01-02T03:04:05")));
            Assert.That(reversePerson.Info, Is.EqualTo("prefix=hello;a=10;b=the;"));
        }

        [Test]
        public void PostAsync_NotFoundGeneric_ThrowsHttpServiceClientException()
        {
            // Arrange

            // Act
            var ex = Assert.ThrowsAsync<HttpServiceClientException>(async () =>
                await this.ServiceClient.PostAsync(
                    "not-existing-route"));

            // Assert
            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(ex.Message, Is.Empty);
        }

        [Test]
        public void PostAsync_NotFoundGenericWithContent_ThrowsHttpServiceClientExceptionWithMessageEqualToContent()
        {
            // Arrange

            // Act
            var ex = Assert.ThrowsAsync<HttpServiceClientException>(async () =>
                await this.ServiceClient.PostAsync("post-returns-notfound"));

            // Assert
            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            var content = ex.Message;
            var contentObject = JsonConvert.DeserializeObject<dynamic>(content);

            Assert.That((string)contentObject.firstProp, Is.EqualTo("first-prop"));
            Assert.That((string)contentObject.secondProp, Is.EqualTo("second-prop"));
        }

        [Test]
        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.RequestTimeout)]
        [TestCase(HttpStatusCode.LengthRequired)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.GatewayTimeout)]
        public void PostAsync_NotSuccessfulStatusCodeWithContent_ThrowsHttpServiceClientException(HttpStatusCode desiredStatusCode)
        {
            // Arrange
            var desiredContent = "Here goes content.";

            // Act
            var ex = Assert.ThrowsAsync<HttpServiceClientException>(async () =>
                await this.ServiceClient.PostAsync(
                    "post-returns-desired-generic-statuscode",
                    queryParams: new
                    {
                        desiredStatusCode,
                        desiredContent
                    }));

            // Assert
            Assert.That(ex.StatusCode, Is.EqualTo(desiredStatusCode));
            Assert.That(ex.Message, Is.EqualTo(desiredContent));
        }

        [Test]
        public void PostAsync_BadRequestError_ThrowsBadRequestErrorServiceClientException()
        {
            // Arrange
            var desiredCode = "BAD_REQUEST";
            var desiredMessage = "Wrong request data.";

            // Act
            var ex = Assert.ThrowsAsync<BadRequestErrorServiceClientException>(async () =>
                await this.ServiceClient.PostAsync(
                    "post-returns-badrequest-error",
                    queryParams: new
                    {
                        desiredCode,
                        desiredMessage
                    }));

            // Assert
            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(ex.Code, Is.EqualTo(desiredCode));
            Assert.That(ex.Message, Is.EqualTo(desiredMessage));
        }

        [Test]
        public void PostAsync_ConflictError_ThrowsConflictErrorServiceClientException()
        {
            // Arrange
            var desiredStatusCode = HttpStatusCode.Conflict;
            var desiredCode = "CONFLICT_ERROR";
            var desiredMessage = "Invalid operation.";

            // Act
            var ex = Assert.ThrowsAsync<ConflictErrorServiceClientException>(async () =>
                await this.ServiceClient.PostAsync(
                    "post-returns-error",
                    queryParams: new
                    {
                        desiredStatusCode,
                        desiredCode,
                        desiredMessage
                    }));

            // Assert
            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            Assert.That(ex.Code, Is.EqualTo(desiredCode));
            Assert.That(ex.Message, Is.EqualTo(desiredMessage));
        }

        [Test]
        public void PostAsync_ForbiddenError_ThrowsForbiddenErrorServiceClientException()
        {
            // Arrange
            var desiredStatusCode = HttpStatusCode.Forbidden;
            var desiredCode = "ACCESS_VIOLATION";
            var desiredMessage = "You're not authorized.";

            // Act
            var ex = Assert.ThrowsAsync<ForbiddenErrorServiceClientException>(async () =>
                await this.ServiceClient.PostAsync(
                    "post-returns-error",
                    queryParams: new
                    {
                        desiredStatusCode,
                        desiredCode,
                        desiredMessage
                    }));

            // Assert
            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            Assert.That(ex.Code, Is.EqualTo(desiredCode));
            Assert.That(ex.Message, Is.EqualTo(desiredMessage));
        }

        [Test]
        public void PostAsync_NotFoundError_ThrowsNotFoundErrorServiceClientException()
        {
            // Arrange
            var desiredStatusCode = HttpStatusCode.NotFound;
            var desiredCode = "NOT_FOUND";
            var desiredMessage = "Resource not found.";

            // Act
            var ex = Assert.ThrowsAsync<NotFoundErrorServiceClientException>(async () =>
                await this.ServiceClient.PostAsync(
                    "post-returns-error",
                    queryParams: new
                    {
                        desiredStatusCode,
                        desiredCode,
                        desiredMessage
                    }));

            // Assert
            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(ex.Code, Is.EqualTo(desiredCode));
            Assert.That(ex.Message, Is.EqualTo(desiredMessage));
        }

        [Test]
        public void PostAsync_ValidationError_ThrowsValidationErrorServiceClientException()
        {
            // Arrange
            var desiredCode = "VALIDATION_ERROR";
            var desiredMessage = "Request is wrong.";

            // Act
            var ex = Assert.ThrowsAsync<ValidationErrorServiceClientException>(async () =>
                await this.ServiceClient.PostAsync(
                    "post-returns-validation-error",
                    queryParams: new
                    {
                        desiredCode,
                        desiredMessage
                    }));

            // Assert
            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(ex.Code, Is.EqualTo(desiredCode));
            Assert.That(ex.Message, Is.EqualTo(desiredMessage));

            var failure = ex.Failures["name"];
            Assert.That(failure.Code, Is.EqualTo("NameValidator"));
            Assert.That(failure.Message, Is.EqualTo("Name is bad."));

            failure = ex.Failures["salary"];
            Assert.That(failure.Code, Is.EqualTo("SalaryValidator"));
            Assert.That(failure.Message, Is.EqualTo("Salary is low."));
        }
    }
}
