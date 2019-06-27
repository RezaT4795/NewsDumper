using NewsDump.Lib.Data;
using NewsDump.Lib.Model;
using NewsDump.Lib.Operations.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewsDump.Lib.Operations
{
    public static class TestOperation
    {
        public static List<News> GetAllNews() => Repository.Of<News>().GetAll().ToList();

        public static void SaveNews(News news)
        {
            var repo = Repository.Of<News>();
            repo.Add(news);
        }

        public static void Test()
        {
            var irib = new IribDumper();
            irib.RunAndSave();
        }
    }
}
