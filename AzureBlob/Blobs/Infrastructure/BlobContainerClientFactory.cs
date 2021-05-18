using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace AzureBlob.Blobs.Infrastructure
{
    public class BlobContainerClientFactory : IBlobContainerClientFactory
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobContainerClientFactory(IOptions<BlobStorageOptions> blobStorageOptions)
        {
            _blobServiceClient = new BlobServiceClient(blobStorageOptions.Value.ConnectionString);
        }

        public async Task<BlobContainerClient> CreateClientAsync(string containerName)
        {
            var client = _blobServiceClient.GetBlobContainerClient(containerName);
            if (await client.ExistsAsync())
            {
                return client;
            }

            return await _blobServiceClient.CreateBlobContainerAsync(containerName);
        }
    }
}