using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace SimbirSoftTestTask.Model
{
    public static class Model
    {

        private static readonly char[] Separators = {
            ' ', ',', '.', '!', '?','"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t', '-'
        };
        
        public static Dictionary<string, int> GetStatistic(string url)
        {
            string htmlString;
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    htmlString = reader.ReadToEnd();
                }
            }

            response.Close();

            string[] words = GetPlainTextFromHtml(htmlString).Split(Separators, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, int> statistic = new Dictionary<string, int>();

            foreach (string word in words)
            {
                if (statistic.TryGetValue(word, out int count))
                {
                    _ = statistic.Remove(word);
                    statistic.Add(word, ++count);
                }
                else
                {
                    statistic.Add(word, 1);
                }
            }

            return statistic;
        }

        private static string GetPlainTextFromHtml(string htmlString)
        {
            Regex regexJsCss = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex regexHtmlTags = new Regex("(<.*?>)+", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            htmlString = regexJsCss.Replace(htmlString, string.Empty);
            htmlString = regexHtmlTags.Replace(htmlString, string.Empty);
            htmlString = Regex.Replace(htmlString, "\\s\\s+", " ");
            htmlString = htmlString.Replace("&nbsp;", string.Empty);

            return htmlString;
        }
    }
}