using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
            bgwInsertAll.RunWorkerAsync(_documentList);

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
                lblStatus1.Text = Resources.The_input_URL_is_invalid + " : " + er;
            }
        }

        private void AlSeleccionar(object sender, EventArgs e)
        {
            if (bgwLoadData.IsBusy) bgwLoadData.CancelAsync();
            while (bgwLoadData.IsBusy) { }
            var selectedItem = lstUrs.SelectedItem as LogicalView;

            LoadInForm(selectedItem);


        }

        private void LoadInForm(LogicalView selectedItem)
        {
            toolStripStatusLabel1.Text = LogicalView.InsertedViewsCount + " inserted documents into DB.   " + LogicalView.ProcessingViewsCount + " documents loading. \t";
            if (selectedItem == null)
            {
                lblStatus1.Text = "Item not loaded or loading.";
                lblInfoTitle.Text = String.Empty;
                lblNoOfKeywords.Text = String.Empty;
                lblUrl.Text = String.Empty;
                txtDesc.Clear();
                return;
            }
            if (selectedItem.IsInitialized)
            {
                lblStatus1.Text = selectedItem.Title + " loaded.";
                lblInfoTitle.Text = selectedItem.Title;
                txtDesc.Text = selectedItem.Description;
                lblNoOfKeywords.Text = selectedItem.NumberOfKeywords.ToString();
                lblUrl.Text = selectedItem.SourceUri.ToString();
                chkInit.Checked = selectedItem.IsInitialized;
                chkIns.Checked = selectedItem.IsInserted;
                chkPros.Checked = selectedItem.IsProcessing;
            }
            else
            {
                lblStatus1.Text = selectedItem.Title + " loading.";
                lblInfoTitle.Text = selectedItem.Title;
                lblNoOfKeywords.Text = string.Empty;
                lblUrl.Text = selectedItem.SourceUri.ToString();
                chkInit.Checked = selectedItem.IsInitialized;
                chkIns.Checked = selectedItem.IsInserted;
                chkPros.Checked = selectedItem.IsProcessing;
            }

        }



        private void startSererToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("table_generator.exe");
        }

        private void IndexerForm_Load(object sender, EventArgs e)
        {
            lstUrs.DataSource = _documentList;
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {

            ((LogicalView)e.Argument).Initialize();
        }

        private void AtEndWorker(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {

                return;
            }
            LoadInForm(e.Result as LogicalView);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            _repo.InsertBatch(e.Argument as IEnumerable<LogicalView>);
        }

        private void AlTerminarInsertar(object sender, RunWorkerCompletedEventArgs e)
        {
            lblStatus1.Text = _documentList.Count + " documents queued.";
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog opfDialog = new OpenFileDialog();
            opfDialog.CheckFileExists = true;
            opfDialog.CheckPathExists = true;
            opfDialog.Multiselect = true;
            switch (opfDialog.ShowDialog())
            {
                case DialogResult.OK:
                    foreach (var file in opfDialog.FileNames)
                    {
                        var urls = File.ReadAllLines(file);

                        foreach (var url in urls)
                        {
                            try
                            {
                                if (url.Contains("google.") || url.Contains("googleusercontent.") || url.Contains("facebook.")) continue;
                                var contentUri = new Uri(url);
                                var lview = new LogicalView(contentUri);
                                _documentList.Add(lview);
                            }
                            catch (UriFormatException er)
                            {
                                lblStatus1.Text = Resources.The_input_URL_is_invalid + " : " + er;
                            }
                        }
                    }
                    break;

            }

        }


    }
}
