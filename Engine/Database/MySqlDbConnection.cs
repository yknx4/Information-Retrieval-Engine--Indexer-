using System.Collections.Concurrent;
using System.Data;
using System.Threading;
using MySql.Data.MySqlClient;

namespace Engine.Database
{
    class MySqlDbConnection
    {
        private const string ClassName = "MySqlDbCoonection";
        private static volatile int _currentConnection;
        private static volatile int _lastConnection;
        private static readonly ConcurrentQueue<MySqlConnection> MySqlConnections = new ConcurrentQueue<MySqlConnection>();
        
        public static  MySqlConnection GetConnection()
        {
            var firstMessage = true;
            while (!AreConnectionsAvailable)
            {
                if (firstMessage)
                {
                    EngineLogger.Log(ClassName,"Too many connections, waiting for pool.");
                    firstMessage = false;
                }
            }
            if (!firstMessage)
            {
                EngineLogger.Log(ClassName, "Thanks for waiting.");
            }

            MySqlConnection connection = GetConnectionWithPriority();
            
            // ReSharper disable once RedundantCheckBeforeAssignment
            if (_lastConnection!= _currentConnection)
            {
                //EngineLogger.Log(ClassName, "Serving connection number: "+_currentConnection);
                _lastConnection = _currentConnection;
            }
            


            
            return connection;
        }

        public static bool AreConnectionsAvailable
        {
            get { return _currentConnection <= Constants.MaxConnections; }
        }
        public static MySqlConnection GetConnectionWithPriority()
        {
            var firstMessage = true;
            while (_currentConnection >= Constants.MaxConnections*4)
            {
                if (firstMessage)
                {
                    EngineLogger.Log(ClassName, "Too many connections, waiting for pool.");
                    firstMessage = false;
                }
            }
            if (!firstMessage)
            {
                EngineLogger.Log(ClassName, "Thanks for waiting.");
            }
            MySqlConnection connection;
            if (MySqlConnections.TryDequeue(out connection))
            {
                if (connection.State == ConnectionState.Broken) connection.Close();
            }
            else
            {
                connection = new MySqlConnection(Constants.ConnectionString);
            }
            try
            {
            	if (connection.State == ConnectionState.Closed) connection.Open();
            }
            catch (System.Exception ex)
            {
            	Thread.Sleep(1000);
                connection = new MySqlConnection(Constants.ConnectionString);
            }

            _currentConnection++;
            return connection;
        }

        public static void ReturnConnection(MySqlConnection inputConnection)
        {
            if (inputConnection.State == ConnectionState.Broken) inputConnection.Close();
            if (inputConnection.State == ConnectionState.Open) inputConnection.Close();
            MySqlConnections.Enqueue(inputConnection);
            _currentConnection--;

        }
    }
}
