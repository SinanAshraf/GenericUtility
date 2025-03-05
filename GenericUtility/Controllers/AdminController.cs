using GenericUtility.Services;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace GenericUtility.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task GetJavaPointContentLinks()
        {
            try
            {
                string url = "https://www.javatpoint.com/";
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(url);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var linksDictionary = new Dictionary<string, object>();

                // Extract all links on the page
                var linkNodes = htmlDocument.DocumentNode.SelectNodes("//a[@href]");
                if (linkNodes != null)
                {
                    foreach (var link in linkNodes)
                    {
                        string linkText = link.InnerText.Trim();
                        string hrefValue = link.Attributes["href"].Value;
                        // Remove duplicate slashes (except for the protocol part)
                        if (hrefValue.StartsWith("/"))
                        {
                            hrefValue = hrefValue.Substring(1);
                        }

                        // Exclude compiler links
                        if (!hrefValue.Contains("compiler"))
                        {
                            linkText = Regex.Unescape(linkText);
                            linksDictionary[linkText] = url + hrefValue;
                        }
                    }
                    string jsonString = JsonSerializer.Serialize(linksDictionary, new JsonSerializerOptions { WriteIndented = true });
                    // Define the file path
                    string folderPath = "Data";
                    string filePath = Path.Combine(folderPath, "links.json");
                    try
                    {
                        // Create the Data folder if it doesn't exist
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        // Write the JSON string to the file in the Data folder
                        await System.IO.File.WriteAllTextAsync(filePath, jsonString);
                        Console.WriteLine("Links have been written to " + filePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error details: " + ex.InnerException);
                Console.WriteLine("Error occurred at: " + ex.StackTrace);
            }
        }

        public async Task GetSidebarLinksFromTopics()
        {
            try
            {
                string inputFilePath = "Data/links.json";
                string jsonString = await System.IO.File.ReadAllTextAsync(inputFilePath);

                var topicsLinksDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

                var httpClient = new HttpClient();

                foreach (var topic in topicsLinksDictionary)
                {
                    string topicName = topic.Key;
                    string topicUrl = topic.Value;

                    if (!topicUrl.EndsWith("#"))
                    {
                        try
                        {
                            var html = await httpClient.GetStringAsync(topicUrl);
                            var htmlDocument = new HtmlDocument();
                            htmlDocument.LoadHtml(html);

                            var sidebarLinks = new List<string>();
                            var sidebarDiv = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='sidebar']");

                            if (sidebarDiv != null)
                            {
                                var linkNodes = sidebarDiv.SelectNodes(".//a[@href]");
                                if (linkNodes != null)
                                {
                                    foreach (var link in linkNodes)
                                    {
                                        string hrefValue = link.Attributes["href"].Value;

                                        // Ensure the href value is a valid URL
                                        if (!hrefValue.StartsWith("http") && !hrefValue.StartsWith("https"))
                                        {
                                            hrefValue = "https://www.javatpoint.com" + hrefValue;
                                        }

                                        sidebarLinks.Add(hrefValue);
                                    }
                                }
                            }

                            // Serialize the sidebar links to JSON
                            string outputJsonString = JsonSerializer.Serialize(sidebarLinks, new JsonSerializerOptions { WriteIndented = true });

                            // Define the file path
                            string folderPath = "CoursesLinks\\Data";
                            string sanitizedTopicName = string.Concat(topicName.Split(Path.GetInvalidFileNameChars())); // Sanitize the file name
                            string filePath = Path.Combine(folderPath, $"{sanitizedTopicName}.json");
                            try
                            {
                                // Create the Data folder if it doesn't exist
                                if (!Directory.Exists(folderPath))
                                {
                                    Directory.CreateDirectory(folderPath);
                                }

                                // Write the JSON string to the file in the Data folder
                                await System.IO.File.WriteAllTextAsync(filePath, outputJsonString);
                                Console.WriteLine($"Sidebar links for {topicName} have been written to " + filePath);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"An error occurred while writing {topicName}.json: " + ex.Message);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred while writing {topicName}.json: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public async Task GetTutorialsContent()
        {
            string folderPath = "Data\\CoursesLinks";
            await FetchAndDownloadCityDivContent(folderPath, 10);
        }

        public async Task FetchAndDownloadCityDivContent(string folderPath)
        {
            try
            {
                var httpClient = new HttpClient();
                var jsonFiles = Directory.GetFiles(folderPath, "*.json");

                foreach (var filePath in jsonFiles)
                {
                    string jsonString = await System.IO.File.ReadAllTextAsync(filePath);

                    // Deserialize the JSON data to a list of URLs
                    var urls = JsonSerializer.Deserialize<List<string>>(jsonString);

                    // Extract the course name from the file path
                    string courseName = Path.GetFileNameWithoutExtension(filePath);

                    // Define the course-specific folder path
                    string courseFolderPath = Path.Combine(folderPath, courseName);

                    // Create the course-specific folder if it doesn't exist
                    if (!Directory.Exists(courseFolderPath))
                    {
                        Directory.CreateDirectory(courseFolderPath);
                    }

                    foreach (var url in urls)
                    {
                        try
                        {
                            var html = await httpClient.GetStringAsync(url);
                            var htmlDocument = new HtmlDocument();
                            htmlDocument.LoadHtml(html);

                            var cityDiv = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='city']");

                            if (cityDiv != null)
                            {
                                string cityDivHtml = cityDiv.OuterHtml;

                                // Sanitize the URL to create a valid file name
                                string sanitizedUrl = string.Concat(url.Split(Path.GetInvalidFileNameChars()));
                                string outputFilePath = Path.Combine(courseFolderPath, $"{sanitizedUrl}.html");

                                try
                                {
                                    // Write the HTML content of the city div to the file
                                    await System.IO.File.WriteAllTextAsync(outputFilePath, cityDivHtml);
                                    Console.WriteLine($"Content of div#city from {url} has been written to " + outputFilePath);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"An error occurred while writing {outputFilePath}: " + ex.Message);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred while Getting Data. " + ex.Message);

                        }
                    }
                }

                //var tasks = new List<Task>();
                //foreach (var filePath in jsonFiles)
                //{
                //    tasks.Add(Task.Run(async () =>
                //    {
                //        string jsonString = await System.IO.File.ReadAllTextAsync(filePath);

                //        // Deserialize the JSON data to a list of URLs
                //        var urls = JsonSerializer.Deserialize<List<string>>(jsonString);

                //        // Extract the course name from the file path
                //        string courseName = Path.GetFileNameWithoutExtension(filePath);

                //        // Define the course-specific folder path
                //        string courseFolderPath = Path.Combine(folderPath, courseName);

                //        // Create the course-specific folder if it doesn't exist
                //        if (!Directory.Exists(courseFolderPath))
                //        {
                //            Directory.CreateDirectory(courseFolderPath);
                //        }

                //        var urlTasks = urls.Select(async url =>
                //        {
                //            try
                //            {
                //                var html = await httpClient.GetStringAsync(url);
                //                var htmlDocument = new HtmlDocument();
                //                htmlDocument.LoadHtml(html);

                //                var cityDiv = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='city']");

                //                if (cityDiv != null)
                //                {
                //                    string cityDivHtml = cityDiv.OuterHtml;

                //                    // Sanitize the URL to create a valid file name
                //                    string sanitizedUrl = string.Concat(url.Split(Path.GetInvalidFileNameChars()));
                //                    string outputFilePath = Path.Combine(courseFolderPath, $"{sanitizedUrl}.html");

                //                    try
                //                    {
                //                        // Write the HTML content of the city div to the file
                //                        await System.IO.File.WriteAllTextAsync(outputFilePath, cityDivHtml);
                //                        Console.WriteLine($"Content of div#city from {url} has been written to " + outputFilePath);
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        Console.WriteLine($"An error occurred while writing {outputFilePath}: " + ex.Message);
                //                    }
                //                }
                //            }
                //            catch (Exception ex)
                //            {
                //                Console.WriteLine($"An error occurred while Getting Data. " + ex.Message);
                //            }
                //        });

                //        await Task.WhenAll(urlTasks);
                //    }));
                //}
                //await Task.WhenAll(tasks);

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public async Task FetchAndDownloadCityDivContent(string folderPath, int batchSize)
        {
            try
            {
                var httpClientHandler = new HttpClientHandler();
                var httpClient = new HttpClient(httpClientHandler)
                {
                    Timeout = TimeSpan.FromMinutes(5) // Set the timeout to 5 minutes
                };


                var random = new Random();
                var jsonFiles = Directory.GetFiles(folderPath, "*.json");

                foreach (var filePath in jsonFiles)
                {
                    string jsonString = await System.IO.File.ReadAllTextAsync(filePath);

                    // Deserialize the JSON data to a list of URLs
                    var urls = JsonSerializer.Deserialize<List<string>>(jsonString);

                    // Extract the course name from the file path
                    string courseName = Path.GetFileNameWithoutExtension(filePath);

                    // Define the course-specific folder path
                    string courseFolderPath = Path.Combine(folderPath, courseName);

                    // Create the course-specific folder if it doesn't exist
                    if (!Directory.Exists(courseFolderPath))
                    {
                        Directory.CreateDirectory(courseFolderPath);
                    }

                    var batches = new List<List<string>>();

                    for (int i = 0; i < urls.Count; i += batchSize)
                    {
                        batches.Add(urls.Skip(i).Take(batchSize).ToList());
                    }

                    await Task.WhenAll(batches.Select(batch => Task.Run(() =>
                    {
                        Parallel.ForEach(batch, async url =>
                        {
                            try
                            {
                                // Introduce a random delay
                                int delay = random.Next(500, 2000);
                                await Task.Delay(delay);

                                var html = await httpClient.GetStringAsync(url);
                                var htmlDocument = new HtmlDocument();
                                htmlDocument.LoadHtml(html);

                                var cityDiv = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='city']");

                                if (cityDiv != null)
                                {
                                    string cityDivHtml = cityDiv.OuterHtml;

                                    // Sanitize the URL to create a valid file name
                                    string sanitizedUrl = string.Concat(url.Split(Path.GetInvalidFileNameChars()));
                                    string outputFilePath = Path.Combine(courseFolderPath, $"{sanitizedUrl}.html");

                                    try
                                    {
                                        // Write the HTML content of the city div to the file
                                        await System.IO.File.WriteAllTextAsync(outputFilePath, cityDivHtml);
                                        Console.WriteLine($"Content of div#city from {url} has been written to " + outputFilePath);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"An error occurred while writing {outputFilePath}: " + ex.Message);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"An error occurred while Getting Data. " + ex.Message);
                            }
                        });
                    })));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public void RemoveSpecificDivs()
        {
            string directoryPath = "Data/CoursesLinks/";
            // Ensure the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Directory does not exist.");
                return;
            }

            // Get all HTML files in the directory
            var htmlFiles = Directory.GetFiles(directoryPath, "*.html", SearchOption.AllDirectories);

            foreach (var file in htmlFiles)
            {
                var doc = new HtmlDocument();
                doc.Load(file);

                // Remove divs with specific IDs and class
                var divsToRemove = doc.DocumentNode.SelectNodes("//div[@id='bottomnextup' or @id='bottomnext' or contains(@class, 'nexttopicdiv')]");
                if (divsToRemove != null)
                {
                    foreach (var div in divsToRemove)
                    {
                        div.Remove();
                    }
                }

                // Save the modified HTML back to the file
                doc.Save(file);
            }

            Console.WriteLine("Specified divs removed successfully.");
        }

        public void FormatAndSaveHtmlFiles()
        {
            string directoryPath = "Data/CoursesLinks/";
            // Ensure the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Directory does not exist.");
                return;
            }

            // Get all HTML files in the directory
            var htmlFiles = Directory.GetFiles(directoryPath, "*.html", SearchOption.AllDirectories);

            foreach (var file in htmlFiles)
            {
                var doc = new HtmlDocument();
                doc.Load(file);

                // Format the HTML
                doc.OptionOutputAsXml = true; // Preserve the output format
                string formattedHtml = doc.DocumentNode.OuterHtml;

                // Save the formatted HTML back to the file
                System.IO.File.WriteAllText(file, formattedHtml);
            }

            Console.WriteLine("HTML files formatted and saved successfully.");
        }


        //public void FormatHtmlFiles(string directoryPath)
        //{
        //    // Ensure the directory exists
        //    if (!Directory.Exists(directoryPath))
        //    {
        //        Console.WriteLine("Directory does not exist.");
        //        return;
        //    }

        //    // Get all HTML files in the directory
        //    var htmlFiles = Directory.GetFiles(directoryPath, "*.html", SearchOption.AllDirectories);

        //    foreach (var file in htmlFiles)
        //    {
        //        var fileContent = System.IO.File.ReadAllText(file);

        //        // Format the HTML content
        //        var prettier = new Prettier();
        //        var formattedHtml = prettier.Format(fileContent, new PrettierOptions
        //        {
        //            Parser = "html"
        //        });

        //        // Save the formatted HTML back to the file
        //        System.IO.File.WriteAllText(file, formattedHtml);
        //    }

        //    Console.WriteLine("HTML files formatted and saved successfully.");
        //}
    }
}