using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ImageStore.Backend.Common.Exceptions
{
    public class ImageStoreException : Exception
    {
        public int StatusCode { get; set; }

        public ImageStoreException(string message, int statusCode = (int)HttpStatusCode.InternalServerError) : base(message)
        {
            StatusCode = statusCode;
        }

        public ImageStoreException() : base()
        {
        }

        public ImageStoreException(string message) : base(message)
        {
        }

        public ImageStoreException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
