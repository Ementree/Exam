using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Exam.Models
{
    public class HtmlSourceMethods
    {
        public static async Task GetHtmlSource(HtmlSource html)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(html.url);
                string result = null;
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
                html.source = result;
                html.body = GetBody(result);
                html.domain = GetDomain(html.url);
            }
        }

        public static string GetBody(string source)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(source);
            return doc.DocumentNode.InnerText;
        }

        public static List<string> GetAllLinks(HtmlSource html)
        {
            var doc = new HtmlDocument();
            //await GetHtmlSource(html);
            doc.LoadHtml(html.source);
            var result = doc.DocumentNode.Descendants("a").Select(node => node.GetAttributeValue("href", "null")).ToList();
            var links = GetRightLinks(result, html);
            return links;
        }

        private static List<string> GetRightLinks(List<string> links, HtmlSource html)
        {
            var result = new List<string>();
            string modifiedLink = null;
            foreach (var link in links)
            {
                if (link.StartsWith("https://", StringComparison.InvariantCulture))
                    modifiedLink = link;
                else if (link.StartsWith('/'))
                    modifiedLink = html.domain + link;
                else break;
                result.Add(modifiedLink);
            }
            return result;
        }

        public static string GetDomain(string url)
        {
            var parts = url.Split('/');
            return parts[0] + '/' + parts[1] + '/' + parts[2];
        }
    }
}
