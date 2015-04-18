using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Timers;
using Engine.Database.Interfaces;
using Engine.Model;
using Engine.Tools;
using MySql.Data.MySqlClient;
using Timer = System.Timers.Timer;

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
                EngineLogger.Log(declaringType.Name, "Logger initalized");
            UpdateTerms();
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
            while (QueryQueue.TryDequeue(out queueItem) && queueCount < Constants.QueriesPerTransaction)
            {
                if (queueItem == null)
                {
                    EngineLogger.Log(ClassName, "WTF!?");
                    continue;
                }
                if (TermsIdDictionary.ContainsKey(queueItem.Item2)) continue;
                queryBuilder.Append(GenericTools.NumberStatementParameter(queueItem.Item1, Constants.Parameters.Value, queueCount));
                queuedTerms.Add(queueItem.Item2);
                queueCount++;
            }
            //using (var conn = new MySqlDbConnection(Constants.ConnectionString))
            var finalQuery = queryBuilder.ToString();
            if (!string.IsNullOrEmpty(finalQuery))
            {


                var conn = MySqlDbConnection.GetConnection();
                using (var cmd = conn.CreateCommand())
                {
                    
                    EngineLogger.Log(ClassName, "Query to process: " + finalQuery);
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
                        EngineLogger.Log(ClassName, "Exception on query! Check it! " + ex);
                    }
                }
                MySqlDbConnection.ReturnConnection(conn);
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

        public static int TermCount
        {
            get { return TermsIdDictionary.Count; }
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
            QueryQueue.Enqueue(new Tuple<string, string>(localQuery, input));

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
            InsertBatch(MySqlDocumentRepository.GetId(uri), dictionary);
        }

        public void InsertBatch(int documentId, IDictionary<string, int> values)
        {

            EngineLogger.Log(this, "Added " + values.Count + " elements to queue.");
            foreach (var termValue in values)
            {
                Insert(documentId, termValue);
            }
        }
        private readonly IWeightRepository _weightRepository = new MySqlWeightRepository();
        public void Insert(int documenId, KeyValuePair<string, int> value)
        {
            var localQuery = Constants.Queries.InsertTermQuery;
            //USED BEFORE, LEFT IN CASE OF DEBUG
            //localQuery = GenericTools.FillParameter(localQuery, Constants.Parameters.Value, value.Key.Replace("'", "''"));
            QueryQueue.Enqueue(new Tuple<string, string>(localQuery, value.Key));
            _weightRepository.Insert(documenId, value);
        }




        //TODO: Move retry to Async task, never get id on main thread.
        private static readonly Dictionary<String, int> TermsIdDictionary = new Dictionary<string, int>();

        private static int singleTermCount = 0;


        public static int GetTermId(string value)
        {
            var id = -1;
            var orval = value;
            value = value.ToLower();
            value = value.ToLowerInvariant();
            value = StringTools.RemoveDiacritics(value);
            var roundCount = 0;
            var found = false;
            while (roundCount < Constants.TermIdMaximumTries && !found)
            {
                roundCount++;
                if (singleTermCount > 100)
                {
                    
                    UpdateTerms();
                    singleTermCount = 0;
                }
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
                    singleTermCount++;
                    id = GetTermId(orval, true);
                }
                if (id > 0)
                {
                    found = true;
                    if (!TermsIdDictionary.ContainsKey(value)) TermsIdDictionary.Add(value, id);
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
                    EngineLogger.Log(ClassName, "Algo empezo a tronar.");
                    Thread.Sleep(10000);
                }
            }
            MySqlDbConnection.ReturnConnection(conn);
            return result;
        }

        private static volatile bool _updating;

        public static void UpdateTerms()
        {
            if (_updating)
            {
                while (_updating)
                {

                }
                return;
            }
            _updating = true;
            //EngineLogger.Log(ClassName, "Updating Terms");
            var terms = new MySqlTermRepository().GetAll();
            foreach (var term in terms)
            {
                if (TermsIdDictionary.ContainsKey(term.Value)) continue;
                TermsIdDictionary.Add(term.Value, term.Id);
            }
            _updating = false;
        }

    }
}