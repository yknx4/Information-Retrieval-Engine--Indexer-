using System;
using System.Collections.Generic;
using Engine.Model;

namespace Engine.Database.Interfaces
{
    public interface IDocumentRepository
    {
        LogicalView Get(int id);
        LogicalView Get(String url);
        int GetId(Uri url);
        void Insert(LogicalView input);
        void InsertBatch(IEnumerable<LogicalView> input);
    }
}
