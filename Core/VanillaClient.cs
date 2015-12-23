using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
        public Thread pingThread;

        public event EventHandler<EventArgs> HeroInited;
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
            if (!Running)
                return;

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
            EndianBinaryReader fadeReader = new EndianBinaryReader(EndianBitConverter.Big, api.fadeClient.stream);

            byte[] lengthBuffer = reader.ReadBytes(2);
            ushort fadeLength;
            ushort fadeID;
            byte[] content;

            lock (api.fadeClient.stream)
            {
                if (!IsConnected())
                    return;
                api.fadeClient.Send(new FadeDecodePacket(lengthBuffer));
                fadeLength = fadeReader.ReadUInt16();
            }

            byte[] contentBuffer = reader.ReadBytes(fadeLength);

            lock(api.fadeClient.stream)
            {
                if (!IsConnected())
                    return;
                api.fadeClient.Send(new FadeDecodePacket(contentBuffer));
                fadeID = fadeReader.ReadUInt16();
                content = fadeReader.ReadBytes(fadeLength - 2);
            }

            EndianBinaryReader cachedReader = new EndianBinaryReader(EndianBitConverter.Big, new MemoryStream(content));

            switch (fadeID)
            {
                case ServerVersionCheck.ID:
                    ServerVersionCheck serverVersionCheck = new ServerVersionCheck(cachedReader);

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
                    ServerRequestCode serverRequetCode = new ServerRequestCode(cachedReader);
                    bool initialized;

                    lock(api.fadeClient.stream)
                    {
                        api.fadeClient.Send(new FadeInitStageOne(serverRequetCode.code));

                        initialized = fadeReader.ReadBoolean();
                    }

                    if (initialized)
                    {
                        Console.WriteLine("StageOne initialized");
                        short callbackLength;
                        lock (api.fadeClient.stream)
                        {
                            api.fadeClient.Send(new FadeRequestCallback());
                            callbackLength = fadeReader.ReadInt16();
                            byte[] buffer = fadeReader.ReadBytes(callbackLength);
                            SendEncoded(new ClientRequestCallback(buffer));
                        }
                    }
                    else
                    {
                        Console.WriteLine("StageOne failed");
                    }

                    break;
                case ServerRequestCallback.ID:
                    ServerRequestCallback serverRequestCallback = new ServerRequestCallback(cachedReader);
                    bool initializedStageTwo;
                    lock (api.fadeClient.stream)
                    {
                        api.fadeClient.Send(new FadeInitStageTwo(serverRequestCallback.secretKey));
                        initializedStageTwo = fadeReader.ReadBoolean();
                    }

                    if (initializedStageTwo)
                    {
                        Console.WriteLine("StageTwo initialized");
                        SendEncoded(new Ping());
                        SendEncoded(new Login(api.Account.UserID, api.Account.SID, 0, api.Account.InstanceID));
                        SendEncoded(new Ready());
                        SendEncoded(new InitPacket(1));
                        SendEncoded(new InitPacket(2));
                    }

                    break;
                case BuildingInit.ID:
                    BuildingInit buildingInit = new BuildingInit(cachedReader);
                    api.Buildings.Add(new Building(buildingInit.Name, buildingInit.X, buildingInit.Y));
                    break;
                case GateInit.ID:
                    GateInit gateInit = new GateInit(cachedReader);
                    api.Gates.Add(new Gate(gateInit.GateType, gateInit.X, gateInit.Y));
                    break;
                case ShipDestroyed.ID:
                    ShipDestroyed shipDestroyed = new ShipDestroyed(cachedReader);
                    if(shipDestroyed.UserID == api.Account.UserID)
                    {
                        Console.WriteLine("Our ship got destryoed");
                        SendEncoded(new ReviveShip(api.Account.UserID, api.Account.SID, (short)api.Account.FactionID, 0, 1));
                    }
                    break;
                case Notify.ID:
                    Notify notify = new Notify(cachedReader);
                    if (notify.MessageType == "ttip_killscreen_basic_repair")
                    {
                        Console.WriteLine("Our ship is destryoed");
                        SendEncoded(new ReviveShip(api.Account.UserID, api.Account.SID, (short)api.Account.FactionID, 0, 1));
                    }
                    break;
                case MapChanged.ID:
                    MapChanged mapChanged = new MapChanged(cachedReader);
                    Console.WriteLine($"Map changed to: {mapChanged.MapID} | {mapChanged.var_294}");
                    SendEncoded(new MapChangeConfirmation(true));
                    break;
                case HeroInit.ID:
                    HeroInit heroInit = new HeroInit(cachedReader);

                    api.Boxes.Clear();
                    api.MemorizedBoxes.Clear();
                    api.Ores.Clear();
                    api.Ships.Clear();
                    api.Gates.Clear();
                    api.Buildings.Clear();

                    // Movement
                    api.Account.X = heroInit.X;
                    api.Account.Y = heroInit.Y;

                    // Map statistics
                    api.Account.HP = (int)heroInit.HP;
                    api.Account.MaxHP = (int)heroInit.MaxHP;
                    api.Account.Shield = (int)heroInit.Shield;
                    api.Account.MaxShield = (int)heroInit.MaxShield;
                    api.Account.NanoHP = (int)heroInit.NanoHP;
                    api.Account.MaxNanoHP = (int)heroInit.MaxNanoHP;
                    api.Account.FreeCargoSpace = (int)heroInit.FreeCargoSpace;
                    api.Account.CargoCapacity = (int)heroInit.CargoCapacity;

                    // Ship
                    api.Account.Shipname = heroInit.Shipname;
                    api.Account.Speed = (int)(heroInit.Speed * 0.97);

                    // Statistics
                    api.Account.Cloaked = heroInit.Cloaked;
                    api.Account.Jackpot = heroInit.Jackpot;
                    api.Account.Premium = heroInit.Premium;
                    api.Account.Credits = heroInit.Credits;
                    api.Account.Honor = heroInit.Honor;
                    api.Account.Uridium = heroInit.Uridium;
                    api.Account.XP = heroInit.XP;
                    api.Account.Level = (int)heroInit.Level;
                    api.Account.Rank = (int)heroInit.Rank;

                    // Social
                    api.Account.ClanID = (int)heroInit.ClanID;
                    api.Account.ClanTag = heroInit.ClanTag;
                    api.Account.FactionID = heroInit.FactionID;

                    HeroInited?.Invoke(this, EventArgs.Empty);
                    api.Account.Ready = true;
                    break;
                case DroneFormationUpdated.ID:
                    DroneFormationUpdated droneFormationUpdated = new DroneFormationUpdated(cachedReader);
                    break;
                case ShipUpdated.ID:
                    ShipUpdated shipUpdated = new ShipUpdated(cachedReader);
                    api.Account.UpdateHitpointsAndShield(shipUpdated.HP, shipUpdated.Shield, shipUpdated.NanoHP);
                    break;
                case ShieldUpdated.ID:
                    ShieldUpdated shieldUpdated = new ShieldUpdated(cachedReader);
                    api.Account.UpdateShield(shieldUpdated.Shield, shieldUpdated.MaxShield);
                    break;
                case CargoUpdated.ID:
                    CargoUpdated cargoUpdated = new CargoUpdated(cachedReader);
                    api.Account.FreeCargoSpace = api.Account.CargoCapacity - (int)cargoUpdated.CargoCount;
                    break;
                case HitpointsUpdated.ID:
                    HitpointsUpdated hitpointsUpdated = new HitpointsUpdated(cachedReader);
                    api.Account.UpdateHitpoints(hitpointsUpdated.HP, hitpointsUpdated.MaxHP, hitpointsUpdated.NanoHP, hitpointsUpdated.MaxNanoHP);
                    break;
                case ShipAttacked.ID:
                    ShipAttacked shipAttacked = new ShipAttacked(cachedReader);
                    Attacked?.Invoke(this, shipAttacked);
                    break;
                case ShipInit.ID:
                    ShipInit shipInit = new ShipInit(cachedReader);
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
                    api.Ships.Add(newShip);
                    break;
                case ShipMove.ID:
                    ShipMove shipMove = new ShipMove(cachedReader);
                    ShipMoving?.Invoke(this, shipMove);
                    break;
                case BoxInit.ID:
                    BoxInit boxInit = new BoxInit(cachedReader);
                    if (boxInit.Hash.Length != 5)
                    {
                        Box box = new Box(boxInit.Hash, boxInit.X, boxInit.Y, boxInit.Type);
                        api.Boxes.Add(box);
                        api.MemorizedBoxes.Add(box);
                    }
                    break;
                case DestroyItem.ID:
                    DestroyItem item = new DestroyItem(cachedReader);
                    lock (api.Boxes)
                    {
                        api.Boxes.RemoveAll(box => box.Hash == item.Hash);
                        if(item.CollectedByPlayer)
                            api.MemorizedBoxes.RemoveAll(box => box.Hash == item.Hash);
                    }
                    lock (api.Ores)
                        api.Ores.RemoveAll(ore => ore.Hash == item.Hash);
                    break;
                case DestroyShip.ID:
                    DestroyShip destroyedShip = new DestroyShip(cachedReader);
                    lock (api.Ships)
                        api.Ships.RemoveAll(ship => ship.UserID == destroyedShip.UserID);
                    break;
                case OreInit.ID:
                    OreInit oreInit = new OreInit(cachedReader);
                    api.Ores.Add(new Ore(oreInit.Hash, oreInit.X, oreInit.Y, oreInit.Type));
                    break;
                case 23240:
                    if (!pingThread.IsAlive)
                    {
                        pingThread = new Thread(new ThreadStart(PingLoop));
                        pingThread.Start();
                    }
                    break;
                case OldStylePacket.ID:
                    OldStylePacket oldStylePacket = new OldStylePacket(cachedReader);
                    string[] splittedMessage = oldStylePacket.Message.Split('|');
                    switch (splittedMessage[1])
                    {
                        case OldPackets.SELECT:
                            switch (splittedMessage[2])
                            {
                                case OldPackets.CONFIG:
                                    api.Account.Config = Convert.ToInt32(splittedMessage[3]);
                                    break;
                            }
                            break;
                        case OldPackets.PORTAL_JUMP:
                            Console.WriteLine($"(Old) Map changed to: {splittedMessage[2]}");
                            SendEncoded(new MapChangeConfirmation(true));
                            break;
                        case OldPackets.LOG_MESSAGE:
                            switch(splittedMessage[3])
                            {
                                case OldPackets.BOX_CONTENT_CREDITS:
                                    api.Account.CollectedCredits += double.Parse(splittedMessage[4]);
                                    break;
                                case OldPackets.BOX_CONTENT_URIDIUM:
                                    api.Account.CollectedUridium += double.Parse(splittedMessage[4]);
                                    break;
                                case OldPackets.BOX_CONTENT_EE:
                                    api.Account.CollectedEE += int.Parse(splittedMessage[4]);
                                    break;
                                case OldPackets.BOX_CONTENT_XP:
                                    api.Account.CollectedXP += double.Parse(splittedMessage[4]);
                                    break;
                                case OldPackets.BOX_CONTENT_HON:
                                    api.Account.CollectedHonor += double.Parse(splittedMessage[4]);
                                    break;
                            }
                            LogMessage?.Invoke(this, string.Join("|", splittedMessage));
                            break;
                        default:
                            Console.WriteLine("Received unsupported old style packet with message: {0}", oldStylePacket.Message);
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Received packet of ID {0} with total size of {1} which is not supported", fadeID, fadeLength + 4);
                    break;
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
