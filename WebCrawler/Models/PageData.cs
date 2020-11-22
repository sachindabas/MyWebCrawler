using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class PageData
    {
        public PageData()
        {

        }
        public void FetchData(HtmlNode html)
        {
            #region Get Image URL
          this.ImageUrls= WebCrawlerHelper.GetPageImages(html);
            #endregion

            #region Get Page COntent and keywords
            var pageContent = WebCrawlerHelper.GetPageContent(html);
            var final = pageContent.Split(' ').GroupBy(x => x).Select(x => new { KeyField = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).Take(10);
            this.barLabel = final.Select(x => x.KeyField).ToArray();
            this.barData = final.Select(x => x.Count).ToArray();
            #endregion
        }


        [Required]
        [Url]
        public string WebUrl { get; set; }
        public List<string> ImageUrls { get; set; }
        public string[] barLabel { get; set; }
        public int[] barData { get; set; }
    }
}
