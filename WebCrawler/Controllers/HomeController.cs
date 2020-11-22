using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using ScrapySharp.Network;
using WebCrawler.Models;

namespace WebCrawler.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(PageData pageData)
        {
            if (ModelState.IsValid)
            {
                var html = WebCrawlerHelper.GetHtml(pageData.WebUrl);
                pageData.FetchData(html);
                return View(pageData);
            }
            return View(pageData);
        }
    }
}
