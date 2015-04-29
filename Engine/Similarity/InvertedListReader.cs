using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Database;

namespace Engine.Similarity
{
    class InvertedListReader
    {
        private static readonly Dictionary<int, Tuple<int, long>> AuxDictionary = new Dictionary<int, Tuple<int, long>>();
        static InvertedListReader ()
        {
            using (var a = new BinaryReader(File.OpenRead(Constants.FinalAuxiliarFile)))
            {
                while (a.BaseStream.Position != a.BaseStream.Length)
                {
                    AuxDictionary.Add(a.ReadInt32(),new Tuple<int, long>(a.ReadInt32(),a.ReadInt64()));
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
