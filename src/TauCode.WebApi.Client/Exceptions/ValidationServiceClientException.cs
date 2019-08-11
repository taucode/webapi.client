using System;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class ValidationServiceClientException : ServiceClientException
    {
        public ValidationServiceClientException(ValidationErrorDto validationErrorResponse)
            : base(validationErrorResponse.Code, validationErrorResponse.Message)
        {
            this.ValidationErrorResponse = validationErrorResponse;
        }

        public ValidationErrorDto ValidationErrorResponse { get; set; }
    }
}
