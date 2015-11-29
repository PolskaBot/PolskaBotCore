using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using MiscUtil.IO;
using MiscUtil.Conversion;
using PolskaBot.Core.Darkorbit.Commands;

namespace PolskaBot.Core
{
    abstract class Client
    {
        public Thread thread { get; private set; }
        public TcpClient tcpClient { get; private set; }
        public NetworkStream stream { get; private set; }
        public MergedClient mergedClient { get; private set; }

        public string IP { get; set; }
        public int port { get; set; }

        public Client(MergedClient mergedClient)
        {
            thread = new Thread(new ThreadStart(Run));
            tcpClient = new TcpClient();
            this.mergedClient = mergedClient;
        }

        public void Connect(string IP, int port)
        {
            this.IP = IP;
            this.port = port;

            tcpClient.Connect(this.IP, this.port);
            if(tcpClient.Connected)
            {
                thread.Start();
                stream = tcpClient.GetStream();
            }
        }

        public void Send(Command command)
        {
            command.Write(stream);
        }

        protected void Run()
        {
            while(true)
            {
                if (!tcpClient.Connected)
                {
                    return;
                }

                Parse(new EndianBinaryReader(EndianBitConverter.Big, stream));
            }

        }

        protected abstract void Parse(EndianBinaryReader reader);
    }
}
