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

        //TODO: Move retry to Async task, never get id on main thread.
        private static readonly Dictionary<String, int> TermsIdDictionary = new Dictionary<string, int>();

        private static int GetTermId(string value)
        {
            var id=-1;
            var orval = value;
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
                    id = GetTermId(orval, true);
                }
                if (id > 0)
                {
                    found = true;
                    if(!TermsIdDictionary.ContainsKey(value))TermsIdDictionary.Add(value,id);
                    continue;
                }
                
                
            }
            //if (id == -1) EngineLogger.Log(ClassName, "Couldn't find id for " + value + " something is wrong.");
            return id;
        }
        public static int GetTermId(string value, bool lol)
        {
            var result = -1;
            //using (var conn = new MySqlDbConnection(Constants.ConnectionString))
            var conn = MySqlDbConnection.GetConnection();
            using (var cmd = conn.CreateCommand())
            {
                try
                {
                    //conn.Open();
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
                catch (NullReferenceException)
                {
                    EngineLogger.Log(ClassName,"Algo empezo a tronar.");
                    Thread.Sleep(10000);
                }
            }
            MySqlDbConnection.ReturnConnection(conn);
            return result;
        }

        static MySqlWeightRepository()
        {
            var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
            if (declaringType != null)
                EngineLogger.Log(declaringType.Name, "Logger initalized");
            QueryQueue = new ConcurrentQueue<Tuple<string, string>>();
            RetryQueue = new ConcurrentQueue<Tuple<int, KeyValuePair<string, int>>>();
            CommitTimer = new Timer { SynchronizingObject = null, Interval = Constants.CommitInterval };
            CommitTimer.Elapsed += CommitQuery;
            CommitTimer.Start();
            RetryTimer = new Timer { SynchronizingObject = null, Interval = Constants.RetryInterval };
            RetryTimer.Elapsed += RetryQuery;
            
        }

        private static void RetryQuery(object sender, ElapsedEventArgs e)
        {
            
            if (RetryQueue.IsEmpty)
            {
                EngineLogger.Log(ClassName,"No more items in retry queue.");
                RetryTimer.Stop();
                return;
            }
            UpdateTerms();
            var queueCount = 0;
            while (!RetryQueue.IsEmpty && queueCount < Constants.QueriesPerTransaction)
            {
                queueCount++;
                Tuple<int, KeyValuePair<string, int>> item;
                if (RetryQueue.TryDequeue(out item))
                {
                    new MySqlWeightRepository().Insert(item.Item1, item.Item2);
                }
            }
            
            
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
        private static readonly ConcurrentQueue<Tuple<string, string>> QueryQueue;
        private static readonly ConcurrentQueue<Tuple<int, KeyValuePair<string, int>>> RetryQueue;

        private static void CommitQuery(object source, ElapsedEventArgs e)
        {
            
            if (QueryQueue.IsEmpty)
            {

                MySqlRepositoriesSync.IsWeightRepositoryWorking = false;
                return;
            }
            //UpdateTerms();

            MySqlRepositoriesSync.IsWeightRepositoryWorking = true;
            var queryBuilder = new StringBuilder();
            Tuple<string, string> queueItem;
            var queueCount = 0;
            while (QueryQueue.TryDequeue(out queueItem) && queueCount < Constants.QueriesPerTransaction)
            {
                if (queueItem == null)
                {
                    EngineLogger.Log(ClassName,"WTF!?");
                    continue;
                }
                var localQuery = GenericTools.FillParameter(queueItem.Item1, Constants.Parameters.TermId, GetTermId(queueItem.Item2));
                queryBuilder.Append(localQuery);
                queueCount++;
            }
            //using (var conn = new MySqlDbConnection(Constants.ConnectionString))
            var conn = MySqlDbConnection.GetConnection();
            using (var cmd = conn.CreateCommand())
            {
                var finalQuery = queryBuilder.ToString();
                EngineLogger.Log(ClassName, "Query to process: " + finalQuery);
                //conn.Open();
                cmd.CommandText = finalQuery;
                cmd.CommandTimeout = Constants.MaxTimeout;
                if(!string.IsNullOrEmpty(finalQuery))cmd.ExecuteNonQuery();
            }
            MySqlDbConnection.ReturnConnection(conn);
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
            
            localQuery = GenericTools.FillParameter(localQuery, Constants.Parameters.DocumentId, documentId);
            localQuery = GenericTools.FillParameter(localQuery, Constants.Parameters.Count, value.Value);
            QueryQueue.Enqueue(new Tuple<string,string>(localQuery,value.Key));
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