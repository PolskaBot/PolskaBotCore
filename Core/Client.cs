using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using MiscUtil.IO;
using MiscUtil.Conversion;
using PolskaBot.Core.Darkorbit.Commands;

namespace PolskaBot.Core
{
    abstract class Client
    {
        public object locker = new object();

        public API api { get; private set; }

        public bool Running { get; set; } = true;

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
            tcpClient = new TcpClient();
        }

        public void Connect(string IP, int port)
        {
            this.IP = IP;
            this.port = port;

            thread = new Thread(new ThreadStart(Run));

            try
            {
                tcpClient.Connect(this.IP, this.port);
            }
            catch (SocketException ex)
            {
                Thread.Sleep(5000);
                Disconnected?.Invoke(this, EventArgs.Empty);
                return;
            }

            if(tcpClient.Connected)
            {
                stream = tcpClient.GetStream();
                if (!thread.IsAlive)
                    thread.Start();
                OnConnected?.Invoke(this, EventArgs.Empty);
            }
        }

        public virtual void Stop()
        {
            Running = false;
            thread?.Abort();
            tcpClient?.Close();
            stream?.Close();
        }

        public void Disconnect()
        {
            tcpClient?.Client?.Disconnect(false);
            tcpClient?.Close();
            stream?.Close();
            thread = new Thread(new ThreadStart(Run));
            tcpClient = new TcpClient();
        }

        public void Send(Command command)
        {
            Send(command.ToArray());
        }

        public void Send(byte[] buffer)
        {
            if (!Running)
                return;

            if (!IsConnected())
            {
                Disconnected?.Invoke(this, EventArgs.Empty);
                return;
            }
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
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

        protected bool IsConnected()
        {
            if (tcpClient == null || tcpClient.Client == null || !stream.CanWrite || !stream.CanRead)
                return false;
            try
            {
                return !(tcpClient.Client.Poll(1, SelectMode.SelectRead) && tcpClient.Client.Available == 0);
            }
            catch (SocketException) { return false; }
        }

        public abstract void Parse(EndianBinaryReader reader);
    }
}
