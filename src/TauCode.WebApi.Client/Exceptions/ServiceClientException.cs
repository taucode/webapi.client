using System;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class ServiceClientException : Exception
    {
        public ServiceClientException(string message)
            : base(message)
        {
        }
    }
}
