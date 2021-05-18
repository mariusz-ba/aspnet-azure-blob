using Azure.Storage.Blobs;
using System.Threading.Tasks;

namespace AzureBlob.Blobs.Infrastructure
{
    public interface IBlobContainerClientFactory
    {
        Task<BlobContainerClient> CreateClientAsync(string containerName);
    }
}