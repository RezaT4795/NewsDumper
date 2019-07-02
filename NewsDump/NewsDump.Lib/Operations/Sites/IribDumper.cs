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
using NewsDump.Lib.Util;
using Olive;

namespace NewsDump.Lib.Operations.Sites
{
    class IribDumper : DumperBase, IDumper
    {
        public News ExtractNews(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // Do assign news.Body and news.Source (If available) ONLY. The rest are assigned below.
            var body = htmlDoc.DocumentNode.GetElementsWithClass("div", "body", "body_media_content_show").FirstOrDefault() ;
            var paragraphs = body.ChildNodes.Where(x => x.Name == "p");
            var newsText = string.Join(Environment.NewLine,paragraphs.Select(x => x.InnerText)).HtmlDecode();


            
           return new News { NewsBody=newsText};
        }

        

        public void RunAndSave()
        {
            var xml = Get(Constants.IribRss);
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
                

                var news = ExtractNews(html);

                //Validate body
                if (news.NewsBody.IsEmpty())
                {
                    Console.WriteLine("News has empty body");
                    continue;
                }
                //Set data from feed
                news = SetNewsFromFeed(news, item);
                

                //Save in database
                news.SaveNewsInDatabase();

            }

        }

        public News SetNewsFromFeed(News news, SyndicationItem feed)
        {
            var uri = feed.GetUri();
            if (feed.Title!=null)
            {
                news.NewsTitle = feed.Title.Text;
            }
            
            news.Link = uri.ToString();

            if (feed.PublishDate!=null)
            {
                news.PublishDate = feed.PublishDate.DateTime;
            }
            
            news.Contributors = string.Join(", ", feed.Authors?.Select(x => x.Name));
            news.SiteName = uri.Host.ToString() ?? "";

            if (feed.Summary!=null)
            {
                news.NewsIntro = feed.Summary.Text ?? "";
            }
           
            return news;
        }
    }
}
