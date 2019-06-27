using CodeHollow.FeedReader;
using NewsDump.Lib.Model;
using NewsDump.Lib.Operations.Sites.Interface;
using NewsDump.Lib.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsDump.Lib.Operations.Sites
{
    class IribDumper : DumperBase, IDumper
    {
        public News ExtractNews(string html)
        {
            throw new NotImplementedException();
        }

        

        public void RunAndSave()
        {
            var feed = GetFeed(Constants.IribRss);

            foreach (var item in feed.Items)
            {
                var html = GetHtml(item.Link);

                var news = ExtractNews(html);

                //Save to DB with the code I shown you before.
            }

        }
    }
}
