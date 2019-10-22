using System;
using System.Net;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class BadRequestErrorServiceClientException : ErrorServiceClientException
    {
        public BadRequestErrorServiceClientException(string code, string message)
            : base(HttpStatusCode.BadRequest, code, message)
        {
        }
    }
}
