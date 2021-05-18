namespace AzureBlob.Blobs.Abstractions
{
    public class BlobListResult
    {
        public string Name { get; set; }
        public string ETag { get; set; }
        public string ContentType { get; set; }
        public long? ContentLength { get; set; }
    }
}