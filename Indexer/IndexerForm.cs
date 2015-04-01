using System;
using System.ComponentModel;
using System.Windows.Forms;
using Engine.Database.Interfaces;
using Engine.Database.Repositories;
using Engine.Model;
using Indexer.Properties;

namespace Indexer
{
    public partial class IndexerForm : Form
    {
        public IndexerForm()
        {
            InitializeComponent();
        }

        private BindingList<LogicalView> _documentList = new BindingList<LogicalView>(); 

        private void btnTests_Click(object sender, EventArgs e)
        {
           
        }

        private void testsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var frmTests = new Tests();
            frmTests.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        IDocumentRepository _repo = new MySqlDocumentRepository();
        private void UploadAllToDB(object sender, EventArgs e)
        {
            _repo.InsertBatch(_documentList);
        }

        private void btnAddUrl_Click(object sender, EventArgs e)
        {
            try
            {
                var contentUri = new Uri(txtUrl.Text);
                var lview = new LogicalView(contentUri);
                _documentList.Add(lview);
                txtUrl.Text = string.Empty;
            }
            catch (UriFormatException er)
            {
                lblStatus1.Text = Resources.The_input_URL_is_invalid;
            }
        }

        private void AlSeleccionar(object sender, EventArgs e)
        {
            var selectedItem = lstUrs.SelectedItem as LogicalView;
            if (selectedItem != null && !selectedItem.IsInitialized)
            {
                selectedItem.Initialize();

            }
            LoadInForm(selectedItem);
        }

        private void LoadInForm(LogicalView selectedItem)
        {
            if (selectedItem != null)
            {
                lblStatus1.Text=selectedItem.Title+" cargado.";
                lblInfoTitle.Text = selectedItem.Title;
                lblNoOfKeywords.Text = selectedItem.NumberOfKeywords.ToString();
                lblUrl.Text = selectedItem.SourceUri.ToString();
                
            }
        }

        private void startSererToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void IndexerForm_Load(object sender, EventArgs e)
        {
            lstUrs.DataSource = _documentList;
        }
    }
}
