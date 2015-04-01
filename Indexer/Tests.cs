using System;
using System.Windows.Forms;
using Engine.Database.Interfaces;
using Engine.Database.Repositories;
using Engine.Model;
using Engine.Tools;

namespace Indexer
{
    public partial class Tests : Form
    {
        public Tests()
        {
            InitializeComponent();
        }

        private void btnStrip_Click(object sender, EventArgs e)
        {
            var originalText = txtContent.Text;
            var result = HtmlTools.StripTagsCharArray(originalText, true, true);
            result = StringTools.ReplaceTabs(result);
            result = StringTools.TrimSpaces(result);
            result = StringTools.TrimNewLines(result);
            txtContent.Text = result;
            txtURL.Text = "Extraidos " + txtContent.Text.Length + " caracteres de: " + HtmlTools.ExtractTitle(originalText) + ".";
        }

        private void statusStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void btnFull_Click(object sender, EventArgs e)
        {
            var originalText = txtContent.Text;
            _logicalView = new LogicalView(originalText);
            _logicalView.Initialize();
            txtContent.Text = _logicalView.Data;
            toolStripStatusLabel1.Text = "Extraidos " + txtContent.Text.Length + " caracteres de: " + _logicalView.Title + ".";
        }

        private LogicalView _logicalView;
        private void btnCargar_Click(object sender, EventArgs e)
        {
            var contentuUri = new Uri(txtURL.Text);
            _logicalView = new LogicalView(contentuUri);
            _logicalView.Initialize();
            txtContent.Text = _logicalView.Data;
            toolStripStatusLabel1.Text = "Extraidos " + txtContent.Text.Length + " caracteres de: " + _logicalView.Title + ".";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_logicalView == null || !_logicalView.IsInitialized) return;
            IDocumentRepository repo = new MySqlDocumentRepository();
            repo.Insert(_logicalView);
        }

        private void btnAddTerms_Click(object sender, EventArgs e)
        {
            var repo = new MySqlTermRepository();
            repo.InsertBatch(txtTerms.Text.Split(' '));
        }
    }
}
