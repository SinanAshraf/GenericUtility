using GenericUtility.Models;
using GenericUtility.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace GenericUtility.Controllers
{
    public class TutorialController : Controller
    {
        private readonly ILogger<TutorialController> _logger;
        private const int PageSize = 1;  // Adjust the page size as needed

        public TutorialController(ILogger<TutorialController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Tutorial/Details/{name}")]
        public IActionResult Index(string name, int page = 1)
        {
            ViewData["TutorialName"] = name;

            var tutorials = SampleData.GetTutorials().Where(t => t.Name == name).ToList();
            var paginatedTutorials = tutorials.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            var model = new PaginatedList<TutorialsVM>
            {
                Items = paginatedTutorials,
                PageIndex = page,
                TotalPages = (int)Math.Ceiling(tutorials.Count / (double)PageSize)
            };

            return View("Tutorial", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
