using System.ComponentModel;
using System.Windows.Forms;

namespace Indexer
{
    partial class IndexerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.testsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddUrl = new System.Windows.Forms.Button();
            this.lstUrs = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkPros = new System.Windows.Forms.CheckBox();
            this.chkIns = new System.Windows.Forms.CheckBox();
            this.chkInit = new System.Windows.Forms.CheckBox();
            this.lblUrl = new System.Windows.Forms.LinkLabel();
            this.lblNoOfKeywords = new System.Windows.Forms.Label();
            this.lblInfoTitle = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bgwLoadData = new System.ComponentModel.BackgroundWorker();
            this.bgwInsertAll = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 416);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(716, 25);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus1
            // 
            this.lblStatus1.Name = "lblStatus1";
            this.lblStatus1.Size = new System.Drawing.Size(37, 20);
            this.lblStatus1.Text = "       ";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(29, 20);
            this.toolStripStatusLabel1.Text = "     ";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.testsToolStripMenuItem,
            this.generateTableToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(716, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveFileToolStripMenuItem,
            this.openFileToolStripMenuItem,
            this.toolStripSeparator1,
            this.testsToolStripMenuItem1,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveFileToolStripMenuItem
            // 
            this.saveFileToolStripMenuItem.Name = "saveFileToolStripMenuItem";
            this.saveFileToolStripMenuItem.Size = new System.Drawing.Size(147, 26);
            this.saveFileToolStripMenuItem.Text = "Save File";
            this.saveFileToolStripMenuItem.Visible = false;
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(147, 26);
            this.openFileToolStripMenuItem.Text = "Open File";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(144, 6);
            // 
            // testsToolStripMenuItem1
            // 
            this.testsToolStripMenuItem1.Name = "testsToolStripMenuItem1";
            this.testsToolStripMenuItem1.Size = new System.Drawing.Size(147, 26);
            this.testsToolStripMenuItem1.Text = "Tests";
            this.testsToolStripMenuItem1.Visible = false;
            this.testsToolStripMenuItem1.Click += new System.EventHandler(this.testsToolStripMenuItem1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(144, 6);
            this.toolStripSeparator2.Visible = false;
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(147, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // testsToolStripMenuItem
            // 
            this.testsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uploadAllToolStripMenuItem});
            this.testsToolStripMenuItem.Name = "testsToolStripMenuItem";
            this.testsToolStripMenuItem.Size = new System.Drawing.Size(84, 24);
            this.testsToolStripMenuItem.Text = "Database";
            // 
            // uploadAllToolStripMenuItem
            // 
            this.uploadAllToolStripMenuItem.Name = "uploadAllToolStripMenuItem";
            this.uploadAllToolStripMenuItem.Size = new System.Drawing.Size(155, 26);
            this.uploadAllToolStripMenuItem.Text = "Upload All";
            this.uploadAllToolStripMenuItem.Click += new System.EventHandler(this.UploadAllToDB);
            // 
            // generateTableToolStripMenuItem
            // 
            this.generateTableToolStripMenuItem.Name = "generateTableToolStripMenuItem";
            this.generateTableToolStripMenuItem.Size = new System.Drawing.Size(121, 24);
            this.generateTableToolStripMenuItem.Text = "Generate Index";
            this.generateTableToolStripMenuItem.Click += new System.EventHandler(this.startSererToolStripMenuItem_Click);
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.Location = new System.Drawing.Point(58, 31);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(566, 22);
            this.txtUrl.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "URL:";
            // 
            // btnAddUrl
            // 
            this.btnAddUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddUrl.Location = new System.Drawing.Point(630, 30);
            this.btnAddUrl.Name = "btnAddUrl";
            this.btnAddUrl.Size = new System.Drawing.Size(75, 23);
            this.btnAddUrl.TabIndex = 5;
            this.btnAddUrl.Text = "Add";
            this.btnAddUrl.UseVisualStyleBackColor = true;
            this.btnAddUrl.Click += new System.EventHandler(this.btnAddUrl_Click);
            // 
            // lstUrs
            // 
            this.lstUrs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstUrs.FormattingEnabled = true;
            this.lstUrs.ItemHeight = 16;
            this.lstUrs.Location = new System.Drawing.Point(15, 75);
            this.lstUrs.Name = "lstUrs";
            this.lstUrs.Size = new System.Drawing.Size(231, 324);
            this.lstUrs.TabIndex = 6;
            this.lstUrs.SelectedValueChanged += new System.EventHandler(this.AlSeleccionar);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtDesc);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.chkPros);
            this.groupBox1.Controls.Add(this.chkIns);
            this.groupBox1.Controls.Add(this.chkInit);
            this.groupBox1.Controls.Add(this.lblUrl);
            this.groupBox1.Controls.Add(this.lblNoOfKeywords);
            this.groupBox1.Controls.Add(this.lblInfoTitle);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(252, 75);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(452, 324);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Information";
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(156, 83);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(145, 128);
            this.txtDesc.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(64, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "Description:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkPros
            // 
            this.chkPros.AutoCheck = false;
            this.chkPros.AutoSize = true;
            this.chkPros.Location = new System.Drawing.Point(243, 217);
            this.chkPros.Name = "chkPros";
            this.chkPros.Size = new System.Drawing.Size(100, 21);
            this.chkPros.TabIndex = 9;
            this.chkPros.Text = "Processing";
            this.chkPros.UseVisualStyleBackColor = true;
            // 
            // chkIns
            // 
            this.chkIns.AutoCheck = false;
            this.chkIns.AutoSize = true;
            this.chkIns.Location = new System.Drawing.Point(156, 217);
            this.chkIns.Name = "chkIns";
            this.chkIns.Size = new System.Drawing.Size(81, 21);
            this.chkIns.TabIndex = 8;
            this.chkIns.Text = "Inserted";
            this.chkIns.UseVisualStyleBackColor = true;
            // 
            // chkInit
            // 
            this.chkInit.AutoCheck = false;
            this.chkInit.AutoSize = true;
            this.chkInit.Location = new System.Drawing.Point(52, 217);
            this.chkInit.Name = "chkInit";
            this.chkInit.Size = new System.Drawing.Size(88, 21);
            this.chkInit.TabIndex = 7;
            this.chkInit.Text = "Initialized";
            this.chkInit.UseVisualStyleBackColor = true;
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(153, 63);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(44, 17);
            this.lblUrl.TabIndex = 6;
            this.lblUrl.TabStop = true;
            this.lblUrl.Text = "http://";
            // 
            // lblNoOfKeywords
            // 
            this.lblNoOfKeywords.AutoSize = true;
            this.lblNoOfKeywords.Location = new System.Drawing.Point(153, 46);
            this.lblNoOfKeywords.Name = "lblNoOfKeywords";
            this.lblNoOfKeywords.Size = new System.Drawing.Size(16, 17);
            this.lblNoOfKeywords.TabIndex = 5;
            this.lblNoOfKeywords.Text = "2";
            // 
            // lblInfoTitle
            // 
            this.lblInfoTitle.AutoSize = true;
            this.lblInfoTitle.Location = new System.Drawing.Point(153, 29);
            this.lblInfoTitle.Name = "lblInfoTitle";
            this.lblInfoTitle.Size = new System.Drawing.Size(16, 17);
            this.lblInfoTitle.TabIndex = 4;
            this.lblInfoTitle.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(107, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "URL:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Number of keywords:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(104, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Title: ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // bgwLoadData
            // 
            this.bgwLoadData.WorkerSupportsCancellation = true;
            this.bgwLoadData.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DoWork);
            this.bgwLoadData.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.AtEndWorker);
            // 
            // bgwInsertAll
            // 
            this.bgwInsertAll.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.bgwInsertAll.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.AlTerminarInsertar);
            // 
            // IndexerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(716, 441);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lstUrs);
            this.Controls.Add(this.btnAddUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "IndexerForm";
            this.Text = "URL Indexer";
            this.Load += new System.EventHandler(this.IndexerForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private StatusStrip statusStrip1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveFileToolStripMenuItem;
        private ToolStripMenuItem openFileToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem testsToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem testsToolStripMenuItem;
        private TextBox txtUrl;
        private Label label1;
        private Button btnAddUrl;
        private ListBox lstUrs;
        private GroupBox groupBox1;
        private ToolStripStatusLabel lblStatus1;
        private Label label5;
        private Label label3;
        private Label label2;
        private LinkLabel lblUrl;
        private Label lblNoOfKeywords;
        private Label lblInfoTitle;
        private ToolStripMenuItem generateTableToolStripMenuItem;
        private ToolStripMenuItem uploadAllToolStripMenuItem;
        private BackgroundWorker bgwLoadData;
        private BackgroundWorker bgwInsertAll;
        private CheckBox chkIns;
        private CheckBox chkInit;
        private CheckBox chkPros;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private TextBox txtDesc;
        private Label label4;
    }
}

