using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewsDump.Lib.Model;
using Olive;

namespace NewsDump.Lib.Util
{
    public static class StringCompare
    {
        public static bool IsPotentiallySimilar(this News first, News second)
        {
            var percentage = GetPercentageOfSimilarity(first.NewsBody, second.NewsBody);
            var isSimilar = percentage > 90;
            if (isSimilar)
            {
                EventBus.Notify($"Similarity {percentage}% {first.NewsTitle} is like {second.NewsTitle}", "info");
            }
            return isSimilar;
        }
        static int GetPercentageOfSimilarity(string first, string second)
        {

            var firstWords = first.ExtractWords().ToList();
            var secondWords = second.ExtractWords().ToList();

            if (firstWords.Count < 80 || secondWords.Count < 80)
            {
                return 0;
            }

            var firstInSecond = firstWords.Intersect(secondWords).ToList();

            return (int)Math.Round((double)(100 * firstInSecond.Count) / firstWords.Count);


        }
        static string[] ExtractWords(this string text)
        {
            return text.Split('.', '\n', ',', ' ', '!', '?', '\"', ';', ':', '/', '\\')
                .Trim().ToLower().Distinct()
                .Except(x => x.ToCharArray().Any(v => v.IsDigit() || v == '\''))
                .Where(v => v.Length > 1)
                .ToArray();
        }
    }
}
