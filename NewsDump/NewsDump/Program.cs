using NewsDump.Lib.Model;
using NewsDump.Lib.Operations;
using System;

namespace NewsDump
{
    class Program
    {
        static void Main(string[] args)
        {
            var news = TestOperation.GetAllNews();
            foreach (var item in news)
            {
                Console.WriteLine(item.NewsTitle);
            }

            var newNews = new News
            {
                NewsTitle = "SurpriseMotherFucker"
            };
            TestOperation.SaveNews(newNews);

            news= TestOperation.GetAllNews();
            news.ForEach(n => Console.WriteLine(n.NewsTitle));
        }
    }
}
