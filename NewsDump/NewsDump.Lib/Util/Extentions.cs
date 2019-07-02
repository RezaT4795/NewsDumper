using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ServiceModel.Syndication;
using Olive;

namespace NewsDump.Lib.Util
{
    public static class Extentions
    {
        public static IEnumerable<HtmlNode> GetElementsWithClass(this HtmlNode doc, string tag, params string[] classNames)
        {

            var exprs = new Regex[classNames.Length];
            for (int i = 0; i < exprs.Length; i++)
            {
                exprs[i] = new Regex("\\b" + Regex.Escape(classNames[i]) + "\\b", RegexOptions.Compiled);
            }

            return doc
                .Descendants()
                .Where(n => n.NodeType == HtmlNodeType.Element)
                .Where(e =>
                   e.Name == tag &&
                   exprs.All(r =>
                      r.IsMatch(e.GetAttributeValue("class", ""))
                   )
                );
        }

        public static Uri GetUri(this SyndicationItem item) => item.Links?.FirstOrDefault().Uri;

        public static IEnumerable<string>GetItemsWithinQuotes(this string str)
        {
            return from Match match in Regex.Matches(str, "\"([^\"]*)\"")
                         select match.ToString().ToLower().Remove("\"");
        }

        public static string Take (this string str , int start,int length)
        {
            if (length+start>str.Length)
            {
                return str;
            }
            return str.Substring(start, length);
        }
    }
}
