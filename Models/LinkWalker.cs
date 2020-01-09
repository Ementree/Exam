using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exam.Models
{
    public class LinkWalker
    {
        public int depth;
        public HashSet<string> addedLinks;

        public LinkWalker(string url, int depth)
        {
            this.depth = depth;
            addedLinks = new HashSet<string>();
        }

        public object HtmlSourceMethod { get; private set; }

        public async Task<List<HtmlSource>> Walk(int currentDepth, List<string> currentLinks, List<HtmlSource> resultLinks)
        {
            foreach (var link in currentLinks)
            {
                if (!addedLinks.Contains(link))
                {
                    addedLinks.Add(link);
                    var html = new HtmlSource() { url = link };
                    await HtmlSourceMethods.GetHtmlSource(html);

                    resultLinks.Add(html);

                    if (depth > currentDepth)
                    {
                        await Walk(currentDepth + 1, HtmlSourceMethods.GetAllLinks(html), resultLinks);
                    }
                }

            }
            return resultLinks;
        }
    }
}
