using System.Collections.Generic;

namespace AzureBlob.Blobs.Infrastructure
{
    public class BlobsCache
    {
        private readonly Dictionary<string, string> _cache = new();

        public void Set(string key, string value) => _cache[key] = value;

        public string GetOrDefault(string key) => _cache.GetValueOrDefault(key);
    }
}