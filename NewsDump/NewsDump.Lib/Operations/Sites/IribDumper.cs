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

namespace NewsDump.Lib.Operations.Sites
{
    class IribDumper : DumperBase, IDumper
    {
        public News ExtractNews(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var news = new News();
            // Do assign news.Body and news.Source ONLY. The rest are assigned below.

            
            return news;
        }

        

        public void RunAndSave()
        {
            var xml = Get(Constants.IribRss);
            var feed = GetFeed(xml);

            foreach (var item in feed.Items)
            {
                var uri = item.Links?.FirstOrDefault().Uri;
                var html = Get(uri.ToString());

                var news = ExtractNews(html);
                //Set data from feed
                news = SetNewsFromFeed(news, item);
                

                //Save in database
                news.SaveNewsInDatabase();
               
            }

        }

        public News SetNewsFromFeed(News news, SyndicationItem feed)
        {
            var uri = feed.Links?.FirstOrDefault().Uri;

            news.NewsTitle = feed.Title.Text;
            news.Link = uri.ToString();
            news.PublishDate = feed.PublishDate.DateTime;
            news.Contributors = string.Join(", ", feed.Authors.Select(x => x.Name));
            news.SiteName = uri.Host.ToString();
            news.NewsIntro = feed.Summary.Text;
            return news;
        }
    }
}
