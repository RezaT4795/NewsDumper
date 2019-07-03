using NewsDump.Lib.Data;
using NewsDump.Lib.Model;
using NewsDump.Lib.Util;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;

namespace NewsDump.Lib.Operations
{
    internal static class NewsOperation
    {
        static Repository<News> _repo = Repository.Of<News>();

        public static List<News> GetAllNews() => _repo.GetAll().ToList();

        public static void SaveNewsInDatabase(this News news) => _repo.Add(news);
        public static bool NewsExists(this SyndicationItem feed) => _repo.Exists(
            x => x.Link.ToLower() == feed.GetUri().ToString().ToLower()
            );
    }
}
