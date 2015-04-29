using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine.Database;

namespace Tests
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAux_Click(object sender, EventArgs e)
        {
            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("TERM\tDF\tPointer");
            using (var f = new BinaryReader(File.OpenRead(Constants.FinalAuxiliarFile)))
            {
                while (f.BaseStream.Position != f.BaseStream.Length)
                {
                    for (var i = 0; i < 3; i++)
                    {
                        if (i != 2)
                        sb.Append(f.ReadInt32()+"\t");
                        else
                            sb.Append(f.ReadInt64() + "\t");
                    }
                    sb.Append(Environment.NewLine);
                }
            }
            txtContent.Text = sb.ToString();
        }

        private void btnFinal_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DocID\tWeight");
            using (var f = new BinaryReader(File.OpenRead(Constants.FinalIndexFile)))
            {
                while (f.BaseStream.Position != f.BaseStream.Length)
                {
                    for (var i = 0; i < 2; i++)
                    {
                        if(i==0)
                        sb.Append(f.ReadInt32() + "\t");
                        else
                            sb.Append(f.ReadDouble() + "\t");
                    }
                    sb.Append(Environment.NewLine);
                }
            }
            txtContent.Text = sb.ToString();
        }

        private void btnInterm_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DocID\tFrecuencia\tNextPoint");
            using (var f = new BinaryReader(File.OpenRead(Constants.IntermediateFileName)))
            {
                while (f.BaseStream.Position != f.BaseStream.Length)
                {
                    for (var i = 0; i < 3; i++)
                    {
                        if(i==2)
                        sb.Append(f.ReadInt64() + "\t");
                        else
                        sb.Append(f.ReadInt32() + "\t");
                    }
                    sb.Append(Environment.NewLine);
                }
            }
            txtContent.Text = sb.ToString();
        }

        private void btnDocuments_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DocID\tTermID\tCount");
            using (var f = new BinaryReader(File.OpenRead(Constants.DocumentsFileName)))
            {
                while (f.BaseStream.Position != f.BaseStream.Length)
                {
                    for (var i = 0; i < 3; i++)
                    {
                        sb.Append(f.ReadInt32() + "\t");
                    }
                    sb.Append(Environment.NewLine);
                }
            }
            txtContent.Text = sb.ToString();
        }
    }
}
