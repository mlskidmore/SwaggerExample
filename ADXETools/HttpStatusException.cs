using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SwaggerExample
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpStatusException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public HttpStatusCode StatusCode { get; protected set; } = HttpStatusCode.InternalServerError;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public HttpStatusException(string message = "", Exception innerException = null)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="reasonPhrase"></param>
        /// <param name="requestMessage"></param>
        public HttpStatusException(HttpStatusCode statusCode, string reasonPhrase, string requestMessage)
            : this($"statusCode: { statusCode }, reason: { reasonPhrase}, request: { requestMessage }")
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="reasonPhrase"></param>
        public HttpStatusException(HttpStatusCode statusCode, string reasonPhrase)
            : this($"statusCode: { statusCode }, reason: { reasonPhrase}")
        {
            StatusCode = statusCode;
        }
    }
}
