using GenericUtility.Services;
using Microsoft.AspNetCore.Mvc;

namespace GenericUtility.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task GetJavapoint(string source)
        {
            var url = "https://www.javatpoint.com/python-history";
            var links = await WebScraper.ScrapeLinksAsync(url);
            var htmlContents = await WebScraper.CaptureHtmlContentAsync(links);

            foreach (var content in htmlContents)
            {
                // Process the HTML content as needed
                Console.WriteLine(content);
            }
        }
    }
}
