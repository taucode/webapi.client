using System;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class NotFoundServiceClientException : ServiceClientException
    {
        public NotFoundServiceClientException()
        {
        }

        public NotFoundServiceClientException(string message)
            : base(message)
        {
        }

        public NotFoundServiceClientException(string code, string message)
            : base(code, message)
        {

        }

        public NotFoundServiceClientException(string code, string message, string uri)
            : base(code, message)
        {
            this.Uri = uri;
        }

        public string Uri { get; set; }
    }
}
