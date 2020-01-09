using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam.Models;
using Newtonsoft.Json;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string url, int depth)
        {
            var walker = new LinkWalker(url, depth);
            var links = await walker.Walk(1, new List<string>() { url }, new List<HtmlSource>());
            ViewBag.Data = links;
            ViewBag.DataSer = JsonConvert.SerializeObject(links);

            return View("~/Views/Home/SaveChanges.cshtml");
        }

        [HttpPost]
        public IActionResult SaveChanges(List<HtmlSource> data, string dataser)
        {
            var links = JsonConvert.DeserializeObject<List<HtmlSource>>(dataser);
            var db = new HtmlContext();
            foreach (var link in links)
            {
                db.Add(link);
            }
            db.SaveChanges();
            ViewBag.Links = db.Htmls.Distinct().ToList();
            ViewBag.Flag = true;
            return View("~/Views/Home/ShowResult.cshtml");
        }

        [HttpPost]
        public IActionResult ShowResult(string domain)
        {
            var db = new HtmlContext();
            var links = new List<HtmlSource>();
            var flag = false;

            if (domain != null)
            {
                foreach (var html in db.Htmls)
                {
                    var domainHtml = html.url.Split('/');
                    if (html.url.Contains(domain))
                        links.Add(html);
                }

                if (links.Count() == 0)
                    links.Add(new HtmlSource());
                else
                {
                    flag = true;
                }
            }
            else links = db.Htmls.ToList();

            ViewBag.Links = links.Distinct().ToList();
            ViewBag.Flag = flag;
            return View("~/Views/Home/ShowResult.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
