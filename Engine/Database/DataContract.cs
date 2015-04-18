namespace Engine.Database
{
    abstract class DataContract
    {
        
        static public class DocumentEntry
        {
            public const string TableName = "documents";
            public const string Id = "id";
            public const string Title = "title";
            public const string Url = "url";
            public const string UrlHash = "url_hash";
        }
         static public class TermEntry
        {
            public const string TableName = "indexes";
            public const string Id = "id";
            public const string Value = "name";
        }

         static public class WeightEntry
        {
            public const string TableName = "weights";
            public const string Id = "id";
            public const string TermId = "term_id";
            public const string DocumentId = "document_id";
            public const string Count = "count";

        }
    }
}
