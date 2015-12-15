using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;
using MiscUtil.Conversion;
using PolskaBot.Core.Darkorbit;
using PolskaBot.Core.Darkorbit.Commands;
using PolskaBot.Core.Darkorbit.Commands.PostHandshake;
using System.Threading;

namespace PolskaBot.Core
{
    public class VanillaClient : Client
    {
        Thread pingThread;

        public event EventHandler<ShipMove> ShipMoving;

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
                        SendEncoded(new Login(api.account.userID, api.account.sid, 0, api.account.instanceID));
                        SendEncoded(new Ready());
                        SendEncoded(new InitPacket(1));
                        SendEncoded(new InitPacket(2));
                    }

                    break;
                case BuildingInit.ID:
                    BuildingInit buildingInit = new BuildingInit(fadeReader);
                    break;
                case GateInit.ID:
                    GateInit gateInit = new GateInit(fadeReader);
                    break;
                case HeroInit.ID:
                    HeroInit heroInit = new HeroInit(fadeReader);
                    // Movement
                    api.account.X = (int)heroInit.x;
                    api.account.Y = (int)heroInit.y;

                    // Map statistics
                    api.account.HP = (int)heroInit.hp;
                    api.account.maxHP = (int)heroInit.maxHP;
                    api.account.shield = (int)heroInit.shield;
                    api.account.maxShield = (int)heroInit.maxShield;
                    api.account.nanoHP = (int)heroInit.nanoHP;
                    api.account.maxNanoHP = (int)heroInit.maxNanoHP;
                    api.account.freeCargoSpace = (int)heroInit.freeCargoSpace;
                    api.account.cargoCapacity = (int)heroInit.freeCargoSpace;

                    // Ship
                    api.account.shipName = heroInit.shipName;
                    api.account.speed = (int)heroInit.speed;

                    // Statistics
                    api.account.cloaked = heroInit.cloaked;
                    api.account.jackpot = heroInit.jackpot;
                    api.account.premium = heroInit.premium;
                    api.account.credits = heroInit.credits;
                    api.account.honor = heroInit.honor;
                    api.account.uridium = heroInit.uridium;
                    api.account.XP = heroInit.XP;
                    api.account.level = (int)heroInit.level;
                    api.account.rank = (int)heroInit.rank;

                    // Social
                    api.account.clanID = (int)heroInit.clanID;
                    api.account.clanTag = heroInit.clanTag;
                    api.account.factionID = heroInit.factionID;

                    api.account.ready = true;
                    break;
                case ShipUpdated.ID:
                    ShipUpdated shipUpdated = new ShipUpdated(fadeReader);
                    break;
                case ShipRepaired.ID:
                    ShipRepaired shipRepaired = new ShipRepaired(fadeReader);
                    break;
                case ShipAttacked.ID:
                    ShipAttacked shipAttacked = new ShipAttacked(fadeReader);
                    break;
                case ShipInit.ID:
                    ShipInit shipInit = new ShipInit(fadeReader);
                    Ship newShip = new Ship();
                    newShip.userID = (int)shipInit.userID;
                    newShip.userName = shipInit.userName;
                    newShip.npc = shipInit.npc;

                    // Movement
                    newShip.X = (int)shipInit.x;
                    newShip.Y = (int)shipInit.y;

                    // Ship
                    newShip.shipName = shipInit.shipName;

                    // Statistics
                    newShip.cloaked = shipInit.cloaked;

                    // Social
                    newShip.clanID = (int)shipInit.clanID;
                    newShip.clanTag = shipInit.clanTag;
                    newShip.factionID = (int)shipInit.factionID;
                    api.ships.Add(newShip);
                    break;
                case ShipMove.ID:
                    ShipMove shipMove = new ShipMove(fadeReader);
                    ShipMoving?.Invoke(this, shipMove);
                    break;
                case BoxInit.ID:
                    BoxInit boxInit = new BoxInit(fadeReader);
                    api.boxes.Add(new Box(boxInit.hash, (int)boxInit.x, (int)boxInit.y, boxInit.type));
                    break;
                case DestroyItem.ID:
                    DestroyItem item = new DestroyItem(fadeReader);
                    lock(api.boxes)
                        api.boxes.RemoveAll(box => box.hash == item.hash);
                    lock(api.ores)
                        api.ores.RemoveAll(ore => ore.hash == item.hash);
                    break;
                case DestroyShip.ID:
                    DestroyShip destroyedShip = new DestroyShip(fadeReader);
                    lock(api.ships)
                        api.ships.RemoveAll(ship => ship.userID == destroyedShip.userID);
                    break;
                case OreInit.ID:
                    OreInit oreInit = new OreInit(fadeReader);
                    api.ores.Add(new Ore(oreInit.hash, (int)oreInit.x, (int)oreInit.y, oreInit.type));
                    break;
                case 29794:
                    Console.WriteLine("Received pong");
                    fadeReader.ReadBytes(fadeLength - 2);
                    if(!pingThread.IsAlive)
                        pingThread.Start();
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
