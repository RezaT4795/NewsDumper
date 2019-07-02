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
    class SookDumper : DumperBase, IDumper
    {
        public News ExtractNews(string html, Uri baseUri)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var printButtton= htmlDoc.DocumentNode.GetElementsWithClass("div", "news_print_botton")?.FirstOrDefault();
            var printValue = printButtton.GetAttributeValue("onclick",null).GetItemsWithinQuotes()?.FirstOrDefault(x=>x.Contains("/fa/print"));

            var printUri = $"http://{baseUri.Host}{printValue}";
            var printHtml = Get(printUri);

            var printDoc = new HtmlDocument();
            printDoc.LoadHtml(printHtml);

            var body = printDoc.DocumentNode.GetElementsWithClass("div", "body")?.FirstOrDefault();
            var paragraphs = body.ChildNodes.Where(x => x.Name == "p");
            var text = string.Join(Environment.NewLine,paragraphs.Select(x => x.InnerText.HtmlDecode().Trim()));

            if (text.IsEmpty())
            {
                //Validate for trivia character
                if (!body.InnerText.HtmlDecode().StartsWith("{$"))
                {
                    text = body.InnerText.HtmlDecode().Trim();
                }
            }

            return new News { NewsBody=text };
        }

        

        public void RunAndSave()
        {
            var xml = Get(Constants.SookRss);
            var feed = GetFeed(xml);

            foreach (var item in feed.Items)
            {

                //Validate Uri
                if (item.Links.None())
                {
                    Console.WriteLine("Something is wrong with this feed");
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
