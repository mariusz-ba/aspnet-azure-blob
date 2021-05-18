using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AzureBlob.Contracts
{
    public class FileUploadRequest
    {
        [Required]
        [FileMaxSize(128 * 1024)]
        [FileContentType("image/jpeg,image/png")]
        public IFormFile File { get; set; }
    }
}