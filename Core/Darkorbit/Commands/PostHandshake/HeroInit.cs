using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class HeroInit : Command
    {
        public const ushort ID = 11451;

        public float jackpot { get; private set; }
        public uint maxShield { get; private set; } //name_101
        public bool premium { get; private set; }
        public bool var_4831 { get; private set; }
        public double credits { get; private set; }
        public double honor { get; private set; } //var_4055
        public uint clanID { get; private set; } //name_46
        public double uridium { get; private set; }
        public bool var_3674 { get; private set; }
        public uint rank { get; private set; } //name_134
        public bool cloaked { get; private set; }
        public string userName { get; private set; } //var_3497
        public uint speed { get; private set; }
        public uint cargoCapacity { get; private set; } //var_3017
        public uint shield { get; private set; }
        public uint x { get; private set; }
        public uint y { get; private set; }
        public uint userID { get; private set; } //name_125
        public uint var_3378 { get; private set; }
        public uint galaxyGatesDone { get; private set; } //var_3911
        public uint freeCargoSpace { get; private set; } //var_4295
        public string shipName { get; private set; } //name_122
        public uint hp { get; private set; } //var_1063
        public uint level { get; private set; }
        public uint nanoHP { get; private set; } //var_2222
        public double pd { get; private set; } //var_4549
        public uint mapId { get; private set; }
        public uint factionID { get; private set; }
        public string clanTag { get; private set; } //name_138
        public uint maxHP { get; private set; } //var_1851
        public uint maxNanoHP { get; private set; } //var_1600

        public HeroInit(EndianBinaryReader reader)
        {
            this.cargoCapacity = reader.ReadUInt32();
            this.cargoCapacity = this.cargoCapacity << 12 | this.cargoCapacity >> 20;
            this.speed = reader.ReadUInt32();
            this.speed = this.speed << 11 | this.speed >> 21;
            this.userName = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            this.var_4831 = reader.ReadBoolean();
            this.hp = reader.ReadUInt32();
            this.hp = this.hp >> 1 | this.hp << 31;
            this.jackpot = reader.ReadSingle();
            this.premium = reader.ReadBoolean();
            this.clanID = reader.ReadUInt32();
            this.clanID = this.clanID << 11 | this.clanID >> 21;
            this.y = reader.ReadUInt32();
            this.y = this.y >> 6 | this.y << 26;
            this.shipName = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            this.shield = reader.ReadUInt32();
            this.shield = this.shield << 9 | this.shield >> 23;
            this.galaxyGatesDone = reader.ReadUInt32();
            this.galaxyGatesDone = this.galaxyGatesDone >> 5 | this.galaxyGatesDone << 27;
            reader.ReadInt16();
            this.clanTag = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            this.mapId = reader.ReadUInt32();
            this.mapId = this.mapId << 14 | this.mapId >> 18;
            this.maxShield = reader.ReadUInt32();
            this.maxShield = this.maxShield >> 9 | this.maxShield << 23;
            this.level = reader.ReadUInt32();
            this.level = this.level << 7 | this.level >> 25;
            this.x = reader.ReadUInt32();
            this.x = this.x >> 5 | this.x << 27;
            this.pd = reader.ReadDouble();
            this.factionID = reader.ReadUInt32();
            this.factionID = this.factionID << 5 | this.factionID >> 27;
            this.freeCargoSpace = reader.ReadUInt32();
            this.freeCargoSpace = this.freeCargoSpace << 3 | this.freeCargoSpace >> 29;
            this.honor = reader.ReadDouble();
            this.uridium = reader.ReadDouble();
            this.maxNanoHP = reader.ReadUInt32();
            this.maxNanoHP = this.maxNanoHP << 5 | this.maxNanoHP >> 27;
            this.rank = reader.ReadUInt32();
            this.rank = this.rank >> 13 | this.rank << 19;
            this.userID = reader.ReadUInt32();
            this.userID = this.userID >> 11 | this.userID << 21;
            this.maxHP = reader.ReadUInt32();
            this.maxHP = this.maxHP >> 14 | this.maxHP << 18;
            for (int i = 0; i < reader.ReadInt32(); i++)
            {
                class_326 class326 = new class_326(reader);
            }
            this.cloaked = reader.ReadBoolean();
            this.var_3378 = reader.ReadUInt32();
            this.var_3378 = this.var_3378 << 9 | this.var_3378 >> 23;
            this.nanoHP = reader.ReadUInt32();
            this.nanoHP = this.nanoHP << 3 | this.nanoHP >> 29;
            this.credits = reader.ReadDouble();
            this.var_3674 = reader.ReadBoolean();
        }

        public override void Write()
        {
            return;
        }

    }
}