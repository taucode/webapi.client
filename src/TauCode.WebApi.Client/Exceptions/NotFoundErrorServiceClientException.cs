using System;
using System.Net;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class NotFoundErrorServiceClientException : ErrorServiceClientException
    {
        public NotFoundErrorServiceClientException(string code, string message)
            : base(HttpStatusCode.NotFound, code, message)
        {
        }

        //public NotFoundErrorServiceClientException(string message)
        //    : base(message)
        //{
        //}

        // todo
        //public NotFoundServiceClientException(string code, string message)
        //    : base(code, message)
        //{

        //}

        //public NotFoundServiceClientException(string code, string message, string uri)
        //    : base(code, message)
        //{
        //    this.Uri = uri;
        //}

        //public string Uri { get; set; }
    }
}
