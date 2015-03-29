using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Engine.Database
{
    abstract class Constants
    {
        public static string Server = "localhost";
        public static string UserName = "yknx4";
        public static string Password = "konami1994";
        public static string Database = "information_retrieval";
        public static uint Port = 3306;

        public static string ConnectionString
        {
            get
            {
                var connString = new MySqlConnectionStringBuilder
                {
                    Server = Server,
                    UserID = UserName,
                    Password = Password,
                    Database = Database,
                    Port = Port
                };
                return connString.ToString();
            }
        }



        public abstract class Queries
        {
            public static string InsertDocumentQuery =
                "INSERT INTO `" + DataContract.DocumentEntry.TableName + "` (`" + DataContract.DocumentEntry.Url + "`,`" + DataContract.DocumentEntry.UrlHash + "`,`" + DataContract.DocumentEntry.Title + "`) VALUES (" + Constants.Parameters.Url + ",md5(" + Constants.Parameters.Url + ")," + Constants.Parameters.Title + ");";
        }

        public abstract class Parameters
        {
            public static string Url = "@url";
            public static string Title = "@title";
        }
        
    }
}
