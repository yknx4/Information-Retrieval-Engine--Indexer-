using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Tools.Structures;
using MySql.Data.MySqlClient;

namespace Engine.Database
{
    class MySqlConnectionPool : ObjectPool<MySqlConnection>
    {
        public MySqlConnectionPool(string connectionString, int maxConnections) : base(() => new MySqlConnection(connectionString))
        {

        }
    }
}
