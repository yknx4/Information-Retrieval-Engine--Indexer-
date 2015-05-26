/*************************************************************************
 * This work is based on original work authored by Craig Baird, released *
 * under the Code Project Open Licence (CPOL) 1.02;                      *
 * http://www.codeproject.com/info/cpol10.aspx                           *
 * This work is provided as is, no guarentees are made as to             *
 * suitability of this work for any specific purpose, use it at          *
 * your own risk.                                                        *
 * This product is not intended for use in any form except               *
 * learning. The author recommends only using small sections of          *
 * code from this project when integrating the attacked                  *
 * TcpServer project into your own project.                              *
 * This product is not intended for use for any comercial                *
 * purposes, however it may be used for such purposes.                   *
 *************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using Engine.Similarity;
using tcpServer;

namespace Search
{
    public partial class ServerInterfce : Form
    {
        public ServerInterfce()
        {
            InitializeComponent();
        }

        private void ServerInterfce_Load(object sender, EventArgs e)
        {
            tcpServer1.Encoding = Encoding.UTF8;
            tcpServer1.Port = (int) Constants.SearchPort;
            tcpServer1.Open();

        }
        public delegate void InvokeDelegate();
        private void OnConnection(tcpServer.TcpServerConnection connection)
        {
            InvokeDelegate setText = () =>
            {
                var text = connection + " connected. (" + tcpServer1.Connections.Count + ")";
                logData(text);
                lblStatus.Text = text;
                //connection.sendData(text);
                //slblConnected.Text = tcpServer1.Connections.Count.ToString();
            };

            Invoke(setText);
        }
        public void logData(bool sent, string text, bool extra)
        {
            if (extra) txtLog.Text += "\r\n" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt") + (sent ? " SENT:\r\n" : " RECEIVED:\r\n");
            txtLog.Text += text;
            txtLog.Text += "\r\n";
            if (txtLog.Lines.Length > 500)
            {
                string[] temp = new string[500];
                Array.Copy(txtLog.Lines, txtLog.Lines.Length - 500, temp, 0, 500);
                txtLog.Lines = temp;
            }
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        public void logData(bool sent, string text)
        {
            logData(sent, text, true);
        }
        public void logData(string text)
        {
            logData(false, text, false);
        }
        private void OnDataAvailable(tcpServer.TcpServerConnection connection)
        {
            if (connection.Socket.Available > 0)
            {
               
                string dataStr=null;
                using (var reader = new StreamReader(connection.Socket.GetStream(),Encoding.UTF8))
                {
                    dataStr = reader.ReadLine();
                }
                var engine = new RetrievalEngine(dataStr);
              //  connection.sendData(dataStr);
                var result = engine.Retrieve().ToList();
               // connection.sendData(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result));
                InvokeDelegate del = () =>
                {
                    logData(false, dataStr);
                };
                Invoke(del);

            }

        }
        private void send(string data, TcpServerConnection connection)
        {
             char[] spl = {'\n', '\r'};
            var lines = data.Split(spl, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                data = data + line.Replace("\r", "").Replace("\n", "") + "\r\n";
            }
            data = data.Substring(0, data.Length - 2);

            connection.sendData(data);

            InvokeDelegate del = () =>
                {
                    tcpServer1.Send(data);
                    logData(true, data);
                };
                Invoke(del);
        }
        private void OnError(tcpServer.TcpServer server, Exception e)
        {
            server.Close();
            
            InvokeDelegate del = () =>
            {
                logData(false, e.ToString());
            };
            Invoke(del);
        }

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            tcpServer1.Close();
        }
    }
}
