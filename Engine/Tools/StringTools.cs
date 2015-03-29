using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Engine.Tools
{
    public abstract class StringTools
    {
        public static string RemoveNonChar(string source)
        {
            var array = new char[source.Length];
            var arrayIndex = 0;

            foreach (var @let in source)
            {
                switch (@let)
                {
                    case '\n':
                        continue;
                    case ' ':
                        continue;
                    case '\r':
                        continue;
                    case '\t':
                        continue;
                    default:                       
                        array[arrayIndex] = @let;
                        arrayIndex++;
                        continue;
                }

            }
            var result = new string(array, 0, arrayIndex);
            result = result.Replace("\n ", "");
            return result.Trim();
        }
        public static string RemoveSpaces(string source)
        {
            return source.Replace(" ", "");
        }
        public static string ReplaceTabs(string source)
        {
            return source.Replace("\t", " ");
        }
        public static string TrimSpaces(string source)
        {
            const RegexOptions options = RegexOptions.None;
            var regex = new Regex(@"[ ]{2,}", options);
           return regex.Replace(source, @" ");
        }
        public static string TrimNewLines(string source)
        {
            var array = new char[source.Length];
            var arrayIndex = 0;
            var inside = false;

            foreach (var @let in source)
            {
                switch (@let)
                {
                    case '\n':
                        inside = true;
                        continue;
                    default:
                        if (inside)
                        {
                            array[arrayIndex] = '\n';
                            arrayIndex++;
                        }
                        array[arrayIndex] = @let;
                        arrayIndex++;
                        inside = false;
                        continue;
                }
                
            }
            var result = new string(array, 0, arrayIndex);
            result = result.Replace("\n ", "");
            return result.Trim();
        }
    }
}
