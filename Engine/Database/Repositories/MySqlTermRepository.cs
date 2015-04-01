using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Timers;
using Engine.Database.Interfaces;
using Engine.Model;
using Engine.Tools;
using MySql.Data.MySqlClient;

namespace Engine.Database.Repositories
{
    public class MySqlTermRepository : ITermRepository
    {
        private const string ClassName = "MySqlTermRepository";
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private static readonly Timer CommitTimer;
        private static readonly ConcurrentQueue<String> QueryQueue;


        static MySqlTermRepository()
        {
            var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
            if (declaringType != null)
                EngineLogger.Log(declaringType.Name,"Logger initalized");
            QueryQueue = new ConcurrentQueue<string>();
            CommitTimer = new Timer { SynchronizingObject = null, Interval = Constants.CommitInterval };
            CommitTimer.Elapsed += CommitQuery;
            CommitTimer.Enabled = true;
        }

        private static void CommitQuery(object source, ElapsedEventArgs e)
        {
            
            if (QueryQueue.IsEmpty)
            {
                MySqlRepositoriesSync.IsTermRepositoryWorking = false;
                return;
            }
            else
            {
                EngineLogger.Log(ClassName, "Committing.");
            }
            if (MySqlRepositoriesSync.IsDocumentRepositoryWorking)
            {
                EngineLogger.Log(ClassName,"Document repository is working. Delaying");
                return;
            }
            MySqlRepositoriesSync.IsTermRepositoryWorking = true;
            var queryBuilder = new StringBuilder();
            string queueItem;
            var queueCount = 0;
            while (QueryQueue.TryDequeue(out queueItem)||queueCount == Constants.QueriesPerTransaction)
            {
                queryBuilder.Append(queueItem);
                queueCount++;
            }
            using (var conn = new MySqlConnection(Constants.ConnectionString))
            using (var cmd = conn.CreateCommand())
            {
                var finalQuery = queryBuilder.ToString();
                EngineLogger.Log(ClassName, "Query to process: "+finalQuery);
                conn.Open();
                cmd.CommandText = finalQuery;
                cmd.Prepare();
                
                cmd.CommandTimeout = Constants.MaxTimeout;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    EngineLogger.Log(ClassName,"Exception on query! Check it! "+ex.ToString());
                }
            }
            if (QueryQueue.IsEmpty)
            {
                EngineLogger.Log(ClassName, "Work done");
                MySqlRepositoriesSync.IsTermRepositoryWorking = false;
            }
            else
            {
                EngineLogger.Log(ClassName, "Elements left: " + QueryQueue.Count);
            }
        }


        public Term Get(int id)
        {
            throw new NotImplementedException();
        }

        public Term Get(string value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Term> GetAll()
        {
            var results = new List<Term>();
            using (var conn = new MySqlConnection(Constants.ConnectionString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = Constants.Queries.SelectAllTermsQuery;
                cmd.CommandTimeout = Constants.MaxTimeout;
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var tmpTerm = new Term()
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetString(1)
                    };
                    results.Add(tmpTerm);
                }
            }
            return results;
        }

        public void Insert(string input)
        {
            var localQuery = Constants.Queries.InsertTermQuery;
            localQuery = GenericTools.FillParameter(localQuery, Constants.Parameters.Value, input.Replace("'", "''"));
            QueryQueue.Enqueue(localQuery);
        }

        public void InsertBatch(IEnumerable<string> input)
        {

            var termValues = input as string[] ?? input.ToArray();
            EngineLogger.Log(this, "Added " + termValues.Length + " elements to queue.");
            foreach (var termValue in termValues)
            {
                Insert(termValue);
            }           
        }

        public void InsertBatch(Uri uri, IDictionary<string, int> dictionary)
        {
            InsertBatch(MySqlDocumentRepository.GetId(uri),dictionary);
        }

        public void InsertBatch(int documentId, IDictionary<string, int> values)
        {
            
            EngineLogger.Log(this, "Added " + values.Count + " elements to queue.");
            foreach (var termValue in values)
            {
                Insert(documentId,termValue);
            }
        }
        private readonly IWeightRepository _weightRepository = new MySqlWeightRepository();
        public void Insert(int documenId, KeyValuePair<string,int> value)
        {
            var localQuery = Constants.Queries.InsertTermQuery;
            localQuery = GenericTools.FillParameter(localQuery, Constants.Parameters.Value, value.Key.Replace("'", "''"));
            QueryQueue.Enqueue(localQuery);
            _weightRepository.Insert(documenId,value);
        }
    }
}