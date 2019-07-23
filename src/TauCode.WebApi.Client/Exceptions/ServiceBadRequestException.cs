using System;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class ServiceBadRequestException : ServiceClientException
    {
        public ServiceBadRequestException()
        {
        }

        public ServiceBadRequestException(string message)
            : base(message)
        {
        }

        public ServiceBadRequestException(string code, string message)
            : base(code, message)
        {
        }
    }
}