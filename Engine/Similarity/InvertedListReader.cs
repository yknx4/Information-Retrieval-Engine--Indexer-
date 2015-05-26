using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Engine.Database;

namespace Engine.Similarity
{
    class InvertedListReader
    {
        private static readonly Dictionary<int, Tuple<int, long>> AuxDictionary = null;
        private static int cnt = 0;
        static InvertedListReader ()
        {
            cnt++;
            if (AuxDictionary == null) { 
            AuxDictionary = new Dictionary<int, Tuple<int, long>>();
            using (var a = new BinaryReader(File.OpenRead(Constants.FinalAuxiliarFile)))
            {
                var pos = a.ReadInt64();
                //while (a.BaseStream.Position != a.BaseStream.Length)
                for(long i = 0;i<pos;i++)
                {
                    Int32 id = a.ReadInt32(), count=a.ReadInt32();
                    Int64 pointer=a.ReadInt64();
                    try
                    {
                        AuxDictionary.Add(id, new Tuple<int, long>(count, pointer));
                    }
                    catch (Exception e)
                    {
                        EngineLogger.Log("InvertedListReader", string.Format("WTF!!!! {1} -{2}- -{0}-", e,id,pointer));
                    }
                }
            }
            }
        }

        public static Tuple<int,double>[] GetDocumentsForIndex(int index)
        {
            var auxEntry = AuxDictionary[index];
            var initialPos = auxEntry.Item2;
            var documentCount = auxEntry.Item1;
            var result = new Tuple<int, double>[documentCount];
            using (var f = new BinaryReader(File.OpenRead(Constants.FinalIndexFile)))
            {
                f.BaseStream.Seek(initialPos, SeekOrigin.Begin);
                for (var i = 0; i < documentCount; i++)
                {
                    
                    result[i] = new Tuple<int, double>(f.ReadInt32(),f.ReadDouble());
                    
                }
            }
            return result;
        }
    }
}
