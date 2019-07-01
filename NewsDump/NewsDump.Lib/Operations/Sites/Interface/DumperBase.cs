using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;

namespace NewsDump.Lib.Operations.Sites.Interface
{
    abstract class DumperBase
    {
        internal SyndicationFeed GetFeed(string content) => Util.RssHelper.ReadRss(content);

        internal string Get(string uri)
        {
            string responseString = string.Empty;
            string url = uri;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                responseString = reader.ReadToEnd();
            }

            return responseString;
        }
    }
}
