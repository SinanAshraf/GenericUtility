using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GenericUtility.WebScrapper
{
    class Program
    {
        static async Task Main(string[] args)
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
