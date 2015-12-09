using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;
using MiscUtil.Conversion;
using PolskaBot.Core.Darkorbit.Commands;
using PolskaBot.Core.Darkorbit.Commands.PostHandshake;
using System.Threading;

namespace PolskaBot.Core
{
    public class VanillaClient : Client
    {

        Thread pingThread;

        bool isMoving = false;

        public VanillaClient(API api) : base(api)
        {
            pingThread = new Thread(new ThreadStart(PingLoop));
        }

        public void SendEncoded(Command command)
        {
            byte[] rawBuffer = command.ToArray();
            api.fadeClient.Send(new FadeEncodePacket(rawBuffer));
            byte[] encodedBuffer = new byte[rawBuffer.Length];
            api.fadeClient.stream.Read(encodedBuffer, 0, rawBuffer.Length);
            Send(encodedBuffer);
        }

        public override void Parse(EndianBinaryReader reader)
        {
            EndianBinaryReader fadeReader = new EndianBinaryReader(EndianBitConverter.Big, api.fadeClient.stream);

            byte[] lengthBuffer = reader.ReadBytes(2);

            api.fadeClient.Send(new FadeDecodePacket(lengthBuffer));
            ushort fadeLength = fadeReader.ReadUInt16();

            byte[] contentBuffer = reader.ReadBytes(fadeLength);

            api.fadeClient.Send(new FadeDecodePacket(contentBuffer));

            ushort fadeID = fadeReader.ReadUInt16();

            switch (fadeID)
            {
                case ServerVersionCheck.ID:
                    ServerVersionCheck serverVersionCheck = new ServerVersionCheck(fadeReader);

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

                    api.fadeClient.Send(new FadeInitStageOne(serverRequetCode.code));

                    bool initialized = fadeReader.ReadBoolean();

                    if(initialized)
                    {
                        Console.WriteLine("StageOne initialized");
                        api.fadeClient.Send(new FadeRequestCallback());
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
                    api.fadeClient.Send(new FadeInitStageTwo(serverRequestCallback.secretKey));
                    bool initializedStageTwo = fadeReader.ReadBoolean();

                    if(initializedStageTwo)
                    {
                        Console.WriteLine("StageTwo initialized");
                        SendEncoded(new Ping());
                        //SendEncoded(new Login(166211055, "44b9b7359fa4555078d059a2a9bad814", 1, 578)); // awesomek
                        SendEncoded(new Login(165206592, "02eb590f393066d68876ffecd0625d28", 1, 578)); // 0cf5e
                        //SendEncoded(new Login(73464017, "98ddb72404598b960e39cc6f61dbdec5", 1, 578)); // Quake
                    }

                    break;
                case HeroInit.ID:
                    HeroInit heroInit = new HeroInit(fadeReader);
                    Console.WriteLine("{0} {1} {2}/{3} {4}/{5} ({6}) pos ({7}, {8}) {9}/{10}", heroInit.rank, heroInit.userName, heroInit.hp, heroInit.maxHP,
                        heroInit.shield, heroInit.maxShield, heroInit.speed, heroInit.x, heroInit.y, heroInit.freeCargoSpace, heroInit.cargoCapacity);
                    Console.WriteLine("Testing: {0} {1}", heroInit.var_3378, heroInit.galaxyGatesDone);
                    if (!isMoving)
                    {
                        SendEncoded(new Move(1000, 1000, 1222, 707));
                        isMoving = true;
                    }
                    break;
                case ShipInit.ID:
                    ShipInit shipInit = new ShipInit(fadeReader);
                    Console.WriteLine("npc: {0} ship: {1} name: {2}, {3}/{4}", shipInit.npc, shipInit.shipName, shipInit.userName, shipInit.x, shipInit.y);
                    break;
                case ShipMove.ID:
                    ShipMove shipMove = new ShipMove(fadeReader);
                    Console.WriteLine("Ship {0} is moving to {1}/{2} at speed {3}", shipMove.player, shipMove.x, shipMove.y, shipMove.duration);
                    break;
                case Box.ID:
                    Box box = new Box(fadeReader);
                    Console.WriteLine("Box ({0}) at {1}/{2} type {3}", box.hash, box.x, box.y, box.type);
                    break;
                case Ore.ID:
                    Ore ore = new Ore(fadeReader);
                    Console.WriteLine("Ore ({0}) at {1}/{2} of type {3}", ore.hash, ore.x, ore.y, ore.type);
                    break;
                case Collected.ID:
                    Collected collected = new Collected(fadeReader);
                    Console.Write("Someone collected {0} name_44 {1}", collected.hash, collected.name_44);
                    break;
                case 29794:
                    Console.WriteLine("Received pong");
                    fadeReader.ReadBytes(fadeLength - 2);
                    if(!pingThread.IsAlive)
                        pingThread.Start();
                    break;
                case 13944:
                    Console.WriteLine("Received box/cargo {0} {1} {2}", fadeReader.ReadString(), fadeReader.ReadUInt32(), fadeReader.ReadUInt32());
                    break;
                case OldStylePacket.ID:
                    OldStylePacket oldStylePacket = new OldStylePacket(fadeReader);
                    Console.WriteLine("Received old style packet with message: {0}", oldStylePacket.message);
                    break;
                default:
                    Console.WriteLine("Received packet of ID {0} with total size of {1} which is not supported", fadeID, fadeLength + 4);
                    fadeReader.ReadBytes(fadeLength - 2);
                    break;
            }
        }

        private void PingLoop()
        {
            while(true)
            {
                Thread.Sleep(10000);
                SendEncoded(new Ping());
            }
        }
    }
}
