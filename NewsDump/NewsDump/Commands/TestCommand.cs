using ManyConsole;
using NewsDump.Lib.Model;
using NewsDump.Lib.Operations;
using Olive;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsDump.Commands
{
    class TestCommand : ConsoleCommand
    {
        public string Additional { get; set; }
        public TestCommand()
        {
            IsCommand("Test", "Runs a test command");
            HasOption("a|additional=", "Adds an additional shit to the added thing", t => Additional = t);
        }
        public override int Run(string[] remainingArguments)
        {

            //TestOperation.Test();

            var news = TestOperation.GetAllNews();
            foreach (var item in news)
            {
                Console.WriteLine(item.NewsTitle);
            }

            var newNews = new News
            {
                NewsTitle = $"SurpriseMotherFucker {Additional}"
            };
            TestOperation.SaveNews(newNews);

            news = TestOperation.GetAllNews();
            news.ForEach(n => Console.WriteLine(n.NewsTitle));

            return 1;
        }
    }
}
