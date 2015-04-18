using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using Engine.Tools;
using MySql.Data.MySqlClient;

namespace Engine.Model
{
    public class LogicalView
    {
        public bool IsFromUrl { get; private set; }
        public bool IsInitialized { get; private set; }
        public Uri SourceUri { get; private set; }

        public LogicalView(string rawInput)
        {
            rawInput = StringTools.RemoveDiacritics(rawInput);
            rawInput = WebUtility.HtmlDecode(rawInput);
            _data = rawInput;
            Title = HtmlTools.ExtractTitle(rawInput);
            
        }

        public LogicalView(Uri location)
        {
            SourceUri = location;
            Title = location.ToString();
            IsFromUrl = true;           
        }

        private String _data;

        public String Data
        {
            get { return _data; }
        }
        [DefaultValue(3)]
        public int GroupSize { get;  set; }

        public int NumberOfKeywords
        {
            get { return _indexTermsCount.Count; }
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        Dictionary<string, int> _indexTermsCount = new Dictionary<string, int>();
        public Dictionary<string, int> IndexTermsCount { get { return _indexTermsCount; } }

        public string Title { get; private set; }

        private void AddTerm(String term)
        {
            term = term.ToLower();
            term = term.ToLowerInvariant();
            term = term.Trim();
            if (IndexTermsCount.ContainsKey(term))
            {
                IndexTermsCount[term]++;
            }
            else
            {
                IndexTermsCount.Add(term,1);
            }
        }

        public void Initialize()
        {
            if(IsInitialized) return;
            if (IsFromUrl)
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    _data = client.DownloadString(SourceUri);
                   
                }
                _data = StringTools.RemoveDiacritics(_data);
                _data = WebUtility.HtmlDecode(_data);
                Title = HtmlTools.ExtractTitle(_data);
            }

            _data = HtmlTools.StripTagsCharArray(_data, true, true);
            _data = StringTools.RemoveNonChar(_data);

            if (GroupSize <= 0) GroupSize = 3;
            if(Title.Length>0)
            foreach (var term in DivideStringInGroups(Title,GroupSize))
            {
                if(term.Length==GroupSize) AddTerm(term);
            }

            if(Data.Length>0)
            foreach (var term in DivideStringInGroups(_data, GroupSize))
            {
                if (term.Length == GroupSize) AddTerm(term);
            }

            IsInitialized = true;
            EngineLogger.Log(this, Title+" initialized.");
        }

        private static IEnumerable<string> DivideStringInGroups(string source, int groupSize)
        {
            var numberOfGroups = source.Length-groupSize;
            if (numberOfGroups < 0) return null;
            var finalArray = new String[numberOfGroups];
            var tmpCharArray = new Char[groupSize];
            var charArrayIndex = 0;
            var stringArrayIndex = 0;
            var lastSourceIndex = 0;
            for (var index = 0; index < source.Count(); index++)
            {
                var currentChar = source[index];
                if (charArrayIndex == 0)
                {
                    tmpCharArray = new Char[groupSize];
                }
                tmpCharArray[charArrayIndex] = currentChar;
                charArrayIndex++;
                if (charArrayIndex == groupSize)
                {
                    finalArray[stringArrayIndex] = new String(tmpCharArray);
                    stringArrayIndex++;
                    lastSourceIndex++;
                    charArrayIndex = 0;
                    index = lastSourceIndex-1;
                    if (stringArrayIndex == numberOfGroups)
                        break;
                }
            }

            return finalArray;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
