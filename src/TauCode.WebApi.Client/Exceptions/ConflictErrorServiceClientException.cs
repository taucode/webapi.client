using System;
using System.Net;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class ConflictErrorServiceClientException : ErrorServiceClientException
    {
        public ConflictErrorServiceClientException(string code, string message)
            : base(HttpStatusCode.Conflict, code, message)
        {
        }
    }
}