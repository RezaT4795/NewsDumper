using NewsDump.Lib.Model;
using NewsDump.Lib.Operations.Sites.Interface;
using NewsDump.Lib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using HtmlAgilityPack;

namespace NewsDump.Lib.Operations.Sites
{
    class IribDumper : DumperBase, IDumper
    {
        public News ExtractNews(string html)
        {
            var MyString = html;

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            //var A = htmlDoc.DocumentNode.SelectSingleNode("//title").InnerText;
            var news = new News();
            news.NewsTitle = htmlDoc.DocumentNode.SelectSingleNode("//title").InnerText;
            
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

                news.NewsTitle = item.Title.Text;
                news.Link = uri.ToString();
                news.PublishDate = item.PublishDate.DateTime;
                news.Contributors = string.Join(", ", item.Authors.Select(x => x.Name));
                news.SiteName = uri.Host.ToString();


                
                //Save to DB with the code I shown you before.
            }

        }
    }
}
