using NewsDump.Lib.Data;
using NewsDump.Lib.Model;
using NewsDump.Lib.Util;
using NewsDump.UI.Utils;
using Olive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel.Syndication;

namespace NewsDump.Lib.Operations
{
    internal static class NewsOperation
    {
        static Repository<News> _repo = Repository.Of<News>();

        public static List<News> GetAllNews() => _repo.GetAll().ToList();

        public static List<News> Where(Expression<Func<News, bool>> exp) => _repo.FindAll(exp).ToList();

        public static void SaveNewsInDatabase(this News news)
        {
            if (Conf.Get<bool>("Hazf"))
            {
                var all = GetAllNews();
                if (all.AsParallel().None(x => StringCompare.IsPotentiallySimilar(x.NewsBody, news.NewsBody)))
                {
                    _repo.Add(news);
                }
            }
            else
            {
                _repo.Add(news);
            }


        }
        public static bool NewsExists(this SyndicationItem feed) => _repo.Exists(
            x => x.Link.ToLower() == feed.GetUri().ToString().ToLower()
            );
    }
}
