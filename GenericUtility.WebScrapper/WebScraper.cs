using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericUtility.WebScrapper
{
    public class WebScraper
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<List<string>> ScrapeLinksAsync(string url)
        {
            var html = await client.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var links = doc.DocumentNode.SelectNodes("//div[@id='menu-sidebar']/ul/li/a")
                .Select(node => node.GetAttributeValue("href", null))
                .Where(href => href != null)
                .ToList();

            return links;
        }

        public static async Task<List<string>> CaptureHtmlContentAsync(List<string> links)
        {
            var htmlContents = new List<string>();
            foreach (var link in links)
            {
                var response = await client.GetStringAsync(link);
                htmlContents.Add(response);
            }
            return htmlContents;
        }

    }

}
