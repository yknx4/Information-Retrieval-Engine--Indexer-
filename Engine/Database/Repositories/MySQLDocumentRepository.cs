using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Engine.Database.Interfaces;
using Engine.Model;
using MySql.Data.MySqlClient;

namespace Engine.Database.Repositories
{
    public class MySqlDocumentRepository : IDocumentRepository
    {
        public LogicalView Get(int id)
        {
            throw new NotImplementedException();
        }

        public LogicalView Get(string url)
        {
            throw new NotImplementedException();
        }

        int IDocumentRepository.GetId(Uri url)
        {
            return GetId(url);
        }

        public static int GetId(Uri url)
        {
            var result = -1;
            using (var conn = new MySqlConnection(Constants.ConnectionString))
            using (var cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = Constants.Queries.SelectDocumentIdFromUrlQuery;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue(Constants.Parameters.Url, url.ToString());
                    result = int.Parse(cmd.ExecuteScalar().ToString());
                    
                    // other codes
                }
                catch (MySqlException ex)
                {
                    if (ex.ToString() != string.Empty) result = -1;
                    // do somthing with the exception
                    // don't hide it
                }          
            }
            return result;
        }

        public void Insert(LogicalView input)
        {
            if (!input.IsInitialized) input.Initialize();
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
            ITermRepository termRepository = new MySqlTermRepository();
            var keys = input.IndexTermsCount.Keys.ToArray();
            termRepository.InsertBatch(input.SourceUri,input.IndexTermsCount);
            Thread.Sleep(500);    
            //IWeightRepository weightRepository = new MySqlWeightRepository();
            //weightRepository.InsertBatch(input.SourceUri,input.IndexTermsCount);
        }

        public void InsertBatch(IEnumerable<LogicalView> input)
        {
            var queryBuilder = new StringBuilder();
            var rowCount = 0;
            var logicalViews = input as LogicalView[] ?? input.ToArray();
            foreach (var logicalView in logicalViews)
            {
                if (!logicalView.IsInitialized) logicalView.Initialize();
                var localQuery = Constants.Queries.InsertDocumentQuery;
                localQuery = localQuery.Replace(Constants.Parameters.Title,
                    Constants.Parameters.Title + rowCount.ToString());
                localQuery = localQuery.Replace(Constants.Parameters.Url,
                    Constants.Parameters.Url + rowCount.ToString());
                rowCount++;
                queryBuilder.Append(localQuery);
            }
            using (var conn = new MySqlConnection(Constants.ConnectionString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = queryBuilder.ToString();
                cmd.Prepare();
                rowCount = 0;
                foreach (var logicalView in logicalViews)
                {
                    cmd.Parameters.AddWithValue(Constants.Parameters.Title + rowCount.ToString(), logicalView.Title);
                    cmd.Parameters.AddWithValue(Constants.Parameters.Url + rowCount.ToString(), logicalView.SourceUri);
                    rowCount++;
                }
                cmd.ExecuteNonQuery();
                foreach (var logicalView in logicalViews)
                {
                    ITermRepository termRepository = new MySqlTermRepository();
                    var keys = logicalView.IndexTermsCount.Keys.ToArray();
                    termRepository.InsertBatch(keys);
                    IWeightRepository weightRepository = new MySqlWeightRepository();
                    weightRepository.InsertBatch(logicalView.SourceUri, logicalView.IndexTermsCount);
                }
            }

        }
    }
}
