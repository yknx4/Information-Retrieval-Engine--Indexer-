using System.ComponentModel;
using System.Windows.Forms;

namespace Indexer
{
    partial class Tests
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
            this.txtContent = new System.Windows.Forms.TextBox();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.btnCargar = new System.Windows.Forms.Button();
            this.btnStrip = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnFull = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtTerms = new System.Windows.Forms.TextBox();
            this.btnAddTerms = new System.Windows.Forms.Button();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtContent
            // 
            this.txtContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContent.Location = new System.Drawing.Point(12, 40);
            this.txtContent.MaxLength = 150000;
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtContent.Size = new System.Drawing.Size(620, 460);
            this.txtContent.TabIndex = 0;
            // 
            // txtURL
            // 
            this.txtURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtURL.Location = new System.Drawing.Point(12, 12);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(620, 22);
            this.txtURL.TabIndex = 1;
            // 
            // btnCargar
            // 
            this.btnCargar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCargar.Location = new System.Drawing.Point(638, 12);
            this.btnCargar.Name = "btnCargar";
            this.btnCargar.Size = new System.Drawing.Size(75, 23);
            this.btnCargar.TabIndex = 2;
            this.btnCargar.Text = "Cargar";
            this.btnCargar.UseVisualStyleBackColor = true;
            this.btnCargar.Click += new System.EventHandler(this.btnCargar_Click);
            // 
            // btnStrip
            // 
            this.btnStrip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStrip.Location = new System.Drawing.Point(638, 41);
            this.btnStrip.Name = "btnStrip";
            this.btnStrip.Size = new System.Drawing.Size(75, 45);
            this.btnStrip.TabIndex = 3;
            this.btnStrip.Text = "Strip HTML";
            this.btnStrip.UseVisualStyleBackColor = true;
            this.btnStrip.Click += new System.EventHandler(this.btnStrip_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus1,
            this.toolStripStatusLabel1});
            this.statusStrip.Location = new System.Drawing.Point(0, 531);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(725, 25);
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip1";
            this.statusStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusStrip_ItemClicked);
            // 
            // lblStatus1
            // 
            this.lblStatus1.Name = "lblStatus1";
            this.lblStatus1.Size = new System.Drawing.Size(0, 20);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(151, 20);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // btnFull
            // 
            this.btnFull.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFull.Location = new System.Drawing.Point(638, 92);
            this.btnFull.Name = "btnFull";
            this.btnFull.Size = new System.Drawing.Size(75, 89);
            this.btnFull.TabIndex = 5;
            this.btnFull.Text = "Strip HTML and NonChar";
            this.btnFull.UseVisualStyleBackColor = true;
            this.btnFull.Click += new System.EventHandler(this.btnFull_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(638, 187);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Upload";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtTerms
            // 
            this.txtTerms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtTerms.Location = new System.Drawing.Point(12, 506);
            this.txtTerms.Name = "txtTerms";
            this.txtTerms.Size = new System.Drawing.Size(499, 22);
            this.txtTerms.TabIndex = 7;
            // 
            // btnAddTerms
            // 
            this.btnAddTerms.Location = new System.Drawing.Point(517, 505);
            this.btnAddTerms.Name = "btnAddTerms";
            this.btnAddTerms.Size = new System.Drawing.Size(115, 23);
            this.btnAddTerms.TabIndex = 8;
            this.btnAddTerms.Text = "Add Terms";
            this.btnAddTerms.UseVisualStyleBackColor = true;
            this.btnAddTerms.Click += new System.EventHandler(this.btnAddTerms_Click);
            // 
            // Tests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 556);
            this.Controls.Add(this.btnAddTerms);
            this.Controls.Add(this.txtTerms);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnFull);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.btnStrip);
            this.Controls.Add(this.btnCargar);
            this.Controls.Add(this.txtURL);
            this.Controls.Add(this.txtContent);
            this.Name = "Tests";
            this.Text = "Tests";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtContent;
        private TextBox txtURL;
        private Button btnCargar;
        private Button btnStrip;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Button btnFull;
        private Button button1;
        private TextBox txtTerms;
        private Button btnAddTerms;
    }
}