using HtmlAgilityPack;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebCrawler
{
    public static class WebCrawlerHelper
    {
        static ScrapingBrowser _browser = new ScrapingBrowser();
        public static HtmlNode GetHtml(string url)
        {
            try
            {
                WebPage webpage = _browser.NavigateToPage(new Uri(url));
                return webpage.Html;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static List<string> GetPageImages(HtmlNode html)
        {
            var ImageUrls = new List<string>();
            var links = html?.SelectNodes(@"//img[@src]");
            if (links != null)
            {
                foreach (var link in links)
                {
                    if (link.Attributes?["src"].Value != null && !string.IsNullOrEmpty(Regex.Match(link.Attributes?["src"].Value, @"(http(s?):)([/|.|\w|\s|-])*\.(?:jpg|gif|png)").Value))
                    {
                        ImageUrls.Add(link.Attributes["src"].Value);
                    }
                }
            }
            return ImageUrls;
        }

        public static string GetPageContent(HtmlNode html)
        {
            #region List all ignore words
            //Master list can be stored/fetched from csv
            string ignoreWordlist = "is|am|are|he|she|it|was|were|do|does|a|an|the|amp|s|and|by|of|to|on|as|in|for";
            string ignorePattern = "\\b" + string.Join("\\b|\\b", ignoreWordlist.Split('|')) + "\\b";
            var ignorerx = new Regex(ignorePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            #endregion

            #region Fetch Text from HTML document
            var requiredNodes = html?.SelectNodes("//body//*[not(self::script)]/text()");
            var pageContent = string.Empty;
            if (requiredNodes != null)
            {
                foreach (HtmlNode node in requiredNodes)
                {
                    var text = Regex.Replace(node.InnerText, @"[^a-zA-Z]+", " ").Trim();
                    if (!string.IsNullOrEmpty(text) && (!ignorerx.IsMatch(text)) && text.Length > 1)
                    {
                        pageContent += " " + text;
                    }
                }
            }
            return pageContent;
            #endregion
        }
    }
}
