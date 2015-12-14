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
        public uint x { get; private set; }
        public uint y { get; private set; }
        public uint clanID { get; private set; } //name_46
        public bool cloaked { get; private set; }
        //public var name_148:package_38.class_940;
        //public var var_2742:Vector.<package_38.class_326>;
        public uint var_3911 { get; private set; }
        public string clanTag { get; private set; } //name_138
        public uint factionID { get; private set; }
        public uint rank { get; private set; } //name_134
        //public var var_4235:package_38.class_396;
        public bool var_3674 { get; private set; }
        public string shipName { get; private set; } //name_122
        public bool npc { get; private set; }
        public uint var_2597 { get; private set; }
        public string userName { get; private set; } //var_3497
        public uint userID { get; private set; } //name_125

        public ShipInit(EndianBinaryReader reader)
        {
            x = reader.ReadUInt32();
            x = x << 2 | x >> 30;
            var_2597 = reader.ReadUInt32();
            var_2597 = var_2597 << 9 | var_2597 >> 23;
            userName = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
            userID = reader.ReadUInt32();
            userID = userID >> 6 | userID << 26;
            reader.ReadBytes(4);
            clanTag = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
            clanID = reader.ReadUInt32();
            clanID = clanID >> 11 | clanID << 21;
            var_4559 = reader.ReadBoolean();
            var_3378 = reader.ReadUInt32();
            var_3378 = var_3378 << 12 | var_3378 >> 20;
            npc = reader.ReadBoolean();
            var_3674 = reader.ReadBoolean();
            var_3911 = reader.ReadUInt32();
            var_3911 = var_3911 >> 7 | var_3911 << 25;
            var_3834 = reader.ReadUInt32();
            var_3834 = var_3834 << 2 | var_3834 >> 30;
            cloaked = reader.ReadBoolean();
            reader.ReadBytes(4);
            factionID = reader.ReadUInt32();
            factionID = factionID >> 13 | factionID << 19;
            int length = reader.ReadInt32();
            if(length > 0)
            {
                reader.ReadInt16();
                for (int i = 0; i < length; i++)
                {
                    class_326 class326 = new class_326(reader);
                }
            }
            rank = reader.ReadUInt32();
            rank = rank << 3 | rank >> 29;
            y = reader.ReadUInt32();
            y = y <<  15 | y >> 17;
            shipName = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
        }

        public override void Write()
        {
            return;
        }

    }
}