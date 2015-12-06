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
        public const ushort ID = 21460;

        public float jackpot { get; private set; }
        public uint maxShield { get; private set; } //name_101
        public bool premium { get; private set; }
        public bool var_4832 { get; private set; }
        public double credits { get; private set; }
        public double honor { get; private set; } //var_4054
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
        public uint galaxyGatesDone { get; private set; }
        public uint freeCargoSpace { get; private set; } //var_4295
        public string shipName { get; private set; } //name_122
        public uint hp { get; private set; } //var_1060
        public uint level { get; private set; }
        public uint nanoHP { get; private set; } //var_2222
        public double pd { get; private set; } //var_4549
        public uint mapId { get; private set; }
        public uint factionID { get; private set; }
        public string clanTag { get; private set; } //name_138
        public uint maxHP { get; private set; } //var_1848
        public uint maxNanoHP { get; private set; }

        public HeroInit(EndianBinaryReader reader)
        {
            Console.WriteLine("Reading heroInit");
            for (int i = 0; i < reader.ReadInt32(); i++)
            {
                class_326 class326 = new class_326(reader);
            }
            this.jackpot = reader.ReadSingle(); //jackpot
            this.maxShield = reader.ReadUInt32();
            this.maxShield = this.maxShield >> 6 | this.maxShield << 26;
            this.premium = reader.ReadBoolean();
            this.var_4832 = reader.ReadBoolean();
            this.credits = reader.ReadDouble();
            this.honor = reader.ReadDouble();
            this.clanID = reader.ReadUInt32();
            this.clanID = this.clanID >> 2 | this.clanID << 30;
            this.uridium = reader.ReadDouble();
            this.var_3674 = reader.ReadBoolean();
            this.rank = reader.ReadUInt32();
            this.rank = this.rank >> 5 | this.rank << 27;
            this.cloaked = reader.ReadBoolean();
            this.userName = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            this.speed = reader.ReadUInt32();
            this.speed = this.speed << 3 | this.speed >> 29;
            this.cargoCapacity = reader.ReadUInt32();
            this.cargoCapacity = this.cargoCapacity >> 15 | this.cargoCapacity << 17;
            this.shield = reader.ReadUInt32();
            this.shield = this.shield << 2 | this.shield >> 30;
            this.x = reader.ReadUInt32();
            this.x = this.x >> 14 | this.x << 18;
            this.userID = reader.ReadUInt32();
            this.userID = this.userID << 11 | this.userID >> 21;
            this.var_3378 = reader.ReadUInt32();
            this.var_3378 = this.var_3378 << 16 | this.var_3378 >> 16;
            this.galaxyGatesDone = reader.ReadUInt32();
            this.galaxyGatesDone = this.galaxyGatesDone >> 3 | this.galaxyGatesDone << 29;
            this.freeCargoSpace = reader.ReadUInt32();
            this.freeCargoSpace = this.freeCargoSpace >> 11 | this.freeCargoSpace << 21;
            this.shipName = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            this.hp = reader.ReadUInt32();
            this.hp = this.hp << 5 | this.hp >> 27;
            this.level = reader.ReadUInt32();
            this.level = this.level >> 2 | this.level << 30;
            this.nanoHP = reader.ReadUInt32();
            this.nanoHP = this.nanoHP >> 5 | this.nanoHP << 27;
            this.pd = reader.ReadDouble();
            this.mapId = reader.ReadUInt32();
            this.mapId = this.mapId << 16 | this.mapId >> 16;
            this.factionID = reader.ReadUInt32();
            this.factionID = this.factionID << 9 | this.factionID >> 23;
            reader.ReadInt16();
            this.y = reader.ReadUInt32();
            this.y = this.y << 11 | this.y >> 21;
            this.clanTag = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            this.maxHP = reader.ReadUInt32();
            this.maxHP = this.maxHP >> 1 | this.maxHP << 31;
            this.maxNanoHP = reader.ReadUInt32();
            this.maxNanoHP = this.maxNanoHP >> 16 | this.maxNanoHP << 16;
        }

        public override void Write()
        {
            return;
        }

    }
}