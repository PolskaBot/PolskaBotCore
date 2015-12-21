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
                if(!thread.IsAlive)
                    thread.Start();
                stream = tcpClient.GetStream();
                OnConnected?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Disconnect()
        {
            tcpClient.Close();
            thread = new Thread(new ThreadStart(Run));
            tcpClient = new TcpClient();
        }

        public void Send(Command command)
        {
            if (!IsConnected())
            {
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
                    Disconnected?.Invoke(this, EventArgs.Empty);
                    return;
                }
                Parse(new EndianBinaryReader(EndianBitConverter.Big, stream));
            }

        }

        public bool IsConnected()
        {
            try
            {
                return !(tcpClient.Client.Poll(1, SelectMode.SelectRead) && tcpClient.Client.Available == 0);
            }
            catch (SocketException) { return false; }
        }

        public abstract void Parse(EndianBinaryReader reader);
    }
}
