using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using TauCode.WebApi.Client.Tests.App.Dto;

namespace TauCode.WebApi.Client.Tests.App.Controllers
{
    [ApiController]
    public class DeleteController : ControllerBase
    {
        [HttpDelete]
        [Route("api/delete-by-code/{code}")]
        public IActionResult DeleteByCode([FromRoute]string code, string name)
        {
            return this.Ok(new CodeDto
            {
                Code = code,
                Name = name,
            });
        }

        [HttpDelete]
        [Route("api/delete-by-id/{needReturnedId}")]
        public IActionResult DeleteById(
            [FromRoute]bool needReturnedId,
            [FromQuery]string expectedReturnedId)
        {
            if (needReturnedId)
            {
                this.Response.Headers.Add(DtoHelper.DeletedInstanceIdHeaderName, expectedReturnedId);
            }

            return this.NoContent();
        }

        [HttpDelete]
        [Route("api/delete-returns-notfound")]
        public IActionResult DeleteReturnsNotFound()
        {
            return this.NotFound(new
            {
                firstProp = "first-prop",
                secondProp = "second-prop"
            });
        }

        [HttpDelete]
        [Route("api/delete-returns-desired-generic-statuscode")]
        public IActionResult DeleteReturnsDesiredGenericStatusCode(
            [FromQuery] HttpStatusCode desiredStatusCode,
            [FromQuery] string desiredContent)
        {
            return new ContentResult
            {
                StatusCode = (int)desiredStatusCode,
                Content = desiredContent,
            };
        }

        [HttpDelete]
        [Route("api/delete-returns-notfound-error")]
        public IActionResult DeleteReturnsNotFoundError([FromQuery]string desiredCode, [FromQuery]string desiredMessage)
        {
            this.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.ErrorPayloadType);

            return this.NotFound(new ErrorDto
            {
                Code = desiredCode,
                Message = desiredMessage,
            });
        }

        [HttpDelete]
        [Route("api/delete-returns-badrequest-error")]
        public IActionResult DeleteReturnsBadRequestError([FromQuery]string desiredCode, [FromQuery]string desiredMessage)
        {
            this.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.ErrorPayloadType);

            return this.BadRequest(new ErrorDto
            {
                Code = desiredCode,
                Message = desiredMessage,
            });
        }

        [HttpDelete]
        [Route("api/delete-returns-error")]
        public IActionResult DeleteReturnsError(
            [FromQuery]HttpStatusCode desiredStatusCode,
            [FromQuery]string desiredCode,
            [FromQuery]string desiredMessage)
        {
            this.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.ErrorPayloadType);

            var error = new ErrorDto
            {
                Code = desiredCode,
                Message = desiredMessage,
            };

            var json = JsonConvert.SerializeObject(error);

            return new ContentResult
            {
                StatusCode = (int)desiredStatusCode,
                Content = json,
                ContentType = "application/json",
            };
        }

        [HttpDelete]
        [Route("api/delete-returns-validation-error")]
        public IActionResult DeleteReturnsValidationError(
            [FromQuery]string desiredCode,
            [FromQuery]string desiredMessage)
        {
            this.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.ValidationErrorPayloadType);

            var validationError = new ValidationErrorDto
            {
                Code = desiredCode,
                Message = desiredMessage,
                Failures = new Dictionary<string, ValidationFailureDto>
                {
                    {
                        "name",
                        new ValidationFailureDto
                        {
                            Code = "NameValidator",
                            Message = "Name is bad."
                        }
                    },
                    {
                        "salary",
                        new ValidationFailureDto
                        {
                            Code = "SalaryValidator",
                            Message = "Salary is low.",
                        }
                    }
                },
            };

            var json = JsonConvert.SerializeObject(validationError);

            return new ContentResult
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Content = json,
                ContentType = "application/json",
            };
        }

        [HttpDelete]
        [Route("api/delete-returns-bad-json")]
        public IActionResult DeleteReturnsBadJson()
        {
            var badJson = "<bad_json>";

            return new ContentResult
            {
                StatusCode = (int)HttpStatusCode.OK,
                Content = badJson,
                ContentType = "application/json",
            };
        }
    }
}
