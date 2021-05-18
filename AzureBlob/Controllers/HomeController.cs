using AzureBlob.Blobs.Infrastructure;
using AzureBlob.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlob.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlobsCache _cache;

        public HomeController(BlobsCache cache)
        {
            _cache = cache;
        }

        public ViewResult Index()
        {
            var model = new HomeViewModel
            {
                Image = $"{Url.ActionLink("GetFile", "Azure")}/{_cache.GetOrDefault("home")}"
            };
            
            return View(model);
        }
    }
}