using AzureBlob.Blobs.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureBlob.Blobs.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddAzureBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BlobStorageOptions>(configuration.GetSection(nameof(BlobStorageOptions)));
            services.AddSingleton<IBlobContainerClientFactory, BlobContainerClientFactory>();
            services.AddTransient<IBlobService, BlobService>();
            
            services.AddSingleton(sp =>
            {
                var cache = new BlobsCache();
                cache.Set("home", "test.jpg");
                return cache;
            });
            
            return services;
        }
    }
}