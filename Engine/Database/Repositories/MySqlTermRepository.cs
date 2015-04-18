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
        private static readonly ConcurrentQueue<Tuple<string, string>> QueryQueue;


        static MySqlTermRepository()
        {
            var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
            if (declaringType != null)
                EngineLogger.Log(declaringType.Name,"Logger initalized");
            QueryQueue = new ConcurrentQueue<Tuple<string, string>>();
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
            MySqlRepositoriesSync.IsTermRepositoryWorking = true;
            var queryBuilder = new StringBuilder();
            var queuedTerms = new List<String>();
            Tuple<string, string> queueItem;
            var queueCount = 0;
            while (QueryQueue.TryDequeue(out queueItem)&&queueCount < Constants.QueriesPerTransaction)
            {
                if (queueItem == null)
                {
                    EngineLogger.Log(ClassName, "WTF!?");
                    continue;
                }
                queryBuilder.Append(GenericTools.NumberStatementParameter(queueItem.Item1,Constants.Parameters.Value,queueCount));
                queuedTerms.Add(queueItem.Item2);
                queueCount++;
            }
            //using (var conn = new MySqlDbConnection(Constants.ConnectionString))
            var conn = MySqlDbConnection.GetConnection();
            using (var cmd = conn.CreateCommand())
            {
                var finalQuery = queryBuilder.ToString();
                EngineLogger.Log(ClassName, "Query to process: "+finalQuery);
                //conn.Open();
                cmd.CommandText = finalQuery;
                cmd.Prepare();
                for (var i = 0; i < queueCount; i++)
                {
                    cmd.Parameters.AddWithValue(Constants.Parameters.Value + i, queuedTerms[i]);
                }
                cmd.CommandTimeout = Constants.MaxTimeout;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    EngineLogger.Log(ClassName,"Exception on query! Check it! "+ex);
                }
            }
            MySqlDbConnection.ReturnConnection(conn);
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
            //using (var conn = new MySqlDbConnection(Constants.ConnectionString))
            var conn = MySqlDbConnection.GetConnection();
            using (var cmd = conn.CreateCommand())
            {
                //conn.Open();
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
            MySqlDbConnection.ReturnConnection(conn);
            return results;
        }

        public void Insert(string input)
        {
            var localQuery = Constants.Queries.InsertTermQuery;
            //localQuery = GenericTools.FillParameter(localQuery, Constants.Parameters.Value, input.Replace("'", "''"));
            QueryQueue.Enqueue(new Tuple<string, string>(localQuery,input));
            
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
            //USED BEFORE, LEFT IN CASE OF DEBUG
            //localQuery = GenericTools.FillParameter(localQuery, Constants.Parameters.Value, value.Key.Replace("'", "''"));
            QueryQueue.Enqueue(new Tuple<string,string>(localQuery,value.Key));
            _weightRepository.Insert(documenId,value);
        }
    }
}