using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MiscUtil.IO;
using MiscUtil.Conversion;
using System.IO;
using PolskaBot.Core.Darkorbit.Commands;

namespace PolskaBot.Core
{
    public class ProxyServer
    {
        public API api { get; private set; }

        TcpListener listener;
        Thread thread;

        TcpClient client;
        NetworkStream stream;
        EndianBinaryReader reader;

        public ProxyServer(API api)
        {
            this.api = api;
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
            listener.Start();

            InitPolicy();

            client = listener.AcceptTcpClient();
            stream = client.GetStream();
            reader = new EndianBinaryReader(EndianBitConverter.Big, stream);

            thread = new Thread(new ThreadStart(Listen));
            thread.Start();
        }

        public void InitPolicy()
        {
            int i = 0;
            byte[] buffer = new byte[100];

            TcpClient client = listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();

            while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string req = Encoding.ASCII.GetString(buffer, 0, i);

                if (req.StartsWith("<policy-file-request/>"))
                {
                    string text = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                    text += "<cross-domain-policy xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://www.adobe.com/xml/schemas/PolicyFile.xsd\">";
                    text += "   <site-control permitted-cross-domain-policies=\"all\"/>";
                    text += "   <allow-access-from domain=\"*\" to-ports=\"*\"/>";
                    text += "</cross-domain-policy>";
                    SendText(stream, text);
                    api.Connect(api.IP);
                    client.Close();
                    return;
                }
            }
        }

        public void Listen()
        {
            while (true)
            {
                ushort length = reader.ReadUInt16();
                ushort id = reader.ReadUInt16();
                byte[] content = reader.ReadBytes(length - 2);

                Console.WriteLine("Packet length: {0} ID: {1}", length, id);

                MemoryStream stream = new MemoryStream();
                EndianBinaryWriter writer = new EndianBinaryWriter(EndianBitConverter.Big, stream);
                writer.Write(length);
                writer.Write(content);

                api.mergedClient.vanillaClient.Send(stream.ToArray());
            }
        }

        public void SendText(NetworkStream stream, string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        public void Send(byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }
    }
}
