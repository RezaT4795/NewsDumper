using HtmlAgilityPack;
using NewsDump.Lib.Model;
using NewsDump.Lib.Operations.Sites.Interface;
using NewsDump.Lib.Util;
using Olive;
using System;
using System.Linq;

namespace NewsDump.Lib.Operations.Sites
{
    class MehrnewsDumper : DumperBase, IDumper
    {
        public News ExtractNews(string html, Uri baseUri)
        {
            var text = "";
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var printButtton1 = htmlDoc.DocumentNode.GetElementsWithClass("div", "item-title")?.FirstOrDefault();
            var printButtton2 = printButtton1.GetElementsWithClass("h1", "title")?.FirstOrDefault();
            if (printButtton2 != null)
            {
                var printValue = printButtton2.SelectSingleNode("//a[contains(@href, '/news/')]").Attributes["href"].Value;
                var printUri = $"http://{baseUri.Host}{printValue}";
                var printHtml = Get(printUri);

                var printDoc = new HtmlDocument();
                printDoc.LoadHtml(printHtml);

                var body = printDoc.DocumentNode.GetElementsWithClass("div", "item-body")?.FirstOrDefault();
                var paragraphs = body.ChildNodes.Where(x => x.Name == "p");
                text = string.Join(Environment.NewLine, paragraphs.Select(x => x.InnerText.HtmlDecode().Trim()));

                if (text.IsEmpty())
                {
                    //Validate for trivia character
                    if (!body.InnerText.HtmlDecode().StartsWith("{$"))
                    {
                        text = body.InnerText.HtmlDecode().Trim();
                    }
                }
            }
            return new News { NewsBody = text };
        }



        public void RunAndSave()
        {
            EventBus.Notify("Mehrnews dumper running", "Info");

            var xml = Get(Constants.MehrnewsRss);
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
                    if (item.NewsExists())
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
                    news.SaveNewsInDatabase();

                }
                catch (Exception ex)
                {

                    EventBus.Notify(ex.Message + "continuing...", "Info");
                }



            }
            EventBus.Notify("Mehrnews dumper exiting", "Info");

        }


    }
}
