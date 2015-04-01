using System;
using System.Collections.Generic;
using Engine.Model;

namespace Engine.Database.Interfaces
{
    public interface IWeightRepository
    {
//        Weight Get(int id);
//        Weight Get(int termId, int documentId);
//        IEnumerable<Weight> GetAll(int documentId);
//        void Insert(int documentId, int termId, int weight);
//        void Insert(int documentId, string termValue, int weight);
//        void Insert(int documentId, Term term, int weight);
        void InsertBatch(int documentId, IDictionary<String,int> values);
        void InsertBatch(Uri documentUri, IDictionary<String, int> values);
        void Insert(int documenId, KeyValuePair<string, int> value);
        void Insert(Uri documentUri, KeyValuePair<string, int> value);
    }
}