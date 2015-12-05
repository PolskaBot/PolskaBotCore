using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PolskaBot.Core
{
    class ProxyServer
    {
        public API api { get; private set; }

        TcpListener listener;
        Thread thread;

        public ProxyServer(API api)
        {
            this.api = api;
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
            listener.Start();
            thread = new Thread(new ThreadStart(Listen));
            thread.Start();
        }

        public void Listen()
        {
            while(true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected to ProxySever");
            }
        }
    }
}
