using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Olive;

namespace NewsDump.Lib.Util
{
    public static class StringCompare
    {
        public static bool IsPotentiallySimilar(this string first, string second)
        {
            var percentage = GetPercentageOfSimilarity(first, second);
            var isSimilar = percentage > 80;
#if DEBUG
            if (isSimilar)
            {
                EventBus.Notify($"Similarity {percentage}%", "info");
            }
#endif
            return isSimilar;
        }
        static int GetPercentageOfSimilarity(string first, string second)
        {
            var firstWords = first.ExtractWords().ToList();
            var secondWords = second.ExtractWords().ToList();

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
