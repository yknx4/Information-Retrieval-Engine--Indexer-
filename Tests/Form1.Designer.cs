namespace Tests
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.btnInterm = new System.Windows.Forms.Button();
            this.btnAux = new System.Windows.Forms.Button();
            this.btnFinal = new System.Windows.Forms.Button();
            this.btnDocuments = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtContent
            // 
            this.txtContent.Font = new System.Drawing.Font("Consolas", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContent.Location = new System.Drawing.Point(12, 41);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ReadOnly = true;
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtContent.Size = new System.Drawing.Size(752, 414);
            this.txtContent.TabIndex = 0;
            // 
            // btnInterm
            // 
            this.btnInterm.Location = new System.Drawing.Point(12, 12);
            this.btnInterm.Name = "btnInterm";
            this.btnInterm.Size = new System.Drawing.Size(100, 23);
            this.btnInterm.TabIndex = 1;
            this.btnInterm.Text = "Intermediate";
            this.btnInterm.UseVisualStyleBackColor = true;
            this.btnInterm.Click += new System.EventHandler(this.btnInterm_Click);
            // 
            // btnAux
            // 
            this.btnAux.Location = new System.Drawing.Point(118, 12);
            this.btnAux.Name = "btnAux";
            this.btnAux.Size = new System.Drawing.Size(75, 23);
            this.btnAux.TabIndex = 2;
            this.btnAux.Text = "Auxiliar";
            this.btnAux.UseVisualStyleBackColor = true;
            this.btnAux.Click += new System.EventHandler(this.btnAux_Click);
            // 
            // btnFinal
            // 
            this.btnFinal.Location = new System.Drawing.Point(199, 12);
            this.btnFinal.Name = "btnFinal";
            this.btnFinal.Size = new System.Drawing.Size(75, 23);
            this.btnFinal.TabIndex = 3;
            this.btnFinal.Text = "Final";
            this.btnFinal.UseVisualStyleBackColor = true;
            this.btnFinal.Click += new System.EventHandler(this.btnFinal_Click);
            // 
            // btnDocuments
            // 
            this.btnDocuments.Location = new System.Drawing.Point(280, 12);
            this.btnDocuments.Name = "btnDocuments";
            this.btnDocuments.Size = new System.Drawing.Size(96, 23);
            this.btnDocuments.TabIndex = 4;
            this.btnDocuments.Text = "Documents";
            this.btnDocuments.UseVisualStyleBackColor = true;
            this.btnDocuments.Click += new System.EventHandler(this.btnDocuments_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 467);
            this.Controls.Add(this.btnDocuments);
            this.Controls.Add(this.btnFinal);
            this.Controls.Add(this.btnAux);
            this.Controls.Add(this.btnInterm);
            this.Controls.Add(this.txtContent);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.Button btnInterm;
        private System.Windows.Forms.Button btnAux;
        private System.Windows.Forms.Button btnFinal;
        private System.Windows.Forms.Button btnDocuments;
    }
}

