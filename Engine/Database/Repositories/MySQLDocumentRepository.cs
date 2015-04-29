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
    public class MySqlDocumentRepository : IDocumentRepository
    {
        private const string ClassName = "MySqlDocumentRepository";
        private static readonly Timer CommitTimer;
        private static readonly ConcurrentQueue<Tuple<string, LogicalView>> QueryQueue;


        static MySqlDocumentRepository()
        {
            var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
            if (declaringType != null)
                EngineLogger.Log(declaringType.Name,"Logger initalized");
            QueryQueue = new ConcurrentQueue<Tuple<string, LogicalView>>();
            CommitTimer = new Timer { SynchronizingObject = null, Interval = Constants.DocumentCommitInterval };
            CommitTimer.Elapsed += CommitQuery;
            CommitTimer.Enabled = true;
        }

        private static void CommitQuery(object source, ElapsedEventArgs e)
        {

            if (QueryQueue.IsEmpty)
            {
                MySqlRepositoriesSync.IsDocumentRepositoryWorking = false;
                return;
            }
            else
            {
               // EngineLogger.Log(ClassName, "Committing.");
            }
            
//            if (LogicalView.ProcessingViewsCount>(Environment.ProcessorCount*4))
//            {
//                if(Constants.DocumentCommitInterval<60000) Constants.DocumentCommitInterval *= 2;
//            }
//            else
//            {
//                Constants.DocumentCommitInterval = 1000;
//            }
            if (LogicalView.ProcessingViewsCount > 80) return; ;
            MySqlRepositoriesSync.IsDocumentRepositoryWorking = true;
            var queryBuilder = new StringBuilder();
            var queuedViews = new List<LogicalView>();
            var queueTerms = new ConcurrentQueue<Tuple<Uri, Dictionary<string, int>>>();
            Tuple<string, LogicalView> queueItem;
            var queueCount = 0;
            //while (QueryQueue.TryDequeue(out queueItem) && queueCount < Constants.DocumentQueriesPerTransaction)
            if (QueryQueue.TryDequeue(out queueItem))
            {
                if (queueItem == null)
                {
                    EngineLogger.Log(ClassName, "WTF!?");
                    return;
                }
                var lv = queueItem.Item2;
                int retryCount=0;
                while (!lv.IsInitialized && retryCount < Constants.TermIdMaximumTries)
                {
                    lv.Initialize();
                    retryCount++;
                }
                
                queueTerms.Enqueue(new Tuple<Uri, Dictionary<string, int>>(lv.SourceUri, lv.IndexTermsCount));               
                queryBuilder.Append(GenericTools.NumberStatementParameter(queueItem.Item1, new []{Constants.Parameters.Url,Constants.Parameters.Title}, queueCount));
                queuedViews.Add(queueItem.Item2);
                queueCount++;
            }
            var conn = MySqlDbConnection.GetConnection();
            //using (var conn = new MySqlDbConnection(Constants.ConnectionString))
            using (var cmd = conn.CreateCommand())
            {
                EngineLogger.Log(ClassName, "Preparing.");
                var finalQuery = queryBuilder.ToString();
                EngineLogger.Log(ClassName, "Query to process: " + finalQuery);
                //conn.Open();
                cmd.CommandText = finalQuery;
                cmd.Prepare();
                for (var i = 0; i < queueCount; i++)
                {
                    cmd.Parameters.AddWithValue(Constants.Parameters.Title + i, queuedViews[i].Title);
                    cmd.Parameters.AddWithValue(Constants.Parameters.Url + i, queuedViews[i].SourceUri);
                }
                cmd.CommandTimeout = Constants.MaxTimeout;
                try
                {

                    EngineLogger.Log(ClassName, "Inserting.");
                    cmd.ExecuteNonQuery();
                    foreach (var queuedView in queuedViews)
                    {
                        queuedView.IsInserted = true;
//                        queuedView.IsProcessing = false;
//                        LogicalView.ProcessingViewsCount--;
                    }
                }
                catch (Exception ex)
                {
                    EngineLogger.Log(ClassName, "Exception on query! Check it! " + ex);
                }
            }
            MySqlDbConnection.ReturnConnection(conn);
            ITermRepository termRepository = new MySqlTermRepository();
            foreach (var queueTerm in queueTerms)
            {
                termRepository.InsertBatch(queueTerm.Item1,queueTerm.Item2);
            }
            
            if (QueryQueue.IsEmpty)
            {
                EngineLogger.Log(ClassName, "Work done");
                MySqlRepositoriesSync.IsDocumentRepositoryWorking = false;
            }
            else
            {
                EngineLogger.Log(ClassName, "Elements left: " + QueryQueue.Count);
            }
        }

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
            //using (var conn = new MySqlDbConnection(Constants.ConnectionString))
            var conn = MySqlDbConnection.GetConnection();
            using (var cmd = conn.CreateCommand())
            {
                try
                {
                    //conn.Open();
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
            MySqlDbConnection.ReturnConnection(conn);
            return result;
        }

        public void Insert(LogicalView input)
        {

            QueryQueue.Enqueue(new Tuple<string, LogicalView>(Constants.Queries.InsertDocumentQuery,input));
              
            
            //Thread.Sleep(500);    
            //IWeightRepository weightRepository = new MySqlWeightRepository();
            //weightRepository.InsertBatch(input.SourceUri,input.IndexTermsCount);
        }

        public void InsertBatch(IEnumerable<LogicalView> input)
        {
            var termValues = input as LogicalView[] ?? input.ToArray();
            EngineLogger.Log(this, "Added " + termValues.Length + " elements to queue.");
            foreach (var termValue in termValues)
            {
                Insert(termValue);
            }           

        }

        public static string GetDocumentName(int id)
        {
            var result = "";
            //using (var conn = new MySqlDbConnection(Constants.ConnectionString))
            var conn = MySqlDbConnection.GetConnectionWithPriority();
            using (var cmd = conn.CreateCommand())
            {
                try
                {
                    //conn.Open();
                    cmd.CommandText = Constants.Queries.SelectDocumentNameFromId;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue(Constants.Parameters.DocumentId, id);
                    result = cmd.ExecuteScalar().ToString();
                }
                catch (MySqlException ex)
                {
                    if (ex.ToString() != string.Empty) result = "as";
                    // do somthing with the exception
                    // don't hide it
                }
                catch (NullReferenceException)
                {
                    EngineLogger.Log(ClassName, "Algo empezo a tronar.");
                    
                }
            }
            MySqlDbConnection.ReturnConnection(conn);
            return result;
        }
    }
}
