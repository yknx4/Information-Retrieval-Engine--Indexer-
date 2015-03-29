using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public static string TrimTag(string source, string tag)
        {
            var regexString = @"\<(?:[^:]+:)?TAG.*?\<\/(?:[^:]+:)?TAG\>";
            regexString = regexString.Replace("TAG", tag);
            var rRemScript = new Regex(regexString,RegexOptions.Singleline|RegexOptions.IgnoreCase); 
            return rRemScript.Replace(source, "");
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
