using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TableFileGenerator
{
    class Program
    {
        private Dictionary<int, int> _documentMaxTf = new Dictionary<int, int>();
        private Dictionary<int, int> _termDf = new Dictionary<int, int>();
        private int[] _currentDocs;
        class Document
        {
            public int Id { get; set; }
            public readonly Queue<Tuple<int, int>> KeysAndCountInts = new Queue<Tuple<int, int>>();
        }

        private Document currentDocument;
        Document GenerateDocument(int docuementId)
        {
            var results = new Document { Id = docuementId };
            using (var conn = new MySqlConnection(Constants.ConnectionString))
            using (var cmd = conn.CreateCommand())
            {
                //conn.Open();
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
                //conn.Open();
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
            using (BinaryWriter b = new BinaryWriter(File.Open(Constants.DocumentsFileName, FileMode.Create)))
            {
            }
            var documentPointer = 0;
            using (BinaryWriter b = new BinaryWriter(File.Open(Constants.DocumentsFileName, FileMode.OpenOrCreate)))
            {
                while (_hasMoreDocuments)
                {
                    Console.WriteLine("Fetching documents " + documentPointer + " to " + (documentPointer + Constants.DefaultDocumentCount));
                    _currentDocs = GetDocumentsId(documentPointer);
                    documentPointer += Constants.DefaultDocumentCount;
                    foreach (var currentDoc in _currentDocs)
                    {
                        if (currentDoc == 0) break;
                        currentDocument = GenerateDocument(currentDoc);
                        //Console.ReadLine();
                    }

                    b.Write(currentDocument.Id);
                    while (currentDocument.KeysAndCountInts.Count > 0)
                    {
                        var item = currentDocument.KeysAndCountInts.Dequeue();
                        b.Write(item.Item1);
                        b.Write(item.Item2);
                    }
                    b.Write((byte)0);
                    b.Write("ff");
                    b.Write((byte)0);


                }
            }
            Console.Read();
        }

        static void Main(string[] args)
        {
            new Program().Exec();
        }
    }
}
