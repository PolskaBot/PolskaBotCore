using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;
using MiscUtil.Conversion;
using PolskaBot.Core.Darkorbit.Commands;

namespace PolskaBot.Core
{
    class VanillaClient : Client
    {
        public VanillaClient(MergedClient mergedClient) : base(mergedClient)
        {

        }

        public void SendEncoded(Command command)
        {
            byte[] rawBuffer = command.ToArray();
            mergedClient.fadeClient.Send(new FadeEncodePacket(rawBuffer));
            byte[] encodedBuffer = new byte[rawBuffer.Length];
            mergedClient.fadeClient.stream.Read(encodedBuffer, 0, rawBuffer.Length - 4);
            Send(encodedBuffer);
        }

        protected override void Parse(EndianBinaryReader reader)
        {
            short length = reader.ReadInt16();
            short id = reader.ReadInt16();

            mergedClient.fadeClient.Send(new FadeDecodeHeader(length, id));
            EndianBinaryReader fadeReader = new EndianBinaryReader(EndianBitConverter.Big, mergedClient.fadeClient.stream);
            short fadeLength = fadeReader.ReadInt16();
            short fadeID = fadeReader.ReadInt16();

            Console.WriteLine("Received packet with ID {0} and length {1}", fadeID, fadeLength);

            mergedClient.fadeClient.Send(new FadeDecodeBody(reader.ReadBytes(fadeLength - 2)));


            switch(id)
            {
                case ServerVersionCheck.ID:
                    ServerVersionCheck serverVersionCheck = new ServerVersionCheck(fadeReader);

                    if(serverVersionCheck.compatible)
                    {
                        Console.WriteLine("Client is compatible");
                        Send(new ClientRequestCode());
                    } else
                    {
                        Console.WriteLine("Client is not compatible");
                        thread.Abort();
                    }
                    break;

                case ServerRequestCode.ID:
                    ServerRequestCode serverRequetCode = new ServerRequestCode(fadeReader);
                    mergedClient.fadeClient.Send(new FadeInitStageOne(serverRequetCode.code));

                    bool initialized = fadeReader.ReadBoolean();

                    if(initialized)
                    {
                        Console.WriteLine("StageOne initialized");
                    } else
                    {
                        Console.WriteLine("StageOne failed");
                    }

                    break;
                default:
                    fadeReader.ReadBytes(length - 2);
                    break;
            }
        }
    }
}
