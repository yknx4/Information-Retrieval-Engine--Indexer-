using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Engine.Database;
using Engine.Tools;

namespace Engine.Model
{
    public class LogicalView
    {
        public const int GroupSize = Constants.GroupSize;

        public LogicalView(string rawInput)
        {
            //rawInput = StringTools.RemoveDiacritics(rawInput);
            //rawInput = WebUtility.HtmlDecode(rawInput);
            Title = String.Empty;
            IsFromUrl = false;
            Data = rawInput;
            //Title = HtmlTools.ExtractTitle(rawInput);
        }

        public LogicalView(Uri location)
        {
            SourceUri = location;
            Title = location.ToString();
            IsFromUrl = true;
        }

        public static int InsertedViewsCount { get; private set; }
        public static int ProcessingViewsCount { get; set; }
        public bool IsFromUrl { get; private set; }
        public bool IsInitialized { get; private set; }

        public bool IsProcessing
        {
            get { return _isProcessing; }
            set
            {
                if (value == _isProcessing) return;
                _isProcessing = value;
                if (_isProcessing)
                    ProcessingViewsCount++;
                else
                {
                    ProcessingViewsCount--;
                }
            }
        }

        public bool IsInserted
        {
            get { return _isInserted; }
            set
            {
                _isInserted = value;
                InsertedViewsCount++;
            }
        }

        public Uri SourceUri { get; private set; }
        public String Data { get; private set; }
        public int RetryNumber { get; set; }

        public int NumberOfKeywords
        {
            get { return _indexTermsCount.Count; }
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private readonly Dictionary<string, int> _indexTermsCount = new Dictionary<string, int>();
        private bool _isInserted;
        private bool _isProcessing;
        //Query Vector of ordered pairs (x,y), x = term, y = frecuency
        public Dictionary<string, int> IndexTermsCount
        {
            get { return _indexTermsCount; }
        }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public string OriginalSourceCode { get; set; }
        private void AddTerm(String term)
        {
            //term = term.ToLower();
            term = term.ToLowerInvariant();
            //term = term.Trim();
            if (IndexTermsCount.ContainsKey(term))
            {
                IndexTermsCount[term]++;
            }
            else
            {
                IndexTermsCount.Add(term, 1);
            }
        }

        public void Initialize()
        {
            if (IsInitialized) return;
            if (!MySqlDbConnection.AreConnectionsAvailable) return;
            try
            {
                if (IsFromUrl)
                {
                    using (var client = new WebClient())
                    {
                        client.Encoding = Encoding.UTF8;
                        OriginalSourceCode = client.DownloadString(SourceUri);
                    }
                    Data = OriginalSourceCode;
                    Data = WebUtility.HtmlDecode(Data);
                    Data = StringTools.RemoveDiacritics(Data);
                    Title = HtmlTools.ExtractTitle(Data);
                }
                IsProcessing = true;
                Data = HtmlTools.StripTagsCharArray(Data, true, true);
                Description = HtmlTools.GenerateDescription(Data);
                Data = StringTools.RemoveNonChar(Data);


                if (Title.Length > 0)
                    foreach (var term in DivideStringInGroups(StringTools.RemoveNonChar(Title), GroupSize))
                    {
                        AddTerm(term);
                        AddTerm(term);
                    }

                if (Data.Length > 0)
                    foreach (var term in DivideStringInGroups(Data, GroupSize))
                    {
                        AddTerm(term);
                    }
                IsProcessing = false;

                IsInitialized = true;
                EngineLogger.Log(this, Title + " initialized.");
            }
            catch (Exception)
            {
                IsInitialized = false;
                EngineLogger.Log(this, "Failed to initialize: "+this);
            }
        }

        private static IEnumerable<string> DivideStringInGroups(string source, int groupSize)
        {
            var sourceSize = source.Length;
            var numberOfGroups = sourceSize - (groupSize - 1);
            if (numberOfGroups < 1) return null;
            var finalArray = new String[numberOfGroups];
            var tmpCharArray = new Char[groupSize];
            var charArrayIndex = 0;
            var stringArrayIndex = 0;
            var lastSourceIndex = 0;
            for (var index = 0; index < sourceSize; index++)
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
                    index = lastSourceIndex - 1;
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