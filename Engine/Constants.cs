using Engine.Database;
using MySql.Data.MySqlClient;

namespace Engine
{
    public abstract class Constants
    {
        public const string Server = "localhost";
        public const string UserName = "yknx4";
        public const string Password = "konami1994";
        public const string Database = "information_retrieval";
        public const uint DatabasePort = 3306;
        public const uint SearchPort = 3002;

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
                    Port = DatabasePort
                };
                return connString.ToString();
            }
        }

        public static int MaxDocumentTries = 5;

        public const int MaximumTermsThreads = 20;

        public const int GroupSize = 3;

        public const int TermIdMaximumTries = 3;

        public const double CommitInterval = 300;
        public static double DocumentCommitInterval = 1000;
        public const double RetryInterval = 15000;
        public const int QueriesPerTransaction = 10000;
        public const int DocumentQueriesPerTransaction = 2;
        public const int MaxTimeout = 2147483;
        public const int MaxConnections = 50;


        public abstract class Queries
        {
            public const string InsertDocumentQuery =
                "INSERT IGNORE INTO `" + DataContract.DocumentEntry.TableName + "` (`" + DataContract.DocumentEntry.Url + "`,`" + DataContract.DocumentEntry.UrlHash + "`,`" + DataContract.DocumentEntry.Title + "`,`" + DataContract.DocumentEntry.Description + "`) VALUES (" + Parameters.Url + ",md5(" + Parameters.Url + ")," + Parameters.Title + "," + Parameters.Description + ");";

            public const string InsertTermQuery =
                "INSERT IGNORE INTO `" + DataContract.TermEntry.TableName + "` (`" + DataContract.TermEntry.Value +
                "`) VALUES ";
            public const string InsertTermValues = "(" + Parameters.Value + ")";

            public const string SelectAllTermsQuery = "SELECT * FROM " + DataContract.TermEntry.TableName + ";";

            public const string SelectDocumentIdFromUrlQuery =
                "SELECT COALESCE((SELECT `" + DataContract.DocumentEntry.Id + "` FROM `" + DataContract.DocumentEntry.TableName + "` WHERE `" + DataContract.DocumentEntry.UrlHash + "` = MD5(" + Parameters.Url + ")),-1) as " + DataContract.DocumentEntry.Id + ";";
            public const string SelectTermIdFromUrlQuery =
               "SELECT COALESCE((SELECT `" + DataContract.TermEntry.Id + "` FROM `" + DataContract.TermEntry.TableName + "` WHERE `" + DataContract.TermEntry.Value + "` = " + Parameters.Value + "),-1) as " + DataContract.TermEntry.Id + ";";

            public const string InsertWeightQuery =
                "INSERT IGNORE INTO `" + DataContract.WeightEntry.TableName + "`(`" + DataContract.WeightEntry.TermId +
                "`,`" + DataContract.WeightEntry.DocumentId + "`,`" + DataContract.WeightEntry.Count + "`) values ";

            public const string InsertWeightValues = "(" + Parameters.TermId + "," + Parameters.DocumentId + "," + Parameters.Count + ")";
            public const string SelectDocumentNameFromId = "select url from documents where id =" +Parameters.DocumentId;
        }

        public abstract class Parameters
        {
            public const string Url = "@url";
            public const string Title = "@title";
            public const string Value = "@value";
            public const string TermId = "@termid";
            public const string DocumentId = "@documentid";
            public const string Count = "@count";
            public const string Description = "@description";

        }

        public const string DocumentsFileName = "documents.bin";
        public const string IntermediateFileName = "intermediate.bin";
        public const string FinalIndexFile = "final.bin";
        public const string FinalAuxiliarFile = "final_aux.bin";

    }
}
