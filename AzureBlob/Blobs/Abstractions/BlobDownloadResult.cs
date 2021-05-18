using System.IO;

namespace AzureBlob.Blobs.Abstractions
{
    public class BlobDownloadResult
    {
        public string Name { get; set; }
        public string ETag { get; set; }
        public string ContentType { get; set; }
        public long? ContentLength { get; set; }
        public Stream Content { get; set; }
    }
}