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


        public static List<News> GetAllNews(Repository<News> _repo) => _repo.GetAll().ToList();

        public static List<News> Where(Repository<News> _repo, Expression<Func<News, bool>> exp) => _repo.FindAll(exp).ToList();

        public static void SaveNewsInDatabase(this News news, Repository<News> _repo)
        {
            var conf = Conf.Get<bool>("Hazf");
            if (conf)
            {
                var all = _repo.FindAll(x=>x.PublishDate>news.PublishDate.AddHours(-10) );
                if (all.AsParallel().None(x => StringCompare.IsPotentiallySimilar(x, news)))
                {
                    _repo.Add(news);
                }
                else
                {
                    
                }
            }
            else
            {
                _repo.Add(news);
            }


        }
        public static bool NewsExists(this SyndicationItem feed, Repository<News> _repo) => _repo.Exists(
            x => x.Link.ToLower() == feed.GetUri().ToString().ToLower()
            );
    }
}
