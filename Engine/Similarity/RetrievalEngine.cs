using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Database.Repositories;
using Engine.Model;

namespace Engine.Similarity
{
    public class RetrievalEngine
    {
        // ReSharper disable once UnusedParameter.Local
        private static double TermSimilarity(double termWeightOnDocument, double termWeightOnQuery = 0)
        {
//            var a = termWeightOnQuery*termWeightOnQuery;
//            var b = termWeightOnDocument*termWeightOnQuery*termWeightOnDocument*termWeightOnQuery;
//            b = Math.Sqrt(b);
//            return a/b;
            return Math.Abs(termWeightOnDocument);
        }
        //Dictionary with DocumentID and Similarity Sums
        readonly Dictionary<int,double> _finalSimilarity = new Dictionary<int, double>();
        private readonly LogicalView _query;

        public RetrievalEngine(string query)
        {
            _query = new LogicalView(query);
            _query.Initialize();          
        }

        public IEnumerable<KeyValuePair<int, double>> Retrieve()
        {
            foreach (var term in _query.IndexTermsCount)
            {
                var termid = MySqlTermRepository.GetTermId(term.Key);
                if(termid==-1)continue;
                
                var docs = InvertedListReader.GetDocumentsForIndex(termid);
                foreach (var doc in docs)
                {
                    var docid = doc.Item1;
                    var weight = doc.Item2;
                    if (_finalSimilarity.ContainsKey(docid))
                    {
                        _finalSimilarity[docid] += TermSimilarity(weight,1);
                    }
                    else
                    {
                        _finalSimilarity.Add(docid, TermSimilarity(weight, 1));
                    }
                }
                

            }
            return  _finalSimilarity.OrderBy(kvp => kvp.Value).Reverse();
        }
//        public int[] Retrieve(int max)
//        {
//
//        }
    }
}
