using System;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class ServiceResourceForbiddenException : ServiceClientException
    {
        public ServiceResourceForbiddenException()
        {
        }

        public ServiceResourceForbiddenException(string message)
            : base(message)
        {
        }

        public ServiceResourceForbiddenException(string code, string message)
            : base(code, message)
        {
        }
    }
}