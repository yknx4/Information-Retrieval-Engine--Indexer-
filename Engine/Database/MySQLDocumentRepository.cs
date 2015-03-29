using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Model;
using MySql.Data.MySqlClient;

namespace Engine.Database
{
    public class MySqlDocumentRepository : IDocumentRepository
    {
        public MySqlDocumentRepository()
        {
            
        }

        public LogicalView Get(int id)
        {
            throw new NotImplementedException();
        }

        public LogicalView Get(string url)
        {
            throw new NotImplementedException();
        }

        public void Insert(LogicalView input)
        {
            if(!input.IsInitialized) input.Initialize();
            using (var conn = new MySqlConnection(Constants.ConnectionString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = Constants.Queries.InsertDocumentQuery;
                cmd.Prepare();
                cmd.Parameters.AddWithValue(Constants.Parameters.Title, input.Title);
                cmd.Parameters.AddWithValue(Constants.Parameters.Url, input.SourceUri);
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertBatch(IEnumerable<LogicalView> input)
        {
            throw new NotImplementedException();
        }
    }
}
