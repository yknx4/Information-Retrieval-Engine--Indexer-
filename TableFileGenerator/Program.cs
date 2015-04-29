using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TableFileGenerator
{
    class Program
    {
        double Idf(int i)
        {
            return Math.Log(_documentMaxTf.Count)/_termDf[i];
        }

        double NormalizedFrecuencie(int docid, int frecuencia)
        {
            return ((double) frecuencia)/_documentMaxTf[docid];
        }

        double Weight(int indice, int documento, int frecuencia)
        {
            return NormalizedFrecuencie(documento,frecuencia)*Idf(indice);
        }


        private readonly Dictionary<int, int> _documentMaxTf = new Dictionary<int, int>();
        private readonly Dictionary<int, int> _termDf = new Dictionary<int, int>();
        private readonly SortedDictionary<int, int> _termPointers = new SortedDictionary<int, int>();
        private LinkedList<IntermediateFileEntry> _intermediateFileEntries = new LinkedList<IntermediateFileEntry>();
        private int[] _currentDocs;
        class Document
        {
            public int Id { get; set; }
            public readonly Queue<Tuple<int, int>> KeysAndCountInts = new Queue<Tuple<int, int>>();
        }

        struct IntermediateFileEntry
        {
            public int DocumentId;
            public int TermCount;
            public int NextPointer;
        }
        private Document _currentDocument;
        Document GenerateDocument(int docuementId)
        {
            var results = new Document { Id = docuementId };
            using (var conn = new MySqlConnection(Constants.ConnectionString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = Constants.Queries.SelectTermsFromDocumentQuery(docuementId);
                cmd.CommandTimeout = Constants.MaxTimeout;
                var reader = cmd.ExecuteReader();
                var i = 0;
                var valmax = 0;

                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var val = reader.GetInt32(1);
                    if (val > valmax) valmax = val;
                    if (_termDf.ContainsKey(id))
                    {
                        _termDf[id]++;
                    }
                    else
                    {
                        _termDf.Add(id, 1);
                    }
                    results.KeysAndCountInts.Enqueue(new Tuple<int, int>(id, val));
                    //Console.WriteLine("Term: " + id+"\nCount:"+val);
                    i++;
                }
                _documentMaxTf.Add(docuementId, valmax);
            }
            return results;
        }
        private bool _hasMoreDocuments = true;

        public int[] GetDocumentsId(int offset)
        {
            return GetDocumentsId(offset, Constants.DefaultDocumentCount);
        }
        public int[] GetDocumentsId(int offset, int count)
        {
            var results = new int[10];
            using (var conn = new MySqlConnection(Constants.ConnectionString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = Constants.Queries.SelectDocumentsQuery(offset, count);
                cmd.CommandTimeout = Constants.MaxTimeout;
                var reader = cmd.ExecuteReader();
                var i = 0;
                while (reader.Read())
                {
                    var val = reader.GetInt32(0);
                    results[i] = val;
                    Console.WriteLine("Got document: " + val);
                    i++;
                }
                if (i < Constants.DefaultDocumentCount)
                {
                    _hasMoreDocuments = false;
                    Console.WriteLine("No more documents.");
                }
            }
            return results;
        }

        void Exec()
        {
            DocumentFile();
            IntermediateFile();

            Console.WriteLine("\n-----------------------------------------------------\n");
            Console.WriteLine("Inverted Index File");
            Console.WriteLine();
            Console.WriteLine("Document ID\tWeight");
            using (new BinaryWriter(File.Open(Constants.FinalIndexFile, FileMode.Create)))
            {
            }
//            using (new BinaryWriter(File.Open(Constants.FinalAuxiliarFile, FileMode.Create)))
//            {
//            }
            using (var r = new BinaryReader(File.OpenRead(Constants.IntermediateFileName)))
            using (var w = new BinaryWriter(File.Open(Constants.FinalIndexFile, FileMode.OpenOrCreate)))
            //using (var a = new BinaryWriter(File.Open(Constants.FinalAuxiliarFile, FileMode.OpenOrCreate)))
            {
                
                foreach (var tentr in _firstAuxiliarDictionary)
                {
                    var indexid = tentr.Key;
                    
                    Console.WriteLine("Term "+indexid);
                    // 2.
                    // Important variables:
                    //int length = (int)b.BaseStream.Length;
                    var pos = tentr.Value.pointer;
                    tentr.Value.pointer = w.BaseStream.Position;
                    //int required = 2000;
                    //int count = 0;

                    // 3.
                    // Seek the required index.
                    var count = 0;
                    do
                    {
                        r.BaseStream.Seek(pos, SeekOrigin.Begin);
                        var docid = r.ReadInt32();
                        var frec = r.ReadInt32();
                        var pointer = r.ReadInt64();
                        pos = pointer;
                        w.Write(docid);
                        w.Write(Weight(indexid, docid, frec));                        
                        count++;
                    } while (pos != -1);

                    Console.WriteLine("Term Finished ");
                    var storedCount = _termDf[indexid];
                    if (count == storedCount)
                    {
                        Console.WriteLine(" OK");    
                    }
                    else
                    {
                        Console.WriteLine("Fail");   
                    }
                }
            }

            using (var a = new BinaryWriter(File.Open(Constants.FinalAuxiliarFile, FileMode.OpenOrCreate)))
            {
                foreach (var auxiliarFileEntry in _firstAuxiliarDictionary)
                {
                    a.Write(auxiliarFileEntry.Value.term);
                    a.Write(auxiliarFileEntry.Value.df);
                    a.Write(auxiliarFileEntry.Value.pointer);
                }
            }

            Console.Read();
        }
        readonly Dictionary<int, AuxiliarFileEntry> _firstAuxiliarDictionary = new Dictionary<int, AuxiliarFileEntry>();
        private void IntermediateFile()
        {
            using (new BinaryWriter(File.Open(Constants.IntermediateFileName, FileMode.Create)))
            {
            }

            Console.WriteLine("\n-----------------------------------------------------\n");
            Console.WriteLine("Intermediate File");
            Console.WriteLine();
            Console.WriteLine("Document ID\tTerm Count\tNext");

            
            var entry = new AuxiliarFileEntry();
            using (var w = new BinaryWriter(File.Open(Constants.IntermediateFileName, FileMode.OpenOrCreate)))
            using (var r = new BinaryReader(File.OpenRead(Constants.DocumentsFileName)))
            {
                 while (r.BaseStream.Position != r.BaseStream.Length)
                 {
                     var docid = r.ReadInt32();
                     var termid = r.ReadInt32();
                     var count = r.ReadInt32();
                     if (!_firstAuxiliarDictionary.ContainsKey(termid))
                     {
                         var newEntry = new AuxiliarFileEntry
                         {
                             pointer = w.BaseStream.Position, 
                             df = 1, 
                             term = termid
                         };
                         _firstAuxiliarDictionary.Add(termid,newEntry);

                         w.Write(docid);
                         w.Write(count);
                         w.Write((long)-1);
                         Console.WriteLine(docid + "\t" + count + "\t" + "NULL" );
                     }
                     else
                     {
                         var oldEntry = _firstAuxiliarDictionary[termid];
                         var oldPos = oldEntry.pointer;
                         var newPos = w.BaseStream.Position;
                         oldEntry.df++;
                         oldEntry.pointer = newPos;
                         w.Write(docid);
                         w.Write(count);
                         w.Write(oldPos);
                         Console.WriteLine(docid + "\t" +count+ "\t" + oldPos);
                     }
                 }
                



            }





    







//            using (var r = new BinaryReader(File.OpenRead(Constants.DocumentsFileName)))
//            {
//                using (var w = new BinaryWriter(File.Open(Constants.IntermediateFileName, FileMode.OpenOrCreate)))
//                {
//                    var entry = new IntermediateFileEntry();
//                    while (r.BaseStream.Position != r.BaseStream.Length)
//                    {
//                        GenerateIntermediateEntries(ref entry, w, r);
//                    }
//                }
//            }
        }
        private void DocumentFile()
        {
            Console.WriteLine("Document File");
            using (var b = new BinaryWriter(File.Open(Constants.DocumentsFileName, FileMode.Create)))
            {
            }
            var documentPointer = 0;
            using (var b = new BinaryWriter(File.Open(Constants.DocumentsFileName, FileMode.OpenOrCreate)))
            {
                while (_hasMoreDocuments)
                {
                    Console.WriteLine("Fetching documents " + documentPointer + " to " + (documentPointer + Constants.DefaultDocumentCount));
                    _currentDocs = GetDocumentsId(documentPointer);
                    documentPointer += Constants.DefaultDocumentCount;
                    foreach (var currentDoc in _currentDocs)
                    {
                        if (currentDoc == 0) break;
                        _currentDocument = GenerateDocument(currentDoc);
                        //Console.ReadLine();
                        while (_currentDocument.KeysAndCountInts.Count > 0)
                        {
                            b.Write(_currentDocument.Id);
                            var item = _currentDocument.KeysAndCountInts.Dequeue();
                            b.Write(item.Item1);
                            b.Write(item.Item2);
                        }
                    }


                    
                    //Sign(b);



                }
            }
        }
        private static void Sign(BinaryWriter b)
        {
            b.Write((int)0);
            b.Write(0xFF);
            b.Write(0xFF);
            b.Write((int)0);

        }
        static void Main(string[] args)
        {
            new Program().Exec();
        }
    }

    internal class AuxiliarFileEntry
    {
        public int term;
        public int df;
        public long pointer;

    }
}
