using NewsDump.Lib.Model;
using NewsDump.Lib.Util;
using System.IO;
using System.Net;
using System.ServiceModel.Syndication;

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

        public News SetNewsFromFeed(News news, SyndicationItem feed)
        {
            var uri = feed.GetUri();
            if (feed.Title != null)
            {
                news.NewsTitle = feed.Title.Text.Trim();
            }

            news.Link = uri.ToString();

            if (feed.PublishDate != null)
            {
                news.PublishDate = feed.PublishDate.DateTime;
            }

            news.SiteName = uri.Host;

            if (feed.Summary != null)
            {
                news.NewsIntro = feed.Summary.Text.Trim();
            }

            return news;
        }
    }
}
