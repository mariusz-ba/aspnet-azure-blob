using AzureBlob.Blobs.Abstractions;
using AzureBlob.Blobs.Infrastructure;
using AzureBlob.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace AzureBlob.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AzureController : ControllerBase
    {
        private readonly IBlobService _blobService;
        private readonly BlobsCache _cache;

        public AzureController(IBlobService blobService, BlobsCache cache)
        {
            _blobService = blobService;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetBlobs()
        {
            var blobs = await _blobService.BrowseAsync("images");

            return Ok(blobs.Select(b => new {
                b.Name,
                Size = $"{Math.Round((double) b.ContentLength / 1024.0, 2)} KB"
            }));
        }

        [HttpGet("{*blobName}")]
        [ResponseCache(Duration = 120)]
        public async Task<IActionResult> GetFile(string blobName)
        {
            var result = await _blobService.DownloadAsync("images", blobName);
            if (result is null)
            {
                return NotFound();
            }

            return File(result.Content, result.ContentType);
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] FileUploadRequest request)
        {
            await using var reader = request.File.OpenReadStream();

            try
            {
                var result = await _blobService.UploadAsync(
                    reader, "images", request.File.FileName, request.File.ContentType);
                
                _cache.Set("home", result.Location);
            }
            catch (BlobException exception)
            {
                return BadRequest(exception.Message);
            }

            return Ok();
        }
    }
}