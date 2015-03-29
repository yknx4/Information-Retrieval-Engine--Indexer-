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

        private void testsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnAddUrl_Click(object sender, EventArgs e)
        {
            try
            {
                var contentUri = new Uri(txtUrl.Text);
                var lview = new LogicalView(contentUri);
                lstUrs.Items.Add(lview);
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
                lstUrs.Items[lstUrs.SelectedIndex] = lstUrs.SelectedItem;
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
    }
}
