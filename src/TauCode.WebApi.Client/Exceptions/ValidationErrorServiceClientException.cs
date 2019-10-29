using System;
using System.Collections.Generic;
using System.Net;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class ValidationErrorServiceClientException : ErrorServiceClientException
    {
        private readonly IDictionary<string, ValidationFailureDto> _failures;

        public ValidationErrorServiceClientException(string code, string message, IDictionary<string, ValidationFailureDto> failures)
            : base(HttpStatusCode.BadRequest, code, message)
        {
            _failures = failures;
        }

        public IDictionary<string, ValidationFailureDto> Failures => _failures;
    }
}
