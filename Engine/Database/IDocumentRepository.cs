using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Model;

namespace Engine.Database
{
    public interface IDocumentRepository
    {
        LogicalView Get(int id);
        LogicalView Get(String url);
        void Insert(LogicalView input);
        void InsertBatch(IEnumerable<LogicalView> input);
    }
}
