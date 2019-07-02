using NewsDump.Lib.Model;
using NewsDump.Lib.Operations.Sites.Interface;
using NewsDump.Lib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;

namespace NewsDump.Lib.Operations.Sites
{
    class MehrnewsDumper : DumperBase, IDumper
    {
        public News ExtractNews(string html, Uri baseUri)
        {
            throw new NotImplementedException();
        }

        

        public void RunAndSave()
        {
            
        }

        public News SetNewsFromFeed(News news, SyndicationItem feed)
        {
            throw new NotImplementedException();
        }
    }
}
