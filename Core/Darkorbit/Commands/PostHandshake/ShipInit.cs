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
        public const ushort ID = 25118;

        public bool var_4559 { get; private set; }
        public uint var_3834 { get; private set; }
        public uint var_3378 { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public uint ClanID { get; private set; } //name_46
        public bool Cloaked { get; private set; }
        //public var name_148:package_38.class_940;
        //public var var_2742:Vector.<package_38.class_326>;
        public uint var_3911 { get; private set; }
        public string ClanTag { get; private set; } //name_138
        public uint FactionID { get; private set; }
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
            X = (int)((uint)X << 2 | (uint)X >> 30);
            var_2597 = reader.ReadUInt32();
            var_2597 = var_2597 << 9 | var_2597 >> 23;
            Username = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
            UserID = reader.ReadUInt32();
            UserID = UserID >> 6 | UserID << 26;
            reader.ReadBytes(4);
            ClanTag = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
            ClanID = reader.ReadUInt32();
            ClanID = ClanID >> 11 | ClanID << 21;
            var_4559 = reader.ReadBoolean();
            var_3378 = reader.ReadUInt32();
            var_3378 = var_3378 << 12 | var_3378 >> 20;
            NPC = reader.ReadBoolean();
            var_3674 = reader.ReadBoolean();
            var_3911 = reader.ReadUInt32();
            var_3911 = var_3911 >> 7 | var_3911 << 25;
            var_3834 = reader.ReadUInt32();
            var_3834 = var_3834 << 2 | var_3834 >> 30;
            Cloaked = reader.ReadBoolean();
            reader.ReadBytes(4);
            FactionID = reader.ReadUInt32();
            FactionID = FactionID >> 13 | FactionID << 19;
            int length = reader.ReadInt32();
            if(length > 0)
            {
                reader.ReadInt16();
                for (int i = 0; i < length; i++)
                {
                    Class326 class326 = new Class326(reader);
                }
            }
            Rank = reader.ReadUInt32();
            Rank = Rank << 3 | Rank >> 29;
            Y = reader.ReadInt32();
            Y = (int)((uint)Y <<  15 | (uint)Y >> 17);
            Shipname = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
        }
    }
}