using System;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class ServiceClientException : Exception
    {
        public ServiceClientException()
        {
        }

        public ServiceClientException(string message)
            : base(message)
        {
        }

        public ServiceClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // todo
        //public ServiceClientException(string code, string message)
        //    : base(message)
        //{
        //    this.Code = code;
        //}

        //public string Code { get; }
    }
}
