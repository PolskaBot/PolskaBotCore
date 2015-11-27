using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using Core.Darkorbit.Commands;

namespace Core.Darkorbit
{
    class Client
    {
        Thread thread;
        TcpClient tcpClient;
        NetworkStream dataStream;

        public static int BUFFER_SIZE = 65536;

        public Client()
        {
            thread = new Thread(new ThreadStart(Run));
            tcpClient = new TcpClient();
            tcpClient.ReceiveBufferSize = BUFFER_SIZE;
        }

        public void Connect(string server, string sid)
        {
            tcpClient.Connect(server, 8080);

            dataStream = tcpClient.GetStream();

            thread.Start();
        }

        public void Send(Command command)
        {
            command.Write(dataStream);
        }

        public void Send(byte[] buffer)
        {
            dataStream.Write(buffer, 0, buffer.Length);
        }


        private void Run()
        {
            while(true)
            {
                byte[] buffer = new byte[BUFFER_SIZE];
                dataStream.Read(buffer, 0, BUFFER_SIZE);

                Command command = Command.Read(buffer);

                if(command == null)
                {
                    continue;
                }

                switch(command.GetID())
                {
                    case ServerVersionCheck.ID:
                        ServerVersionCheck versionCheckPacket = (ServerVersionCheck)command;

                        if(versionCheckPacket.compatible)
                        {
                            Send(new ClientRequestCode());
                        }
                        break;
                    case ServerRequestCode.ID:
                        ServerRequestCode requestCodePacket = (ServerRequestCode)command;
                        // TODO: Connect to Vanilla
                        break;
                    default:
                        Console.WriteLine(command.GetID());
                        break;
                        
                }
            }
        }
    }
}

