using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;
using AzureBlob.Blobs.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AzureBlob.Blobs.Infrastructure
{
    public class BlobService : IBlobService
    {
        private readonly IBlobContainerClientFactory _blobContainerClientFactory;

        public BlobService(IBlobContainerClientFactory blobContainerClientFactory)
        {
            _blobContainerClientFactory = blobContainerClientFactory;
        }

        public async Task<IEnumerable<BlobListResult>> BrowseAsync(string containerName)
        {
            var containerClient = await _blobContainerClientFactory.CreateClientAsync(containerName);

            var blobs = new List<BlobListResult>();

            await foreach (var item in containerClient.GetBlobsAsync())
            {
                blobs.Add(new BlobListResult
                {
                    Name = item.Name,
                    ContentType = item.Properties.ContentType,
                    ContentLength = item.Properties.ContentLength,
                    ETag = item.Properties.ETag.ToString()
                });
            }

            return blobs;
        }

        public async Task<BlobDownloadResult> DownloadAsync(string containerName, string blobName)
        {
            var blobClient = await GetBlobClientAsync(containerName, blobName);
            
            try
            {
                var result = await blobClient.DownloadAsync();

                return new BlobDownloadResult
                {
                    Name = blobName,
                    Content = result.Value.Content,
                    ContentLength = result.Value.ContentLength,
                    ContentType = result.Value.ContentType,
                    ETag = result.Value.Details.ETag.ToString()
                };
            }
            catch (RequestFailedException)
            {
                return null;
            }
        }

        public async Task<BlobUploadResult> UploadAsync(Stream stream, string containerName, string blobName, string contentType)
        {
            try
            {
                var blobClient = await GetBlobClientAsync(containerName, blobName);
                var result = await blobClient.UploadAsync(stream, new BlobHttpHeaders {ContentType = contentType});
                return GetBlobUploadResult(blobName, contentType, result.Value);
            }
            catch (RequestFailedException exception)
            {
                throw new BlobException("Could not upload file.", exception);
            }
        }

        public async Task<BlobUploadResult> UploadAsync(byte[] data, string containerName, string blobName, string contentType)
        {
            try
            {
                await using var stream = new MemoryStream(data);
                var blobClient = await GetBlobClientAsync(containerName, blobName);
                var result = await blobClient.UploadAsync(stream, new BlobHttpHeaders {ContentType = contentType});
                return GetBlobUploadResult(blobName, contentType, result.Value);
            }
            catch (RequestFailedException exception)
            {
                throw new BlobException("Could not upload file.", exception);
            }
        }

        private async Task<BlobClient> GetBlobClientAsync(string containerName, string blobName)
        {
            var containerClient = await _blobContainerClientFactory.CreateClientAsync(containerName);
            return containerClient.GetBlobClient(blobName);
        }

        private BlobUploadResult GetBlobUploadResult(string name, string contentType, BlobContentInfo contentInfo)
        {
            var nameEncoded = string.Join('/', name.Split('/').Select(HttpUtility.UrlEncode));
            var etagEncoded = HttpUtility.UrlEncode(contentInfo.ETag.ToString());

            return new BlobUploadResult
            {
                Name = name,
                ContentType = contentType,
                ETag = contentInfo.ETag.ToString(),
                Location = $"{nameEncoded}?etag={etagEncoded}"
            };
        }
    }
}