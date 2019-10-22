using System;
using System.Net;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class ForbiddenErrorServiceClientException : ErrorServiceClientException
    {
        public ForbiddenErrorServiceClientException(string code, string message)
            : base(HttpStatusCode.Forbidden, code, message)
        {
        }
    }
}
