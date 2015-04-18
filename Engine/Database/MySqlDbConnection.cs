using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Engine.Database
{
    class MySqlDbConnection
    {
        private const string ClassName = "MySqlDbCoonection";
        private static volatile int _currentConnection = 0;
        private static volatile int _lastConnection = 0;
        private static readonly ConcurrentQueue<MySqlConnection> MySqlConnections = new ConcurrentQueue<MySqlConnection>();
        
        public static  MySqlConnection GetConnection()
        {
            var firstMessage = true;
            while (_currentConnection >= Constants.MaxConnections)
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
            
            if (_lastConnection!= _currentConnection)
            {
                //EngineLogger.Log(ClassName, "Serving connection number: "+_currentConnection);
                _lastConnection = _currentConnection;
            }
            


            
            return connection;
        }

        public static MySqlConnection GetConnectionWithPriority()
        {
            MySqlConnection connection;
            if (MySqlConnections.TryDequeue(out connection))
            {
                if (connection.State == ConnectionState.Broken) connection.Close();
            }
            else
            {
                connection = new MySqlConnection(Constants.ConnectionString);
            }
            if (connection.State == ConnectionState.Closed) connection.Open();
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
