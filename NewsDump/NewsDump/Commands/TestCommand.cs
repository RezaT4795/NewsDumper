using NewsDump.Lib.Model;
using NewsDump.Lib.Operations;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsDump.Commands
{
    static class TestCommand
    {
        public static void Run()
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

            news = TestOperation.GetAllNews();
            news.ForEach(n => Console.WriteLine(n.NewsTitle));

            Console.ReadLine();
        }
    }
}
