using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
