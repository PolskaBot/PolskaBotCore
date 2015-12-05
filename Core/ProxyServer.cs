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

namespace PolskaBot.Core
{
    class ProxyServer
    {
        public API api { get; private set; }

        TcpListener listener;
        Thread thread;

        NetworkStream stream;
        EndianBinaryReader reader;

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
            bool policyFileRequest = false;

            while(true)
            {
                TcpClient client = listener.AcceptTcpClient();
                stream = client.GetStream();
                reader = new EndianBinaryReader(EndianBitConverter.Big, stream);

                // Handle policy request
                if(!policyFileRequest)
                {
                    int i = 0;
                    byte[] buffer = new byte[100];

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
                            SendText(text);
                            policyFileRequest = true;
                            api.Connect(api.IP);
                            client.Close();
                            break;
                        }
                    }
                } else
                {
                    api.mergedClient.vanillaClient.Parse(reader);
                }
            }
        }

        public void SendText(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }
    }
}
