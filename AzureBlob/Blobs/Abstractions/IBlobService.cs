using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AzureBlob.Blobs.Abstractions
{
    public interface IBlobService
    {
        Task<IEnumerable<BlobListResult>> BrowseAsync(string containerName);
        Task<BlobDownloadResult> DownloadAsync(string containerName, string blobName);
        Task<BlobUploadResult> UploadAsync(Stream stream, string containerName, string blobName, string contentType);
        Task<BlobUploadResult> UploadAsync(byte[] data, string containerName, string blobName, string contentType);
    }
}