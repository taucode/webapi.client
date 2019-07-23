using System;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class ServiceBusinessLogicException : ServiceClientException
    {
        public ServiceBusinessLogicException()
        {
        }

        public ServiceBusinessLogicException(string message)
            : base(message)
        {
        }

        public ServiceBusinessLogicException(string code, string message)
            : base(code, message)
        {
        }
    }
}