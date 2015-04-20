using System.Diagnostics;
using System.IO;

namespace Engine
{
    public class EngineLogger
    {
       
        static EngineLogger()
        {
            Debug.WriteLine("Engine logger initialized.");
        }
        public static void Log(object source, string message)
        {            
            Debug.WriteLine(source.GetType().Name + ": " + message);
        }
        public static void Log(string source, string message)
        {
            Debug.WriteLine(source + ": " + message);
        }
    }
}
