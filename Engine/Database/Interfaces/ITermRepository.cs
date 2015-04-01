using System;
using System.Collections.Generic;
using Engine.Model;

namespace Engine.Database.Interfaces
{
    public interface ITermRepository
    {
        Term Get(int id);
        Term Get(String value);
        IEnumerable<Term> GetAll();
        void Insert(String input);
        void InsertBatch(IEnumerable<String> input);

        void InsertBatch(Uri uri, IDictionary<string, int> dictionary);
    }
}
