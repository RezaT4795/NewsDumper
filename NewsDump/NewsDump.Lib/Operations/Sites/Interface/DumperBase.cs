using CodeHollow.FeedReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace NewsDump.Lib.Operations.Sites.Interface
{
    abstract class DumperBase
    {
        public Feed GetFeed(string uri) => Util.RssHelper.ReadRss(uri);

        public string GetHtml(string uri)
        {
            string html = string.Empty;
            string url = uri;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            return html;
        }
    }
}
