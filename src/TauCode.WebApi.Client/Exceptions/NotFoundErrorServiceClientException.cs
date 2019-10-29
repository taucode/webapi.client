using System;
using System.Net;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class NotFoundErrorServiceClientException : ErrorServiceClientException
    {
        public NotFoundErrorServiceClientException(string code, string message)
            : base(HttpStatusCode.NotFound, code, message)
        {
        }
    }
}
