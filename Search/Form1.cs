using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using Engine.Database.Repositories;
using Engine.Similarity;

namespace Search
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private RetrievalEngine _engine;
        private void btnSearch_Click(object sender, EventArgs e)
        {
            lstResult.Items.Clear();
            _engine = new RetrievalEngine(txtQuery.Text);
            var result = _engine.Retrieve().ToList();
            foreach (var resulten in result)
            {
                lstResult.Items.Add(MySqlDocumentRepository.GetDocumentName(resulten.Key) + " => " + resulten.Value);
            }
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            new ServerInterfce().Show();
        }
    }
}
