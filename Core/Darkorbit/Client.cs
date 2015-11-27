using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using Core.Darkorbit.Commands;
using MiscUtil.IO;
using MiscUtil.Conversion;
using System.IO;

namespace Core.Darkorbit
{
    class Client
    {
        Thread thread;
        Thread vanillaThread;

        TcpClient tcpClient;
        TcpClient vanillaTcpClient;

        NetworkStream dataStream;
        NetworkStream vanillaDataStream;

        public static int BUFFER_SIZE = 65536;

        public Client()
        {
            thread = new Thread(new ThreadStart(Run));
            tcpClient = new TcpClient();
            tcpClient.ReceiveBufferSize = BUFFER_SIZE;

            vanillaThread = new Thread(new ThreadStart(RunVanilla));
            vanillaTcpClient = new TcpClient();
            vanillaTcpClient.ReceiveBufferSize = BUFFER_SIZE;
        }

        public void Connect(string server, string sid)
        {
            tcpClient.Connect(server, 8080);
            dataStream = tcpClient.GetStream();
            thread.Start();

            vanillaTcpClient.Connect("127.0.0.1", 8080);
            vanillaDataStream = vanillaTcpClient.GetStream();
            vanillaThread.Start();
        }

        public void Send(Command command)
        {
            command.Write(dataStream);
        }

        public void Send(byte[] buffer)
        {
            dataStream.Write(buffer, 0, buffer.Length);
        }

        public void SendVanilla(Command command)
        {
            command.Write(vanillaDataStream);
        }

        public void SendVanilla(byte[] buffer)
        {
            vanillaDataStream.Write(buffer, 0, buffer.Length);
        }

        #region private

        private void Run()
        {
            while(true)
            {
                byte[] buffer = new byte[BUFFER_SIZE];
                dataStream.Read(buffer, 0, BUFFER_SIZE);

                MemoryStream memoryStream = new MemoryStream(buffer);
                EndianBinaryReader reader = new EndianBinaryReader(EndianBitConverter.Big, memoryStream);

                short length = reader.ReadInt16();
                short id = reader.ReadInt16();

                switch (id)
                {
                    case ServerVersionCheck.ID:
                        ServerVersionCheck versionCheckPacket = new ServerVersionCheck(reader);

                        if(versionCheckPacket.compatible)
                        {
                            Send(new ClientRequestCode());
                        }
                        break;
                    default:
                        SendVanilla(new ClientProxy(buffer));
                        break;
                        
                }
            }
        }

        private void RunVanilla()
        {
            while(true)
            {
                byte[] buffer = new byte[BUFFER_SIZE];
                vanillaDataStream.Read(buffer, 0, BUFFER_SIZE);

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

