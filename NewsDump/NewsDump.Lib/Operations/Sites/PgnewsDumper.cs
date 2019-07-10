using HtmlAgilityPack;
using NewsDump.Lib.Model;
using NewsDump.Lib.Operations.Sites.Interface;
using NewsDump.Lib.Util;
using Olive;
using System;
using System.Linq;

namespace NewsDump.Lib.Operations.Sites
{
    class PgnewsDumper : DumperBase, IDumper
    {
        public News ExtractNews(string html, Uri baseUri)
        {
            var text = "";
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var printValue = htmlDoc.DocumentNode.SelectSingleNode("//a[contains(@href, 'template=print')]").Attributes["href"].Value;
            //This site is unstable. I've handled the !null value below so it may proceed only when printValue is not null, but it might still throw null exception. 
            if (printValue != null)
            {
                var printUri = $"http://{baseUri.Host}{printValue}";
                var printHtml = Get(printUri);

                var printDoc = new HtmlDocument();
                printDoc.LoadHtml(printHtml);

                var body = printDoc.DocumentNode.GetElementsWithClass("div", "body")?.FirstOrDefault();
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
            EventBus.Notify("Pgnews dumper running", "Info");

            var xml = Get(Constants.PgnewsRss);
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

            EventBus.Notify("Pgnews dumper exiting", "Info");

        }


    }
}
