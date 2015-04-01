using System.Text;
using System.Text.RegularExpressions;

namespace Engine.Tools
{
    public abstract class StringTools
    {
        // \p{Mn} or \p{Non_Spacing_Mark}: 
        //   a character intended to be combined with another 
        //   character without taking up extra space 
        //   (e.g. accents, umlauts, etc.). 
        private readonly static Regex NonSpacingMarkRegex =
            new Regex(@"\p{Mn}", RegexOptions.Compiled);

        public static string RemoveDiacritics(string text)
        {
            if (text == null)
                return string.Empty;

            var normalizedText =
                text.Normalize(NormalizationForm.FormD);

            return NonSpacingMarkRegex.Replace(normalizedText, string.Empty);
        }

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
