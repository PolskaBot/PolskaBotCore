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
        public API api { get; private set; }

        public Thread thread { get; private set; }
        public TcpClient tcpClient { get; private set; }
        public NetworkStream stream { get; private set; }

        public string IP { get; set; }
        public int port { get; set; }

        public event EventHandler<EventArgs> OnConnected;
        public event EventHandler<EventArgs> Disconnected;

        public Client(API api)
        {
            this.api = api;
            thread = new Thread(new ThreadStart(Run));
            tcpClient = new TcpClient();
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
            if (!IsConnected())
            {
                Console.WriteLine("Detected disconnect (onSendCommand)");
                Disconnected?.Invoke(this, EventArgs.Empty);
                return;
            }
            byte[] buffer = command.ToArray();
            stream.Write(buffer, 0, buffer.Length);
        }

        public void Send(byte[] buffer)
        {
            if (!IsConnected())
            {
                Console.WriteLine("Detected disconnect (onSendBuffer)");
                Disconnected?.Invoke(this, EventArgs.Empty);
                return;
            }
            stream.Write(buffer, 0, buffer.Length);
        }

        protected void Run()
        {
            while(true)
            {
                if (!IsConnected())
                {
                    Console.WriteLine("Detected disconnect");
                    Disconnected?.Invoke(this, EventArgs.Empty);
                    continue;
                }

                Parse(new EndianBinaryReader(EndianBitConverter.Big, stream));
            }

        }

        private bool IsConnected()
        {
            try
            {
                return !(api.vanillaClient.tcpClient.Client.Poll(1, SelectMode.SelectRead) && api.vanillaClient.tcpClient.Client.Available == 0);
            }
            catch (SocketException) { return false; }
        }

        public abstract void Parse(EndianBinaryReader reader);
    }
}
