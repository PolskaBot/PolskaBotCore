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
            mergedClient.fadeClient.stream.Read(encodedBuffer, 0, rawBuffer.Length);
            Send(encodedBuffer);
        }

        protected override void Parse(EndianBinaryReader reader)
        {
            EndianBinaryReader fadeReader = new EndianBinaryReader(EndianBitConverter.Big, mergedClient.fadeClient.stream);

            byte[] lengthBuffer = reader.ReadBytes(2);

            mergedClient.fadeClient.Send(new FadeDecodePacket(lengthBuffer));
            ushort fadeLength = fadeReader.ReadUInt16();

            byte[] contentBuffer = reader.ReadBytes(fadeLength);

            mergedClient.fadeClient.Send(new FadeDecodePacket(contentBuffer));

            ushort fadeID = fadeReader.ReadUInt16();

            switch(fadeID)
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
                        mergedClient.fadeClient.Send(new FadeRequestCallback());
                        short callbackLength = fadeReader.ReadInt16();
                        byte[] buffer = fadeReader.ReadBytes(callbackLength);
                        SendEncoded(new ClientRequestCallback(buffer));
                    } else
                    {
                        Console.WriteLine("StageOne failed");
                    }

                    break;
                case ServerRequestCallback.ID:
                    ServerRequestCallback serverRequestCallback = new ServerRequestCallback(fadeReader);
                    mergedClient.fadeClient.Send(new FadeInitStageTwo(serverRequestCallback.secretKey));

                    bool initializedStageTwo = fadeReader.ReadBoolean();

                    if(initializedStageTwo)
                    {
                        Console.WriteLine("StageTwo initialized");
                    }

                    break;
                default:
                    Console.WriteLine("Received packet of ID {0} which is not supported", fadeID);
                    fadeReader.ReadBytes(fadeLength - 2);
                    break;
            }
        }
    }
}
