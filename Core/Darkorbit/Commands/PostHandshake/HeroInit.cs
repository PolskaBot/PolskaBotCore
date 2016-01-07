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
        public const ushort ID = 23292;

        public float Jackpot { get; private set; }
        public uint MaxShield { get; private set; } //name_103
        public bool Premium { get; private set; }
        public bool var_4823 { get; private set; }
        public double Credits { get; private set; }
        public double Honor { get; private set; } //var_4055
        public uint ClanID { get; private set; } //name_48
        public double Uridium { get; private set; }
        public bool var_3678 { get; private set; }
        public uint Rank { get; private set; } //name_134
        public bool Cloaked { get; private set; }
        public string Username { get; private set; } //var_3495
        public uint Speed { get; private set; }
        public uint CargoCapacity { get; private set; } //var_3020
        public uint Shield { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public uint UserID { get; private set; } //name_125
        public uint var_3377 { get; private set; }
        public uint var_3914 { get; private set; } //var_3914
        public uint FreeCargoSpace { get; private set; } //var_4296
        public string Shipname { get; private set; } //name_122
        public uint HP { get; private set; } //var_1065
        public uint Level { get; private set; }
        public uint NanoHP { get; private set; } //var_2224
        public double XP { get; private set; } //var_4549
        public uint Map { get; private set; }
        public uint FactionID { get; private set; }
        public string ClanTag { get; private set; } //name_138
        public uint MaxHP { get; private set; } //var_1851
        public uint MaxNanoHP { get; private set; } //var_1600

        public HeroInit(EndianBinaryReader reader)
        {
            var_3377 = reader.ReadUInt32();
            var_3377 = var_3377 << 11 | var_3377 >> 21;
            X = reader.ReadInt32();
            X = (int)((uint)X << 11 | (uint)X >> 21);
            Rank = reader.ReadUInt32();
            Rank = Rank >> 2 | Rank << 30;
            XP = reader.ReadDouble();
            Credits = reader.ReadDouble();
            int length = reader.ReadInt32();
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    reader.ReadInt16();
                    Class326 class326 = new Class326(reader);
                }
            }
            FreeCargoSpace = reader.ReadUInt32();
            FreeCargoSpace = FreeCargoSpace >> 4 | FreeCargoSpace << 28;
            CargoCapacity = reader.ReadUInt32();
            CargoCapacity = CargoCapacity >> 11 | CargoCapacity << 21;
            var_3914 = reader.ReadUInt32();
            var_3914 = var_3914 >> 2 | var_3914 << 30;
            var_4823 = reader.ReadBoolean();
            Honor = reader.ReadDouble();
            NanoHP = reader.ReadUInt32();
            NanoHP = NanoHP << 16 | NanoHP >> 16;
            Uridium = reader.ReadDouble();
            Speed = reader.ReadUInt32();
            Speed = Speed << 4 | Speed >> 28;
            Cloaked = reader.ReadBoolean();
            Level = reader.ReadUInt32();
            Level = Level << 2 | Level >> 30;
            Shield = reader.ReadUInt32();
            Shield = Shield >> 16 | Shield << 16;
            Shipname = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            FactionID = reader.ReadUInt32();
            FactionID = FactionID << 10 | FactionID >> 22;
            ClanTag = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            Premium = reader.ReadBoolean();
            reader.ReadUInt32();
            UserID = reader.ReadUInt32();
            UserID = UserID >> 16 | UserID << 16;
            var_3678 = reader.ReadBoolean();
            Map = reader.ReadUInt32();
            Map = Map >> 10 | Map << 22;
            Y = reader.ReadInt32();
            Y = (int)((uint)Y << 8 | (uint)Y >> 24);
            MaxHP = reader.ReadUInt32();
            MaxHP = MaxHP >> 8 | MaxHP << 24;
            Username = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            MaxShield = reader.ReadUInt32();
            MaxShield = MaxShield >> 2 | MaxShield << 30;
            ClanID = reader.ReadUInt32();
            ClanID = ClanID << 2 | ClanID >> 30;
            HP = reader.ReadUInt32();
            HP = HP << 16 | HP >> 16;
            Jackpot = reader.ReadSingle();
        }
    }
}