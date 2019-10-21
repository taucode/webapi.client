using System;

namespace TauCode.WebApi.Client.Exceptions
{
    [Serializable]
    public class ForbiddenServiceClientException : ServiceClientException
    {
        public ForbiddenServiceClientException()
        {
        }

        public ForbiddenServiceClientException(string message)
            : base(message)
        {
        }

        // todo
        //public ForbiddenServiceClientException(string code, string message)
        //    : base(code, message)
        //{
        //}
    }
}
