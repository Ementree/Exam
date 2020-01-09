using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Exam.Models
{
    public class HtmlSource
    {
        public int id { get; set; }
        public string source { get; set; }
        public string body { get; set; }
        public string url { get; set; }
        public string domain { get; set; }
    }
}
