using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableFileGenerator
{
    public class DocumentStructure
    {
        private readonly Dictionary<int, int> _keywordDictionary = new Dictionary<int, int>();

        public void AddKeyword(int keywordId, int keywordCount)
        {
            _keywordDictionary.Add(keywordId,keywordCount);
        }

        public int GetKeywordCount(int keywordId)
        {
            return _keywordDictionary.ContainsKey(keywordId) ? _keywordDictionary[keywordId] : 0;
        }

        public Dictionary<int, int> GetRawData()
        {
            return _keywordDictionary;
        }

        public int Id { get; set; }
    }
}
