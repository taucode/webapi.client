using System;
using System.Net;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class HttpServiceClientException : ServiceClientException
    {
        public HttpServiceClientException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}
