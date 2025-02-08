
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace GenericUtility.Services
{
   
    public static class HtmlHelper
    {
        public static HtmlNode GetSingleNode(HtmlDocument doc, string xpath)
        {
            if (doc == null || string.IsNullOrEmpty(xpath)) return null;

            var node = doc.DocumentNode.SelectSingleNode(xpath);
            return node;
        }

        public static IEnumerable<HtmlNode> GetNodes(HtmlDocument doc, string xpath)
        {
            if (doc == null || string.IsNullOrEmpty(xpath)) return Enumerable.Empty<HtmlNode>();

            var nodes = doc.DocumentNode.SelectNodes(xpath);
            return nodes ?? Enumerable.Empty<HtmlNode>();
        }

        public static string GetAttribute(HtmlNode node, string attributeName)
        {
            if (node == null || string.IsNullOrEmpty(attributeName)) return null;

            return node.GetAttributeValue(attributeName, null);
        }
    }

}
