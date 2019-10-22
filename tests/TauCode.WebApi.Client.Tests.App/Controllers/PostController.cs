using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TauCode.WebApi.Client.Tests.App.Dto;

namespace TauCode.WebApi.Client.Tests.App.Controllers
{
    [ApiController]
    public class PostController : ControllerBase
    {
        [HttpPost]
        [Route("post-reverse-person/{prefix}")]
        public IActionResult PostFromRoute(
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

        [HttpPost]
        [Route("post-returns-notfound")]
        public IActionResult PostReturnsNotFound()
        {
            return this.NotFound(new
            {
                firstProp = "first-prop",
                secondProp = "second-prop"
            });
        }

        [HttpPost]
        [Route("post-returns-desired-generic-statuscode")]
        public IActionResult PostReturnsDesiredGenericStatusCode(
            [FromQuery] HttpStatusCode desiredStatusCode,
            [FromQuery] string desiredContent)
        {
            return new ContentResult
            {
                StatusCode = (int)desiredStatusCode,
                Content = desiredContent,
            };
        }

        [HttpPost]
        [Route("post-returns-notfound-error")]
        public IActionResult PostReturnsNotFoundError([FromQuery]string desiredCode, [FromQuery]string desiredMessage)
        {
            this.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.ErrorPayloadType);

            return this.NotFound(new ErrorDto
            {
                Code = desiredCode,
                Message = desiredMessage,
            });
        }

        [HttpPost]
        [Route("post-returns-badrequest-error")]
        public IActionResult PostReturnsBadRequestError([FromQuery]string desiredCode, [FromQuery]string desiredMessage)
        {
            this.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.ErrorPayloadType);

            return this.BadRequest(new ErrorDto
            {
                Code = desiredCode,
                Message = desiredMessage,
            });
        }

        [HttpPost]
        [Route("post-returns-error")]
        public IActionResult PostReturnsError(
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

        [HttpPost]
        [Route("post-returns-validation-error")]
        public IActionResult PostReturnsValidationError(
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

        [HttpPost]
        [Route("post-returns-bad-json")]
        public IActionResult PostReturnsBadJson()
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
