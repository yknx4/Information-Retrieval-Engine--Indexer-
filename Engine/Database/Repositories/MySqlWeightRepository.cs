using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Timers;
using Engine.Database.Interfaces;
using Engine.Tools;
using MySql.Data.MySqlClient;
using Timer = System.Timers.Timer;

namespace Engine.Database.Repositories
{
    public class MySqlWeightRepository : IWeightRepository
    {
        private static readonly Dictionary<String, int> TermsIdDictionary = new Dictionary<string, int>();

        private static int GetTermId(string value)
        {
            var id=-1;
            value = value.ToLower();
            value = value.ToLowerInvariant();
            value = StringTools.RemoveDiacritics(value);
            var roundCount = 0;
            var found = false;
            while (roundCount<Constants.TermIdMaximumTries && !found)
            {
                roundCount++;
                if (!TermsIdDictionary.TryGetValue(value, out id))
                {
                    //EngineLogger.Log(ClassName, "Couldn't find id for " + value + " in cache. Trying on DB.");
                    id = -1;
                }
                else
                {
                    found = true;
                }
                if (id < 0)
                {
                    id = GetTermId(value, true);
                }
                if (id > 0)
                {
                    found = true;
                    continue;
                }
                
                
            }
            //if (id == -1) EngineLogger.Log(ClassName, "Couldn't find id for " + value + " something is wrong.");
            return id;
        }
        public static int GetTermId(string value, bool lol)
        {
            var result = -1;
            using (var conn = new MySqlConnection(Constants.ConnectionString))
            using (var cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = Constants.Queries.SelectTermIdFromUrlQuery;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue(Constants.Parameters.Value, value);
                    result = int.Parse(cmd.ExecuteScalar().ToString());
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

        static MySqlWeightRepository()
        {
            var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
            if (declaringType != null)
                EngineLogger.Log(declaringType.Name, "Logger initalized");
            QueryQueue = new ConcurrentQueue<string>();
            RetryQueue = new ConcurrentQueue<Tuple<int, KeyValuePair<string, int>>>();
            CommitTimer = new Timer { SynchronizingObject = null, Interval = Constants.CommitInterval };
            CommitTimer.Elapsed += CommitQuery;
            CommitTimer.Start();
            RetryTimer = new Timer { SynchronizingObject = null, Interval = Constants.RetryInterval };
            RetryTimer.Elapsed += RetryQuery;
            UpdateTerms();
        }

        private static void RetryQuery(object sender, ElapsedEventArgs e)
        {
            if (RetryQueue.IsEmpty)
            {
                EngineLogger.Log(ClassName,"No more items in retry queue.");
                RetryTimer.Stop();
                return;
            }
            var queueCount = 0;
            while (!RetryQueue.IsEmpty || queueCount == Constants.QueriesPerTransaction)
            {
                queueCount++;
                Tuple<int, KeyValuePair<string, int>> item;
                if (RetryQueue.TryDequeue(out item))
                {
                    new MySqlWeightRepository().Insert(item.Item1, item.Item2);
                }
            }
            
            
        }

        public MySqlWeightRepository()
        {
            //EngineLogger.Log(ClassName, "MySqlWeightRepository initialized");



        }

        private static volatile bool _updating;

        private static void UpdateTerms()
        {
            if (_updating)
            {
                while (_updating)
                {
                    
                }
                return;
            }
            _updating = true;
            EngineLogger.Log(ClassName, "Updating Terms");
            var terms = new MySqlTermRepository().GetAll();
            foreach (var term in terms)
            {
                if (TermsIdDictionary.ContainsKey(term.Value)) continue;
                TermsIdDictionary.Add(term.Value, term.Id);
            }
            _updating = false;
        }

        private const string ClassName = "MySqlWeightRepository";
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private static readonly Timer CommitTimer;
        private static readonly Timer RetryTimer;
        private static readonly ConcurrentQueue<String> QueryQueue;
        private static readonly ConcurrentQueue<Tuple<int, KeyValuePair<string, int>>> RetryQueue;

        private static void CommitQuery(object source, ElapsedEventArgs e)
        {
            
            if (QueryQueue.IsEmpty)
            {

                MySqlRepositoriesSync.IsWeightRepositoryWorking = false;
                return;
            }
            UpdateTerms();
            if (MySqlRepositoriesSync.IsDocumentRepositoryWorking || MySqlRepositoriesSync.IsTermRepositoryWorking)
            {
                EngineLogger.Log(ClassName, "Document repository or Term Repository is working. Delaying");
                return;
            }
            MySqlRepositoriesSync.IsWeightRepositoryWorking = true;
            var queryBuilder = new StringBuilder();
            string queueItem;
            var queueCount = 0;
            while (QueryQueue.TryDequeue(out queueItem) || queueCount == Constants.QueriesPerTransaction)
            {
                queryBuilder.Append(queueItem);
                queueCount++;
            }
            using (var conn = new MySqlConnection(Constants.ConnectionString))
            using (var cmd = conn.CreateCommand())
            {
                var finalQuery = queryBuilder.ToString();
                EngineLogger.Log(ClassName, "Query to process: " + finalQuery);
                conn.Open();
                cmd.CommandText = finalQuery;
                cmd.CommandTimeout = Constants.MaxTimeout;
                cmd.ExecuteNonQuery();
            }
            if (QueryQueue.IsEmpty)
            {
                MySqlRepositoriesSync.IsWeightRepositoryWorking = false;
            }
            else
            {
                EngineLogger.Log(ClassName, "Elements left: " + QueryQueue.Count);
            }
        }


        public void InsertBatch(int documentId, IDictionary<string, int> values)
        {
            EngineLogger.Log(this, "Added " + values.Count + " elements to queue.");
            foreach (var dictValue in values)
            {
                Insert(documentId,dictValue);
            }

        }

        public void InsertBatch(Uri documentUri, IDictionary<string, int> values)
        {
            InsertBatch(MySqlDocumentRepository.GetId(documentUri), values);
        }

        public void Insert(int documentId, KeyValuePair<string, int> value)
        {
            var termId = GetTermId(value.Key);
            if (termId <= 0)
            {
                Retry(documentId,value);
                return;
            }
            var localQuery = Constants.Queries.InsertWeightQuery;
            localQuery = GenericTools.FillParameter(localQuery, Constants.Parameters.TermId, GetTermId(value.Key));
            localQuery = GenericTools.FillParameter(localQuery, Constants.Parameters.DocumentId, documentId);
            localQuery = GenericTools.FillParameter(localQuery, Constants.Parameters.Count, value.Value);
            QueryQueue.Enqueue(localQuery);
        }

        private static void Retry(int documentId, KeyValuePair<string, int> value)
        {
            //EngineLogger.Log(ClassName,"Retrying value "+value.Key+" on document "+documentId);
            RetryQueue.Enqueue(new Tuple<int, KeyValuePair<string, int>>(documentId,value));
            if (!RetryTimer.Enabled)
            {
                RetryTimer.Start();
            }
        }


        public void Insert(Uri documentUri, KeyValuePair<string, int> value)
        {
            Insert(MySqlDocumentRepository.GetId(documentUri), value);
        }
    }
}