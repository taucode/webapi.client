using System;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class BadRequestServiceClientException : ServiceClientException
    {
        public BadRequestServiceClientException()
        {
        }

        public BadRequestServiceClientException(string message)
            : base(message)
        {
        }

        public BadRequestServiceClientException(string code, string message)
            : base(code, message)
        {
        }
    }
}