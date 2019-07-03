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
    class FarsnewsDumper : DumperBase, IDumper
    {
        public News ExtractNews(string html, Uri baseUri)
        {
            var text = "";
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var printButtton1= htmlDoc.DocumentNode.GetElementsWithClass("div", "print-surl", "d-flex", "align-items-center", "justify-content-center")?.FirstOrDefault();
            if (printButtton1 != null)
            {
                var printButtton2= printButtton1.GetElementsWithClass("div", "print","ml-2")?.FirstOrDefault();
                var printValue = printButtton2.SelectSingleNode("//a[contains(@href, '/printnews/')]").Attributes["href"].Value;

                var printUri = $"http://{baseUri.Host}{printValue}";
                var printHtml = Get(printUri);

                var printDoc = new HtmlDocument();
                printDoc.LoadHtml(printHtml);

                var body = printDoc.DocumentNode.GetElementsWithClass("div", "p-nt")?.FirstOrDefault();
                var paragraphs = body.ChildNodes.Where(x => x.Name == "p");
                text = string.Join(Environment.NewLine,paragraphs.Select(x => x.InnerText.HtmlDecode().Trim()));

                if (text.IsEmpty())
                {
                    text = body.InnerText.HtmlDecode().Trim();
                }
            }
            return new News { NewsBody=text };
        }

        

        public void RunAndSave()
        {
            EventBus.Notify("Farsnews Dumper initializing", "Info");

            var xml = Get(Constants.FarsnewsRss);
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

            EventBus.Notify("Farsnews Dumper Exiting", "Info");

        }

        public News SetNewsFromFeed(News news, SyndicationItem feed)
        {
            var uri = feed.GetUri();
            if (feed.Title!=null)
            {
                news.NewsTitle = feed.Title.Text.Trim();
            }
            
            news.Link = uri.ToString();

            if (feed.PublishDate!=null)
            {
                news.PublishDate = feed.PublishDate.DateTime;
            }
            
            news.Contributors = string.Join(", ", feed.Authors?.Select(x => x.Name)).Trim();
            news.SiteName = uri.Host;

            if (feed.Summary!=null)
            {
                news.NewsIntro = feed.Summary.Text.Trim();
            }
           
            return news;
        }
    }
}
