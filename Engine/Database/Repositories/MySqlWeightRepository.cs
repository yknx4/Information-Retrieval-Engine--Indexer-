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
        private static  int _lastTermCount = -1;
        private static void RetryQuery(object sender, ElapsedEventArgs e)
        {
            
            if (RetryQueue.IsEmpty)
            {
                EngineLogger.Log(ClassName,"No more items in retry queue.");
                RetryTimer.Stop();
                return;
            }
            if (_lastTermCount != MySqlTermRepository.TermCount)
            {
                MySqlTermRepository.UpdateTerms();
                _lastTermCount = MySqlTermRepository.TermCount;
            }
            
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



       
        private const string ClassName = "MySqlWeightRepository";
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private static readonly Timer CommitTimer;
        private static readonly Timer RetryTimer;
        private static readonly ConcurrentQueue<Tuple<string, string>> QueryQueue;
        private static readonly ConcurrentQueue<Tuple<int, KeyValuePair<string, int>>> RetryQueue;

        private static void CommitQuery(object source, ElapsedEventArgs e)
        {
            if(MySqlRepositoriesSync.IsTermRepositoryWorking) return;
            if (QueryQueue.IsEmpty)
            {
                CommitTimer.Stop();
                MySqlRepositoriesSync.IsWeightRepositoryWorking = false;
                return;
            }
            //UpdateTerms();

            MySqlRepositoriesSync.IsWeightRepositoryWorking = true;
            var queryBuilder = new StringBuilder();
            Tuple<string, string> queueItem;
            var queueCount = 0;
            queryBuilder.Append(Constants.Queries.InsertWeightQuery);
            while (QueryQueue.TryDequeue(out queueItem) && queueCount < Constants.QueriesPerTransaction)
            {
                if (queueItem == null)
                {
                    EngineLogger.Log(ClassName,"WTF!?");
                    continue;
                }
                var localQuery = GenericTools.FillParameter(queueItem.Item1, Constants.Parameters.TermId, MySqlTermRepository.GetTermId(queueItem.Item2));
                queryBuilder.Append(localQuery+",");
                
                queueCount++;
            }
            if(!(queueCount>0)) return;
            
            //using (var conn = new MySqlDbConnection(Constants.ConnectionString))
            var conn = MySqlDbConnection.GetConnectionWithPriority();
            using (var cmd = conn.CreateCommand())
            {
                var finalQuery = queryBuilder.ToString().TrimEnd(',');
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
            var termId = MySqlTermRepository.GetTermId(value.Key);
            if (termId <= 0)
            {
                Retry(documentId,value);
                return;
            }
            var localQuery = Constants.Queries.InsertWeightValues;
            
            localQuery = GenericTools.FillParameter(localQuery, Constants.Parameters.DocumentId, documentId);
            localQuery = GenericTools.FillParameter(localQuery, Constants.Parameters.Count, value.Value);
            QueryQueue.Enqueue(new Tuple<string,string>(localQuery,value.Key));
            if (!CommitTimer.Enabled)
            {
                CommitTimer.Start();
            }
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