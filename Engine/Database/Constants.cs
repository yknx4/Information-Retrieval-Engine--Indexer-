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

        public static int TermIdMaximumTries = 3;

        public static double CommitInterval = 2000;
        public static double RetryInterval = 10000;
        public static int QueriesPerTransaction = 100;
        public static int MaxTimeout = 2147483;


        public abstract class Queries
        {
            public static string InsertDocumentQuery =
                "INSERT IGNORE INTO `" + DataContract.DocumentEntry.TableName + "` (`" + DataContract.DocumentEntry.Url + "`,`" + DataContract.DocumentEntry.UrlHash + "`,`" + DataContract.DocumentEntry.Title + "`) VALUES (" + Parameters.Url + ",md5(" + Parameters.Url + ")," + Parameters.Title + ");";
            public static string InsertTermQuery =
                "INSERT IGNORE INTO `" + DataContract.TermEntry.TableName + "` (`" + DataContract.TermEntry.Value + "`) VALUES (" + Parameters.Value + ");";

            public static string SelectAllTermsQuery = "SELECT * FROM " + DataContract.TermEntry.TableName + ";";

            public static string SelectDocumentIdFromUrlQuery =
                "SELECT COALESCE((SELECT `" + DataContract.DocumentEntry.Id + "` FROM `" + DataContract.DocumentEntry.TableName + "` WHERE `" + DataContract.DocumentEntry.UrlHash + "` = MD5(" + Parameters.Url + ")),-1) as " + DataContract.DocumentEntry.Id + ";";
            public static string SelectTermIdFromUrlQuery =
               "SELECT COALESCE((SELECT `" + DataContract.TermEntry.Id + "` FROM `" + DataContract.TermEntry.TableName + "` WHERE `" + DataContract.TermEntry.Value + "` = " + Parameters.Value + "),-1) as " + DataContract.TermEntry.Id + ";";

            public static string InsertWeightQuery =
                "INSERT IGNORE INTO `" + DataContract.WeightEntry.TableName + "`(`" + DataContract.WeightEntry.TermId + "`,`" + DataContract.WeightEntry.DocumentId + "`,`" + DataContract.WeightEntry.Count + "`) values (" + Parameters.TermId + "," + Parameters.DocumentId + "," + Parameters.Count + ");";
        }

        public abstract class Parameters
        {
            public static string Url = "@url";
            public static string Title = "@title";
            public static string Value = "@value";
            public static string TermId = "@termid";
            public static string DocumentId = "@documentid";
            public static string Count = "@count";

        }
        
    }
}
