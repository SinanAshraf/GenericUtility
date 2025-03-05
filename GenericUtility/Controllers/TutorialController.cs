using GenericUtility.Models;
using GenericUtility.Services;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace GenericUtility.Controllers
{
    public class TutorialController : Controller
    {
        private readonly ILogger<TutorialController> _logger;
        private readonly IMemoryCache _cache;
        private const int PageSize = 20;  // Adjust the page size as needed

        public TutorialController(ILogger<TutorialController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        [HttpGet("Tutorial/Details/{name}")]
        public IActionResult Index(string name, int page = 1)
        {
            name = ".Net";
            ViewData["TutorialName"] = name;

            var cacheKey = $"tutorials_{name}";
            if (!_cache.TryGetValue(cacheKey, out List<TutorialsVM> tutorials))
            {
                tutorials = new List<TutorialsVM>();
                var basePath = $"Data/CoursesLinks/{name}";
                string removePrefix = "Data/CoursesLinks/.Net\\httpswww.javatpoint.com";

                var courseFiles = Directory.GetFiles(basePath, "*.html", SearchOption.AllDirectories);

                foreach (var file in courseFiles)
                {
                    var fileContent = System.IO.File.ReadAllText(file);
                    string result = file.Substring(removePrefix.Length);

                    var tutorial = new TutorialsVM() { Name = result, Content = fileContent };
                    tutorials.Add(tutorial);
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Adjust the cache expiration as needed

                _cache.Set(cacheKey, tutorials, cacheEntryOptions);
            }

            var paginatedTutorials = tutorials.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            var model = new PaginatedList<TutorialsVM>
            {
                Items = paginatedTutorials,
                PageIndex = page,
                TotalPages = (int)Math.Ceiling(tutorials.Count / (double)PageSize)
            };

            return View("Tutorial", model);
        }

        public IActionResult Details(string courseName, string ContentRequested, int page = 1)
        {
            var tutorials = new List<TutorialsVM>();
            var basePath = $"Data/CoursesLinks/{courseName}";
            string removePrefix = "Data/CoursesLinks/.Net\\httpswww.javatpoint.com";

            var courseFiles = Directory.GetFiles(basePath, "*.html", SearchOption.AllDirectories);

            foreach (var file in courseFiles)
            {
                var fileContent = System.IO.File.ReadAllText(file);
                string result = file.Substring(removePrefix.Length);

                var tutorial = new TutorialsVM() { Name = result, Content = fileContent };
                tutorials.Add(tutorial);
            }

            var paginatedTutorials = tutorials.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            var model = new PaginatedList<TutorialsVM>
            {
                Items = paginatedTutorials,
                PageIndex = page,
                TotalPages = (int)Math.Ceiling(tutorials.Count / (double)PageSize)
            };

            return View("Tutorial", model);
        }

        [HttpGet("Tutorial/GetCourses")]
        public List<CoursesVM> GetCourses()
        {
            var cacheKey = "courses";
            if (!_cache.TryGetValue(cacheKey, out List<CoursesVM> courses))
            {
                courses = new List<CoursesVM>();
                var filePath = "Data/Links.json";

                if (System.IO.File.Exists(filePath))
                {
                    var fileContent = System.IO.File.ReadAllText(filePath);
                    var coursesDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileContent);

                    foreach (var course in coursesDict)
                    {
                        var courseVM = new CoursesVM
                        {
                            Name = course.Key,
                            Link = "Tutorial/Details/" + course.Key
                        };
                        courses.Add(courseVM);
                    }
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Adjust the cache expiration as needed

                _cache.Set(cacheKey, courses, cacheEntryOptions);
            }

            return courses;
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
