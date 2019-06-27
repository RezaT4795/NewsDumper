using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace NewsDump.Lib.Util
{
    static class RssHelper
    {
        public static SyndicationFeed ReadRss(string xml)
        {
            using (var reader = XmlReader.Create(new StringReader(xml)))
            {
                return SyndicationFeed.Load(reader);
            }
            
        }
    }
}
