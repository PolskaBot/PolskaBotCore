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

        public float Jackpot { get; private set; }
        public uint MaxShield { get; private set; } //name_101
        public bool Premium { get; private set; }
        public bool var_4831 { get; private set; }
        public double Credits { get; private set; }
        public double Honor { get; private set; } //var_4055
        public uint ClanID { get; private set; } //name_46
        public double Uridium { get; private set; }
        public bool var_3674 { get; private set; }
        public uint Rank { get; private set; } //name_134
        public bool Cloaked { get; private set; }
        public string Username { get; private set; } //var_3497
        public uint Speed { get; private set; }
        public uint CargoCapacity { get; private set; } //var_3017
        public uint Shield { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public uint UserID { get; private set; } //name_125
        public uint var_3378 { get; private set; }
        public uint var_3911 { get; private set; } //var_3911
        public uint FreeCargoSpace { get; private set; } //var_4295
        public string Shipname { get; private set; } //name_122
        public uint HP { get; private set; } //var_1063
        public uint Level { get; private set; }
        public uint NanoHP { get; private set; } //var_2222
        public double XP { get; private set; } //var_4549
        public uint Map { get; private set; }
        public uint FactionID { get; private set; }
        public string ClanTag { get; private set; } //name_138
        public uint MaxHP { get; private set; } //var_1851
        public uint MaxNanoHP { get; private set; } //var_1600

        public HeroInit(EndianBinaryReader reader)
        {
            CargoCapacity = reader.ReadUInt32();
            CargoCapacity = CargoCapacity << 12 | CargoCapacity >> 20;
            Speed = reader.ReadUInt32();
            Speed = Speed << 11 | Speed >> 21;
            Username = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            var_4831 = reader.ReadBoolean();
            HP = reader.ReadUInt32();
            HP = HP >> 1 | HP << 31;
            Jackpot = reader.ReadSingle();
            Premium = reader.ReadBoolean();
            ClanID = reader.ReadUInt32();
            ClanID = ClanID << 11 | ClanID >> 21;
            Y = reader.ReadInt32();
            Y = (int)((uint)Y >> 6 | (uint)Y << 26);
            Shipname = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            Shield = reader.ReadUInt32();
            Shield = Shield << 9 | Shield >> 23;
            var_3911 = reader.ReadUInt32();
            var_3911 = var_3911 >> 5 | var_3911 << 27;
            reader.ReadInt16();
            ClanTag = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            Map = reader.ReadUInt32();
            Map = Map << 14 | Map >> 18;
            MaxShield = reader.ReadUInt32();
            MaxShield = MaxShield >> 9 | MaxShield << 23;
            Level = reader.ReadUInt32();
            Level = Level << 7 | Level >> 25;
            X = reader.ReadInt32();
            X = (int)((uint)X >> 5 | (uint)X << 27);
            XP = reader.ReadDouble();
            FactionID = reader.ReadUInt32();
            FactionID = FactionID << 5 | FactionID >> 27;
            FreeCargoSpace = reader.ReadUInt32();
            FreeCargoSpace = FreeCargoSpace << 3 | FreeCargoSpace >> 29;
            Honor = reader.ReadDouble();
            Uridium = reader.ReadDouble();
            MaxNanoHP = reader.ReadUInt32();
            MaxNanoHP = MaxNanoHP << 5 | MaxNanoHP >> 27;
            Rank = reader.ReadUInt32();
            Rank = Rank >> 13 | Rank << 19;
            UserID = reader.ReadUInt32();
            UserID = UserID >> 11 | UserID << 21;
            MaxHP = reader.ReadUInt32();
            MaxHP = MaxHP >> 14 | MaxHP << 18;
            int length = reader.ReadInt32();
            if (length > 0)
            {
                reader.ReadInt16();
                for (int i = 0; i < length; i++)
                {
                    Class326 class326 = new Class326(reader);
                }
            }
            Cloaked = reader.ReadBoolean();
            var_3378 = reader.ReadUInt32();
            var_3378 = var_3378 << 9 | var_3378 >> 23;
            NanoHP = reader.ReadUInt32();
            NanoHP = NanoHP << 3 | NanoHP >> 29;
            Credits = reader.ReadDouble();
            var_3674 = reader.ReadBoolean();
        }
    }
}