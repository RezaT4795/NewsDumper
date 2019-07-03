using NewsDump.Lib.Model;
using NewsDump.Lib.Operations.Sites.Interface;
using NewsDump.Lib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using HtmlAgilityPack;
using System.ServiceModel.Syndication;
using Olive;

namespace NewsDump.Lib.Operations.Sites
{
    class IrnaDumper : DumperBase, IDumper
    {
        public News ExtractNews(string html, Uri baseUri)
        {
            var text = "";
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var printButtton1= htmlDoc.DocumentNode.GetElementsWithClass("div", "item-title")?.FirstOrDefault();
            var printButtton2= printButtton1.GetElementsWithClass("h1", "title")?.FirstOrDefault();
            if (printButtton2 != null)
            {
                var printValue = printButtton2.SelectSingleNode("//a[contains(@href, '/news/')]").Attributes["href"].Value;

            var printUri = $"http://{baseUri.Host}{printValue}";
            var printHtml = Get(printUri);

            var printDoc = new HtmlDocument();
            printDoc.LoadHtml(printHtml);

            var body = printDoc.DocumentNode.GetElementsWithClass("div", "item-body")?.FirstOrDefault();
            var paragraphs = body.ChildNodes.Where(x => x.Name == "p");
            text = string.Join(Environment.NewLine,paragraphs.Select(x => x.InnerText.HtmlDecode().Trim()));

            if (text.IsEmpty())
            {
                //Validate for trivia character
                if (!body.InnerText.HtmlDecode().StartsWith("{$"))
                {
                    text = body.InnerText.HtmlDecode().Trim();
                }
            }
            }
            return new News{ NewsBody=text };
        }

        

        public void RunAndSave()
        {
            EventBus.Notify("Irna dumper running", "Info");

            var xml = Get(Constants.IrnaRss);
            var feed = GetFeed(xml);

            foreach (var item in feed.Items)
            {

                //Validate Uri
                if (item.Links.None())
                {
                    EventBus.Notify("Something is wrong with this feed","Alert");
                    continue;
                }

                //Run operation for new items only
                if (item.NewsExists())
                {
                    continue;
                }

                var html = Get(item.GetUri().ToString());
                

                var news = ExtractNews(html,item.GetUri());

                
                

                //Set data from feed
                news = SetNewsFromFeed(news, item);

                if (news.NewsIntro.IsEmpty() && news.NewsBody.HasValue())
                {
                    news.NewsIntro = news.NewsBody.Take(0,100)+"...";
                }
                

                //Save in database
                news.SaveNewsInDatabase();

            }

            EventBus.Notify("Irna dumper exiting", "Info");

        }

       
    }
}
