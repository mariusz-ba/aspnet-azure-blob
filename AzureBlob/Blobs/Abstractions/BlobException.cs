using System;

namespace AzureBlob.Blobs.Abstractions
{
    public class BlobException : Exception
    {
        public BlobException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}