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
                var html = Get(item.Links.FirstOrDefault().Uri.ToString());

                var news = ExtractNews(html);

                //Save to DB with the code I shown you before.
            }

        }
    }
}
