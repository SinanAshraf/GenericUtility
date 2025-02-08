using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GenericUtility.Services
{
    public class WebScraper
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<List<string>> ScrapeLinksAsync(string url)
        {
            try
            {
                var html = await client.GetStringAsync(url);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var linkNodes = HtmlHelper.GetNodes(doc, "//div[@id='menu-sidebar']//ul//li//a");
                var links = linkNodes
                    .Select(node => HtmlHelper.GetAttribute(node, "href"))
                    .Where(href => href != null)
                    .ToList();

                if (links.Count == 0)
                {
                    Console.WriteLine("No links found in the specified div.");
                }

                return links;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while scraping links: {ex.Message}");
                return new List<string>();
            }
        }

        public static async Task<List<string>> CaptureHtmlContentAsync(List<string> links)
        {
            var htmlContents = new List<string>();
            foreach (var link in links)
            {
                try
                {
                    var response = await client.GetStringAsync(link);
                    var doc = new HtmlDocument();
                    doc.LoadHtml(response);

                    var contentNode = HtmlHelper.GetSingleNode(doc, "//section[@class='course-details']//div[@class='col-md-7']");
                    if (contentNode != null)
                    {
                        htmlContents.Add(contentNode.InnerHtml);
                    }
                    else
                    {
                        Console.WriteLine($"No content found in the specified div for link: {link}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while capturing HTML content from {link}: {ex.Message}");
                }
            }
            return htmlContents;
        }

    }
}
