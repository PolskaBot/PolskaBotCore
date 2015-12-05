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
    public abstract class Client
    {
        public Thread thread { get; private set; }
        public TcpClient tcpClient { get; private set; }
        public NetworkStream stream { get; private set; }
        public MergedClient mergedClient { get; private set; }

        public string IP { get; set; }
        public int port { get; set; }

        public event EventHandler<EventArgs> OnConnected;

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
                OnConnected?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Send(Command command)
        {
            byte[] buffer = command.ToArray();
            stream.Write(buffer, 0, buffer.Length);
        }

        public void Send(byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
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

        public abstract void Parse(EndianBinaryReader reader);
    }
}
