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
using PolskaBot.Fade;

namespace PolskaBot.Core
{
    class VanillaClient : Client
    {

        private FadeProxyClient _proxy;
        private RemoteClient _remoteClient;

        public Thread pingThread;

        public event EventHandler<EventArgs> HeroInited;
        public event EventHandler<EventArgs> Compatible;
        public event EventHandler<EventArgs> NotCompatible;
        public event EventHandler<ShipAttacked> Attacked;
        public event EventHandler<ShipMove> ShipMoving;
        public event EventHandler<EventArgs> Destroyed;

        public event EventHandler<EventArgs> AuthFailed;

        public event EventHandler<string> LogMessage;

        public VanillaClient(API api, FadeProxyClient proxy, RemoteClient remoteClient) : base(api)
        {
            _proxy = proxy;

            _proxy.StageOneLoaded += (s, e) =>
            {
                byte[] secret = proxy.GenerateKey();
                SendEncoded(new ClientRequestCallback(secret));
            };

            _proxy.StageOneFailed += (s, e) => Console.WriteLine("StageOne failed");

            _remoteClient = remoteClient;
            pingThread = new Thread(new ThreadStart(PingLoop));
        }

        public override void Stop()
        {
            base.Stop();
            _proxy.Disconnect();
        }

        public void SendEncoded(Command command)
        {
            if (!Running)
                return;

            Send(_proxy.Encrypt(command.ToArray()));
        }

        public override void Parse(EndianBinaryReader reader)
        {
            ushort length;
            ushort id;
            byte[] content;

            if (!IsConnected() || tcpClient.Available == 0)
                return;

            var lengthBuffer = reader.ReadBytes(2);

            lengthBuffer = _proxy.Decrypt(lengthBuffer);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(lengthBuffer);
            length = BitConverter.ToUInt16(lengthBuffer, 0);

            if (!IsConnected())
                return;

            content = _proxy.Decrypt(reader.ReadBytes(length));

            EndianBinaryReader cachedReader = new EndianBinaryReader(EndianBitConverter.Big, new MemoryStream(content));

            id = cachedReader.ReadUInt16();

            switch (id)
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

                    lock(_remoteClient.locker)
                    {
                        _remoteClient.Send(new RemoteInitStageOne(serverRequetCode.code, api.Account.UserID));
                        EndianBinaryReader remoteReader = new EndianBinaryReader(EndianBitConverter.Big, _remoteClient.stream);
                        short remoteLength = remoteReader.ReadInt16();
                        if(remoteReader.ReadInt16() == 102)
                        {
                            Console.WriteLine("Received stageOne code response");
                            if (remoteLength != 3)
                                _proxy.InitStageOne(remoteReader.ReadBytes(remoteLength - 2));
                            else
                                AuthFailed?.Invoke(this, EventArgs.Empty);
                        }
                    }

                    break;
                case ServerRequestCallback.ID:
                    Console.WriteLine("Stage two received");
                    ServerRequestCallback serverRequestCallback = new ServerRequestCallback(cachedReader);

                    _proxy.InitStageTwo(serverRequestCallback.secretKey);

                    Console.WriteLine("StageTwo initialized");
                    SendEncoded(new Ping());
                    SendEncoded(new Login(api.Account.UserID, api.Account.SID, 0, api.Account.InstanceID));
                    SendEncoded(new Ready());
                    break;
                case BuildingInit.ID:
                    BuildingInit buildingInit = new BuildingInit(cachedReader);
                    lock(api.buildingsLocker)
                        api.Buildings.Add(new Building(buildingInit.BuildingID, buildingInit.Name, buildingInit.X, buildingInit.Y, buildingInit.AssetType));
                    break;
                case DestroyBuilding.ID:
                    DestroyBuilding destroyBuilding = new DestroyBuilding(cachedReader);
                    lock(api.buildingsLocker)
                        api.Buildings.RemoveAll(building => building.BuildingID == destroyBuilding.BuildingID);
                    break;
                case GateInit.ID:
                    GateInit gateInit = new GateInit(cachedReader);
                    api.Gates.Add(new Gate(gateInit.GateType, gateInit.X, gateInit.Y));
                    break;
                case ShipDestroyed.ID:
                    ShipDestroyed shipDestroyed = new ShipDestroyed(cachedReader);
                    if(shipDestroyed.UserID == api.Account.UserID)
                    {
                        Destroyed?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                case Notify.ID:
                    Notify notify = new Notify(cachedReader);
                    if (notify.MessageType == "ttip_killscreen_basic_repair")
                    {
                        Destroyed?.Invoke(this, EventArgs.Empty);
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
                    Task.Delay(15000).ContinueWith(_ => api.Account.JumpAllowed = true);

                    SendEncoded(new InitPacket(1));
                    SendEncoded(new InitPacket(2));
                    break;
                case 31982: //CpuInitializationCommand
                    SendEncoded(new OldStylePacket("JCPU|GET"));
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
                    lock(api.shipsLocker)
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
                        lock(api.boxesLocker) lock (api.memorizedBoxesLocker)
                        {
                            api.Boxes.Add(box);
                            api.MemorizedBoxes.Add(box);
                        }
                    }
                    break;
                case DestroyItem.ID:
                    DestroyItem item = new DestroyItem(cachedReader);
                    lock (api.boxesLocker) lock (api.memorizedBoxesLocker)
                    {
                        api.Boxes.RemoveAll(box => box.Hash == item.Hash);
                        if(item.CollectedByPlayer)
                            api.MemorizedBoxes.RemoveAll(box => box.Hash == item.Hash);
                    }
                    lock (api.oresLocker)
                        api.Ores.RemoveAll(ore => ore.Hash == item.Hash);
                    break;
                case DestroyShip.ID:
                    DestroyShip destroyedShip = new DestroyShip(cachedReader);
                    lock (api.Ships)
                        api.Ships.RemoveAll(ship => ship.UserID == destroyedShip.UserID);
                    break;
                case OreInit.ID:
                    OreInit oreInit = new OreInit(cachedReader);
                    lock(api.oresLocker)
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
                    Console.WriteLine("Received packet of ID {0} with total size of {1} which is not supported", id, length + 4);
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
