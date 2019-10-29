using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TauCode.WebApi.Client.Tests.App.Dto;

namespace TauCode.WebApi.Client.Tests.App.Controllers
{
    [ApiController]
    public class PutController : ControllerBase
    {
        [HttpPut]
        [Route("put-reverse-person/{prefix}")]
        public IActionResult PutFromRoute(
            [FromBody]PersonDto person,
            [FromRoute]string prefix,
            [FromQuery]string a,
            [FromQuery]string b)
        {
            var sb = new StringBuilder();
            sb.Append($"prefix={prefix};");
            sb.Append($"a={a};");
            sb.Append($"b={b};");

            var reversedPerson = new PersonDto
            {
                Name = new string(person.Name.Reverse().ToArray()),
                Salary = -person.Salary,
                BornAt = person.BornAt.AddYears(-10),
                Info = sb.ToString(),
            };

            return this.Ok(reversedPerson);
        }

        [HttpPut]
        [Route("put-returns-notfound")]
        public IActionResult PutReturnsNotFound()
        {
            return this.NotFound(new
            {
                firstProp = "first-prop",
                secondProp = "second-prop"
            });
        }

        [HttpPut]
        [Route("put-returns-desired-generic-statuscode")]
        public IActionResult PutReturnsDesiredGenericStatusCode(
            [FromQuery] HttpStatusCode desiredStatusCode,
            [FromQuery] string desiredContent)
        {
            return new ContentResult
            {
                StatusCode = (int)desiredStatusCode,
                Content = desiredContent,
            };
        }

        [HttpPut]
        [Route("put-returns-notfound-error")]
        public IActionResult PutReturnsNotFoundError([FromQuery]string desiredCode, [FromQuery]string desiredMessage)
        {
            this.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.ErrorPayloadType);

            return this.NotFound(new ErrorDto
            {
                Code = desiredCode,
                Message = desiredMessage,
            });
        }

        [HttpPut]
        [Route("put-returns-badrequest-error")]
        public IActionResult PutReturnsBadRequestError([FromQuery]string desiredCode, [FromQuery]string desiredMessage)
        {
            this.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.ErrorPayloadType);

            return this.BadRequest(new ErrorDto
            {
                Code = desiredCode,
                Message = desiredMessage,
            });
        }

        [HttpPut]
        [Route("put-returns-error")]
        public IActionResult PutReturnsError(
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

        [HttpPut]
        [Route("put-returns-validation-error")]
        public IActionResult PutReturnsValidationError(
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

        [HttpPut]
        [Route("put-returns-bad-json")]
        public IActionResult PutReturnsBadJson()
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
