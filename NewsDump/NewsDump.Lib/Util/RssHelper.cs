using CodeHollow.FeedReader;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsDump.Lib.Util
{
    static class RssHelper
    {
        public static Feed ReadRss(string uri)=> FeedReader.ReadAsync(uri).GetAwaiter().GetResult();
    }
}
