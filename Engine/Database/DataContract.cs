namespace Engine.Database
{
    abstract class DataContract
    {
        
        static public class DocumentEntry
        {
            public static string TableName = "documents";
            public static string Id = "id";
            public static string Title = "title";
            public static string Url = "url";
            public static string UrlHash = "url_hash";
        }
        static public class TermEntry
        {
            public static string TableName = "indexes";
            public static string Id = "id";
            public static string Value = "name";
        }

        static public class WeightEntry
        {
            public static string TableName = "weights";
            public static string Id = "id";
            public static string TermId = "term_id";
            public static string DocumentId = "document_id";
            public static string Count = "count";

        }
    }
}
