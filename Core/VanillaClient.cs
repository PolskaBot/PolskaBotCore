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

        public event EventHandler<EventArgs> Compatible;
        public event EventHandler<EventArgs> NotCompatible;
        public event EventHandler<ShipAttacked> Attacked;
        public event EventHandler<ShipMove> ShipMoving;

        public event EventHandler<string> LogMessage;

        public VanillaClient(API api) : base(api)
        {
            pingThread = new Thread(new ThreadStart(PingLoop));
        }

        public void SendEncoded(Command command)
        {
            byte[] rawBuffer = command.ToArray();
            byte[] encodedBuffer = new byte[rawBuffer.Length];
            lock (api.fadeClient.stream)
            {
                api.fadeClient.Send(new FadeEncodePacket(rawBuffer));
                api.fadeClient.stream.Read(encodedBuffer, 0, rawBuffer.Length);
            }
            Send(encodedBuffer);
        }

        public override void Parse(EndianBinaryReader reader)
        {
            lock(api.fadeClient.stream)
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
                            Compatible?.Invoke(this, EventArgs.Empty);
                            Send(new ClientRequestCode());
                        }
                        else
                        {
                            NotCompatible?.Invoke(this, EventArgs.Empty);
                            thread.Abort();
                        }
                        break;

                    case ServerRequestCode.ID:
                        ServerRequestCode serverRequetCode = new ServerRequestCode(fadeReader);

                        api.fadeClient.Send(new FadeInitStageOne(serverRequetCode.code));

                        bool initialized = fadeReader.ReadBoolean();

                        if (initialized)
                        {
                            Console.WriteLine("StageOne initialized");
                            api.fadeClient.Send(new FadeRequestCallback());
                            short callbackLength = fadeReader.ReadInt16();
                            byte[] buffer = fadeReader.ReadBytes(callbackLength);
                            SendEncoded(new ClientRequestCallback(buffer));
                        }
                        else
                        {
                            Console.WriteLine("StageOne failed");
                        }

                        break;
                    case ServerRequestCallback.ID:
                        ServerRequestCallback serverRequestCallback = new ServerRequestCallback(fadeReader);
                        api.fadeClient.Send(new FadeInitStageTwo(serverRequestCallback.secretKey));
                        bool initializedStageTwo = fadeReader.ReadBoolean();

                        if (initializedStageTwo)
                        {
                            Console.WriteLine("StageTwo initialized");
                            SendEncoded(new Ping());
                            SendEncoded(new Login(api.account.UserID, api.account.SID, 0, api.account.InstanceID));
                            SendEncoded(new Ready());
                            SendEncoded(new InitPacket(1));
                            SendEncoded(new InitPacket(2));
                        }

                        break;
                    case BuildingInit.ID:
                        BuildingInit buildingInit = new BuildingInit(fadeReader);
                        api.buildings.Add(new Building(buildingInit.Name, buildingInit.X, buildingInit.Y));
                        break;
                    case GateInit.ID:
                        GateInit gateInit = new GateInit(fadeReader);
                        api.gates.Add(new Gate(gateInit.GateType, gateInit.X, gateInit.Y));
                        break;
                    case HeroInit.ID:
                        HeroInit heroInit = new HeroInit(fadeReader);
                        // Movement
                        api.account.X = heroInit.X;
                        api.account.Y = heroInit.Y;

                        // Map statistics
                        api.account.HP = (int)heroInit.HP;
                        api.account.MaxHP = (int)heroInit.MaxHP;
                        api.account.Shield = (int)heroInit.Shield;
                        api.account.MaxShield = (int)heroInit.MaxShield;
                        api.account.NanoHP = (int)heroInit.NanoHP;
                        api.account.MaxNanoHP = (int)heroInit.MaxNanoHP;
                        api.account.FreeCargoSpace = (int)heroInit.FreeCargoSpace;
                        api.account.CargoCapacity = (int)heroInit.CargoCapacity;

                        // Ship
                        api.account.Shipname = heroInit.Shipname;
                        api.account.Speed = (int)(heroInit.Speed * 0.97);

                        // Statistics
                        api.account.Cloaked = heroInit.Cloaked;
                        api.account.Jackpot = heroInit.Jackpot;
                        api.account.Premium = heroInit.Premium;
                        api.account.Credits = heroInit.Credits;
                        api.account.Honor = heroInit.Honor;
                        api.account.Uridium = heroInit.Uridium;
                        api.account.XP = heroInit.XP;
                        api.account.Level = (int)heroInit.Level;
                        api.account.Rank = (int)heroInit.Rank;

                        // Social
                        api.account.ClanID = (int)heroInit.ClanID;
                        api.account.ClanTag = heroInit.ClanTag;
                        api.account.FactionID = heroInit.FactionID;

                        api.account.Ready = true;
                        break;
                    case DroneFormationUpdated.ID:
                        DroneFormationUpdated droneFormationUpdated = new DroneFormationUpdated(fadeReader);
                        break;
                    case ShipUpdated.ID:
                        ShipUpdated shipUpdated = new ShipUpdated(fadeReader);
                        api.account.UpdateHitpointsAndShield(shipUpdated.HP, shipUpdated.Shield, shipUpdated.NanoHP);
                        break;
                    case ShieldUpdated.ID:
                        ShieldUpdated shieldUpdated = new ShieldUpdated(fadeReader);
                        api.account.UpdateShield(shieldUpdated.Shield, shieldUpdated.MaxShield);
                        break;
                    case HitpointsUpdated.ID:
                        HitpointsUpdated hitpointsUpdated = new HitpointsUpdated(fadeReader);
                        api.account.UpdateHitpoints(hitpointsUpdated.HP, hitpointsUpdated.MaxHP, hitpointsUpdated.NanoHP, hitpointsUpdated.MaxNanoHP);
                        break;
                    case ShipAttacked.ID:
                        ShipAttacked shipAttacked = new ShipAttacked(fadeReader);
                        Attacked?.Invoke(this, shipAttacked);
                        break;
                    case ShipInit.ID:
                        ShipInit shipInit = new ShipInit(fadeReader);
                        Ship newShip = new Ship();
                        newShip.UserID = (int)shipInit.UserID;
                        newShip.Username = shipInit.Username;
                        newShip.NPC = shipInit.NPC;

                        // Movement
                        newShip.X = shipInit.X;
                        newShip.Y = shipInit.Y;

                        // Ship
                        newShip.Shipname = shipInit.Shipname;

                        // Statistics
                        newShip.Cloaked = shipInit.Cloaked;

                        // Social
                        newShip.ClanID = (int)shipInit.ClanID;
                        newShip.ClanTag = shipInit.ClanTag;
                        newShip.FactionID = (int)shipInit.FactionID;
                        api.ships.Add(newShip);
                        break;
                    case ShipMove.ID:
                        ShipMove shipMove = new ShipMove(fadeReader);
                        ShipMoving?.Invoke(this, shipMove);
                        break;
                    case BoxInit.ID:
                        BoxInit boxInit = new BoxInit(fadeReader);
                        if(boxInit.Hash.Length != 5)
                            api.boxes.Add(new Box(boxInit.Hash, boxInit.X, boxInit.Y, boxInit.Type));
                        break;
                    case DestroyItem.ID:
                        DestroyItem item = new DestroyItem(fadeReader);
                        lock (api.boxes)
                            api.boxes.RemoveAll(box => box.Hash == item.Hash);
                        lock (api.ores)
                            api.ores.RemoveAll(ore => ore.Hash == item.Hash);
                        break;
                    case DestroyShip.ID:
                        DestroyShip destroyedShip = new DestroyShip(fadeReader);
                        lock (api.ships)
                            api.ships.RemoveAll(ship => ship.UserID == destroyedShip.UserID);
                        break;
                    case OreInit.ID:
                        OreInit oreInit = new OreInit(fadeReader);
                        api.ores.Add(new Ore(oreInit.Hash, oreInit.X, oreInit.Y, oreInit.Type));
                        break;
                    case 29794:
                        Console.WriteLine("Received pong");
                        fadeReader.ReadBytes(fadeLength - 2);
                        if (!pingThread.IsAlive)
                            pingThread.Start();
                        break;
                    case OldStylePacket.ID:
                        OldStylePacket oldStylePacket = new OldStylePacket(fadeReader);
                        string[] splittedMessage = oldStylePacket.Message.Split('|');
                        switch (splittedMessage[1])
                        {
                            case OldPackets.SELECT:
                                switch (splittedMessage[2])
                                {
                                    case OldPackets.CONFIG:
                                        api.account.Config = Convert.ToInt32(splittedMessage[3]);
                                        break;
                                }
                                break;
                            case OldPackets.LOG_MESSAGE:
                                LogMessage?.Invoke(this, "Collected box");
                                switch(splittedMessage[3])
                                {
                                    case OldPackets.BOX_CONTENT_CREDITS:
                                        api.account.CollectedCredits += double.Parse(splittedMessage[4]);
                                        break;
                                    case OldPackets.BOX_CONTENT_URIDIUM:
                                        api.account.CollectedUridium += double.Parse(splittedMessage[4]);
                                        break;
                                    case OldPackets.BOX_CONTENT_XP:
                                        api.account.CollectedXP += double.Parse(splittedMessage[4]);
                                        break;
                                    case OldPackets.BOX_CONTENT_HON:
                                        api.account.CollectedHonor += double.Parse(splittedMessage[4]);
                                        break;
                                    case OldPackets.BOX_CONTENT_EE:
                                        Console.WriteLine(splittedMessage);
                                        api.account.CollectedEE++;
                                        break;
                                }
                                break;
                            default:
                                Console.WriteLine("Received unsupported old style packet with message: {0}", oldStylePacket.Message);
                                break;
                        }
                        break;
                    default:
                        Console.WriteLine("Received packet of ID {0} with total size of {1} which is not supported", fadeID, fadeLength + 4);
                        fadeReader.ReadBytes(fadeLength - 2);
                        break;
                }
            }
        }

        private void PingLoop()
        {
            while(true)
            {
                Thread.Sleep(1000);
                SendEncoded(new Ping());
            }
        }
    }
}
