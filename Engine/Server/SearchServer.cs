using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Engine.Similarity;
using tcpServer;

namespace Engine.Server
{
    class SearchServer
    {

        private SearchServer GetInstance()
        {
            return _instance ?? (_instance = new SearchServer());
        }

        private TcpServer _server;
        private SearchServer _instance;

        private SearchServer()
        {
           
            _server = new TcpServer {Port = (int) Constants.SearchPort};
        }



    }
}
