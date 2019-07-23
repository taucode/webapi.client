using System;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class ServiceResourceNotFoundException : ServiceClientException
    {
        public ServiceResourceNotFoundException()
        {
        }

        public ServiceResourceNotFoundException(string message)
            : base(message)
        {
        }

        public ServiceResourceNotFoundException(string code, string message)
            : base(message)
        {

        }

        public ServiceResourceNotFoundException(string code, string message, string uri)
            : base(code, message)
        {
            this.Uri = uri;
        }

        public string Uri { get; set; }
    }
}
