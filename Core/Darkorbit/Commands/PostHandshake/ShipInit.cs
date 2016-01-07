using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class ShipInit : Command
    {
        public const ushort ID = 31178;

        public bool var_4552 { get; private set; }
        public uint var_3834 { get; private set; }
        public uint var_3378 { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public uint ClanID { get; private set; } //name_46
        public bool Cloaked { get; private set; }
        //public var name_148:package_38.class_940;
        //public var var_2742:Vector.<package_38.class_326>;
        public int var_3914 { get; private set; }
        public string ClanTag { get; private set; } //name_138
        public int FactionID { get; private set; }
        public uint Rank { get; private set; } //name_134
        //public var var_4235:package_38.class_396;
        public bool var_3674 { get; private set; }
        public string Shipname { get; private set; } //name_122
        public bool NPC { get; private set; }
        public uint var_2597 { get; private set; }
        public string Username { get; private set; } //var_3497
        public uint UserID { get; private set; } //name_125

        public ShipInit(EndianBinaryReader reader)
        {
            X = reader.ReadInt32();
            X = (int)((uint)X << 14 | (uint)X >> 18);
            var_3914 = reader.ReadInt32();
            var_3914 = var_3914 >> 7 | var_3914 << 25;
            reader.ReadUInt16(); //class_940
            reader.ReadUInt16();
            reader.ReadUInt16();
            var_4552 = reader.ReadBoolean();
            NPC = reader.ReadBoolean();
            reader.ReadUInt16(); //class_396
            reader.ReadUInt16();
            Cloaked = reader.ReadBoolean();
            FactionID = reader.ReadInt32();
            FactionID = (int)((uint)FactionID << 12 | (uint)FactionID >> 20);
            Username = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
            reader.ReadUInt16();
            var_3674 = reader.ReadBoolean();
            reader.ReadUInt16();
            var_3378 = reader.ReadUInt32();
            var_3378 = var_3378 >> 13 | var_3378 << 19;
            UserID = reader.ReadUInt32();
            UserID = UserID << 4 | UserID >> 28;
            Rank = reader.ReadUInt32();
            Rank = Rank << 15 | Rank >> 17;
            var_3834 = reader.ReadUInt32();
            var_3834 = var_3834 >> 3 | var_3834 << 29;
            ClanID = reader.ReadUInt32();
            ClanID = ClanID >> 12 | ClanID << 20;
            var_2597 = reader.ReadUInt32();
            var_2597 = var_2597 >> 13 | var_2597 << 19;
            int length = reader.ReadInt32();
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    reader.ReadInt16();
                    Class326 class326 = new Class326(reader);
                }
            }
            Shipname = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
            Y = reader.ReadInt32();
            Y = (int)((uint)Y >> 10 | (uint)Y << 22);
            ClanTag = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
        }
    }
}