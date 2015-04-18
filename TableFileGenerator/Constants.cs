using MySql.Data.MySqlClient;

namespace TableFileGenerator
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
                    Port = Port,
                    DefaultCommandTimeout = 999
                };
                return connString.ToString();
            }
        }

        public static int TermIdMaximumTries = 3;

        public static double CommitInterval = 2000;
        public static double RetryInterval = 10000;
        public static int QueriesPerTransaction = 2000;
        public static int MaxTimeout = 2147483;
        public static int DefaultDocumentCount = 2;

        public abstract class Queries
        {
            public static string SelectDocumentsQuery(int offsett, int count)
            {
                return "SELECT D.id from documents D LIMIT " + offsett + "," + count + ";";
            }

            public static string SelectTermsFromDocumentQuery(int documentId)
            {
                return
                    "SELECT I.id, W.count from indexes I INNER JOIN weights W on W.term_id = I.id WHERE W.document_id = " + documentId + ";";
            }
        }

        public static string DocumentsFileName = "documents.bin";


    }
}
