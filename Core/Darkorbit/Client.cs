using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using PolskaBot.Core.Darkorbit.Commands;
using MiscUtil.IO;
using MiscUtil.Conversion;
using System.IO;

namespace PolskaBot.Core.Darkorbit
{
    class Client
    {
        Thread thread;
        Thread fadeThread;

        TcpClient tcpClient;
        TcpClient fadeTcpClient;

        NetworkStream dataStream;
        NetworkStream fadeDataStream;

        public static int BUFFER_SIZE = 65536;

        public Client()
        {
            thread = new Thread(new ThreadStart(Run));
            tcpClient = new TcpClient();
            tcpClient.ReceiveBufferSize = BUFFER_SIZE;

            fadeThread = new Thread(new ThreadStart(RunFade));
            fadeTcpClient = new TcpClient();
            fadeTcpClient.ReceiveBufferSize = BUFFER_SIZE;
        }

        public void Connect(string server, string sid)
        {
            tcpClient.Connect(server, 8080);
            dataStream = tcpClient.GetStream();
            thread.Start();

            fadeTcpClient.Connect("127.0.0.1", 8081);
            fadeDataStream = fadeTcpClient.GetStream();
            fadeThread.Start();
        }

        public void Send(Command command)
        {
            command.Write(dataStream);
        }

        public void Send(byte[] buffer)
        {
            dataStream.Write(buffer, 0, buffer.Length);
        }

        public void SendFade(Command command)
        {
            command.Write(fadeDataStream);
        }

        public void SendFade(byte[] buffer)
        {
            fadeDataStream.Write(buffer, 0, buffer.Length);
        }

        #region private

        private void Run()
        {
            while(true)
            {
                EndianBinaryReader reader = new EndianBinaryReader(EndianBitConverter.Big, dataStream);

                short length = reader.ReadInt16();
                short id = reader.ReadInt16();

                Console.WriteLine("Received packet of ID {0} and length {1}", id, length);

                switch (id)
                {
                    case ServerVersionCheck.ID:
                        ServerVersionCheck versionCheckPacket = new ServerVersionCheck(reader);

                        if (versionCheckPacket.compatible)
                        {
                            Console.WriteLine("Is compatible");
                            Send(new ClientRequestCode());
                        }
                        break;
                    default:
                        Console.WriteLine("sending packet with id {0} to fade", id);
                        SendFade(new ClientProxy(length, id, reader.ReadBytes(length - 2)));
                        break;

                }
            }
        }

        private void RunFade()
        {
            while(true)
            {
                byte[] buffer = new byte[BUFFER_SIZE];
                fadeDataStream.Read(buffer, 0, BUFFER_SIZE);

                EndianBinaryReader reader = new EndianBinaryReader(EndianBitConverter.Big, new MemoryStream(buffer));

                short length = reader.ReadInt16();
                short id = reader.ReadInt16();
                
                if(id != 100)
                {
                    continue;
                }

                byte[] message = reader.ReadBytes(length);

                Send(message);
            }
        }

        #endregion
    }
}

