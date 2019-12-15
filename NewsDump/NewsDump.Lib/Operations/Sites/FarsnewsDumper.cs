using HtmlAgilityPack;
using NewsDump.Lib.Data;
using NewsDump.Lib.Model;
using NewsDump.Lib.Operations.Sites.Interface;
using NewsDump.Lib.Util;
using Olive;
using System;
using System.Linq;

namespace NewsDump.Lib.Operations.Sites
{
    class FarsnewsDumper : DumperBase, IDumper
    {
        Repository<News> _repo = Repository.Of<News>();
        public News ExtractNews(string html, Uri baseUri)
        {
            var text = "";
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var printButtton1 = htmlDoc.DocumentNode.GetElementsWithClass("div", "print-surl", "d-flex", "align-items-center", "justify-content-center")?.FirstOrDefault();
            if (printButtton1 != null)
            {
                var printButtton2 = printButtton1.GetElementsWithClass("div", "print", "ml-2")?.FirstOrDefault();
                var printValue = printButtton2.SelectSingleNode("//a[contains(@href, '/printnews/')]").Attributes["href"].Value;

                var printUri = $"http://{baseUri.Host}{printValue}";
                var printHtml = Get(printUri);

                var printDoc = new HtmlDocument();
                printDoc.LoadHtml(printHtml);

                var body = printDoc.DocumentNode.GetElementsWithClass("div", "p-nt")?.FirstOrDefault();
                var paragraphs = body.ChildNodes.Where(x => x.Name == "p");
                text = string.Join(Environment.NewLine, paragraphs.Select(x => x.InnerText.HtmlDecode().Trim()));

                if (text.IsEmpty())
                {
                    text = body.InnerText.HtmlDecode().Trim();
                }
            }
            return new News { NewsBody = text };
        }



        public void RunAndSave()
        {
            EventBus.Notify("Farsnews dumper running", "Info");

            var xml = Get(Constants.FarsnewsRss);
            var feed = GetFeed(xml);

            foreach (var item in feed.Items)
            {
                try
                {
                    //Validate Uri
                    if (item.Links.None())
                    {
                        EventBus.Notify("This feed has no links", "Alert");
                        continue;
                    }

                    //Run operation for new items only
                    if (item.NewsExists(_repo))
                    {
                        continue;
                    }

                    var html = Get(item.GetUri().ToString());


                    var news = ExtractNews(html, item.GetUri());




                    //Set data from feed
                    news = SetNewsFromFeed(news, item);

                    if (news.NewsIntro.IsEmpty() && news.NewsBody.HasValue())
                    {
                        news.NewsIntro = news.NewsBody.Take(0, 100) + "...";
                    }


                    //Save in database
                    news.SaveNewsInDatabase(_repo);
                }
                catch (Exception ex)
                {

                    EventBus.Notify("Retrying, Fars...", "Info");
                    EventBus.Log(ex.Message, "retrying in FarsNews");
                }


            }

            EventBus.Notify("Farsnews dumper exiting", "Info");

        }


    }
}
