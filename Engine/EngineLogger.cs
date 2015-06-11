using System;
using System.Diagnostics;
using System.IO;
using Engine.Tools;

namespace Engine
{
    public class EngineLogger
    {
       // private static StreamWriter _debugWriter;
        static EngineLogger()
        {
            Debug.WriteLine("Engine logger initialized.");
            //var debugFile = File.OpenWrite(string.Format("RetrievalEngine.{0}.{1}.{2}.log", DateTime.Now.Year, DateTime.Now.Month,DateTime.Now.Day));
            //_debugWriter = new StreamWriter(debugFile);
        }
        public static void Log(object source, string message)
        {
            string txt = source.GetType().Name + ": " + message;
            Debug.WriteLine(txt);
           // _debugWriter.WriteLineAsync(txt);
        }
        public static void Log(string source, string message)
        {
            string txt = source + ": " + message;
            Debug.WriteLine(txt);
           // _debugWriter.WriteLineAsync(txt);
        }
    }
}
