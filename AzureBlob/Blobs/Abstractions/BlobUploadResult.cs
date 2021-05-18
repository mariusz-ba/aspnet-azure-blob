namespace AzureBlob.Blobs.Abstractions
{
    public class BlobUploadResult
    {
        public string Name { get; set; }
        public string ETag { get; set; }
        public string ContentType { get; set; }
        public string Location { get; set; }
    }
}