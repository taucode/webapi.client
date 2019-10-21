using System;
using System.Net;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class ErrorServiceClientException : ServiceClientException
    {
        public ErrorServiceClientException(HttpStatusCode statusCode, string code, string message)
            : base(message)
        {
            this.StatusCode = statusCode;
            this.Code = code;
        }

        public HttpStatusCode StatusCode { get; }

        public string Code { get; }
    }
}
