using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace Engine.Tools
{
    public abstract class HtmlTools
    {
        /// <summary>
        /// Remove HTML tags from string using char array.
        /// </summary>

        public static string ExtractTextBetweenTag(string source, string tag)
        {
            tag = tag.Trim();
            var openTag = "<" + tag + ">";
            var closeTag = "</" + tag + ">";

            if(source.Contains(openTag)&&source.Contains(closeTag)) return(source.Substring(source.IndexOf(openTag, StringComparison.Ordinal) + openTag.Length, source.IndexOf(closeTag, StringComparison.Ordinal) - source.IndexOf(openTag, StringComparison.Ordinal) - openTag.Length));
            return "Unknown Title";
        }

        public static string ExtractTitle(string source)
        {
           return ExtractTextBetweenTag(source,"title");
        }

        private static readonly Dictionary<string, Regex> regexForTag = new Dictionary<string, Regex>();

        
        private static Regex GetRegexForTag(string tag)
        {
            const string regexString = @"\<(?:[^:]+:)?TAG.*?\<\/(?:[^:]+:)?TAG\>";
            if (regexForTag.ContainsKey(tag))
            {
                return regexForTag[tag];
            }
            var regexFinal = regexString.Replace("TAG", tag);
            var rg = new Regex(regexFinal, RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled); 
            regexForTag.Add(tag,rg);
            return rg;
        }

        public static string TrimTag(string source, string tag)
        {            
            return GetRegexForTag(tag).Replace(source, "");
        }
        public static string StripTagsCharArray(string source, bool removeScripts, bool removeStyles)
        {
            source = WebUtility.HtmlDecode(source);
           if(removeScripts) source = TrimTag(source, "script");
            if(removeStyles) source = TrimTag(source, "style");
            var array = new char[source.Length];
            var arrayIndex = 0;
            var inside = false;

            foreach (var @let in source)
            {
                switch (@let)
                {
                    case '<':
                        inside = true;
                        continue;
                    case '>':
                        inside = false;
                        continue;
                }
                if (inside) continue;
                array[arrayIndex] = @let;
                arrayIndex++;
            }
            var result = new string(array, 0, arrayIndex);           
            return result.Trim();
        }
    }
}
