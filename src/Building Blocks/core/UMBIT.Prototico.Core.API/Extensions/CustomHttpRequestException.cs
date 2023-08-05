using System;
using System.Net;

namespace UMBIT.Prototico.Core.API.Extensions
{
    public class CustomHttpRequestException : Exception
    {
        public HttpStatusCode StatusCode;

        protected CustomHttpRequestException()
        {

        }

        public CustomHttpRequestException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public CustomHttpRequestException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
