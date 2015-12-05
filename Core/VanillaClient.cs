using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;
using MiscUtil.Conversion;
using PolskaBot.Core.Darkorbit.Commands;
using PolskaBot.Core.Darkorbit.Commands.PostHandshake;

namespace PolskaBot.Core
{
    public class VanillaClient : Client
    {

        public VanillaClient(MergedClient mergedClient) : base(mergedClient)
        {

        }

        public void SendEncoded(Command command)
        {
            Console.WriteLine(command.GetType().ToString());
            byte[] rawBuffer = command.ToArray();
            mergedClient.fadeClient.Send(new FadeEncodePacket(rawBuffer));
            byte[] encodedBuffer = new byte[rawBuffer.Length];
            mergedClient.fadeClient.stream.Read(encodedBuffer, 0, rawBuffer.Length);
            Send(encodedBuffer);
        }

        public override void Parse(EndianBinaryReader reader)
        {
            EndianBinaryReader fadeReader = new EndianBinaryReader(EndianBitConverter.Big, mergedClient.fadeClient.stream);

            byte[] lengthBuffer = reader.ReadBytes(2);

            mergedClient.fadeClient.Send(new FadeDecodePacket(lengthBuffer));
            ushort fadeLength = fadeReader.ReadUInt16();

            byte[] contentBuffer = reader.ReadBytes(fadeLength);

            mergedClient.fadeClient.Send(new FadeDecodePacket(contentBuffer));

            ushort fadeID = fadeReader.ReadUInt16();

            Console.WriteLine($"Received packet of ID {fadeID}");

            switch (fadeID)
            {
                case ServerVersionCheck.ID:
                    ServerVersionCheck serverVersionCheck = new ServerVersionCheck(fadeReader);

                    if (mergedClient.api.mode == API.Mode.PROXY)
                    {
                        SendBack(lengthBuffer, contentBuffer);
                        return;
                    }

                    if (serverVersionCheck.compatible)
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

                    Console.WriteLine("Received server code with length of {0}", serverRequetCode.codeLength);

                    if (mergedClient.api.mode == API.Mode.PROXY)
                    {
                        mergedClient.fadeClient.Send(new FadeInitStageOne(serverRequetCode.code));
                        if(fadeReader.ReadBoolean())
                            SendBack(lengthBuffer, contentBuffer);
                        return;
                    }

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
                        SendEncoded(new Ping());
                        SendEncoded(new Login(165206592, "4112a8cafd36f7e51b0c03423e3b433f", 1, 578));
                    }

                    break;
                case HeroInit.ID:
                    HeroInit heroInit = new HeroInit(fadeReader);
                    Console.WriteLine("{0} {1} {2}/{3} {4}/{5} ({6}) pos ({7}, {8})", heroInit.rank, heroInit.userName, heroInit.hp, heroInit.maxHP,
                        heroInit.shield, heroInit.maxShield, heroInit.speed, heroInit.x, heroInit.y);
                    break;
                default:
                    Console.WriteLine("Received packet of ID {0} which is not supported", fadeID);
                    fadeReader.ReadBytes(fadeLength - 2);
                    break;
            }
        }

        public void SendBack(byte[] length, byte[] buffer)
        {
            List<byte> list = new List<byte>();
            list.AddRange(length);
            list.AddRange(buffer);
            mergedClient.api.proxyServer.Send(list.ToArray());
        }
    }
}
