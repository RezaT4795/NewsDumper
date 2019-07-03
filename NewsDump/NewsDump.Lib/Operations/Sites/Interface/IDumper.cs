using NewsDump.Lib.Model;
using System;
using System.ServiceModel.Syndication;

namespace NewsDump.Lib.Operations.Sites.Interface
{
    interface IDumper
    {
        void RunAndSave();
        News ExtractNews(string html, Uri baseUri);
        News SetNewsFromFeed(News news, SyndicationItem feed);
    }
}
