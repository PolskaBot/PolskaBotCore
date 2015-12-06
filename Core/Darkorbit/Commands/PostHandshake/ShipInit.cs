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
        public const ushort ID = 17123;

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
        public uint var_2596 { get; private set; }
        public string userName { get; private set; } //var_3497
        public uint userID { get; private set; } //name_125

        public ShipInit(EndianBinaryReader reader)
        {
            Console.WriteLine("Reading ShipInit");
            var_4559 = reader.ReadBoolean();
            var_3834 = reader.ReadUInt32();
            var_3834 = var_3834 >> 15 | var_3834 << 17;
            var_3378 = reader.ReadUInt32();
            x = reader.ReadUInt32();
            x = x >> 1 | x << 31;
            reader.ReadUInt16();
            clanID = reader.ReadUInt32();
            clanID = clanID << 16 | clanID >> 16;
            cloaked = reader.ReadBoolean();
            reader.ReadBytes(6);
            for (int i = 0; i < reader.ReadUInt32(); i++)
            {
                class_326 class326 = new class_326(reader);
            }
            var_3911 = reader.ReadUInt32();
            var_3911 = var_3911 << 10 | var_3911 >> 22;
            clanTag = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
            factionID = reader.ReadUInt32();
            factionID = factionID << 5 | factionID >> 27;
            rank = reader.ReadUInt32();
            rank = rank << 8 | rank >> 24;
            reader.ReadBytes(6);
            var_3674 = reader.ReadBoolean();
            shipName = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
            npc = reader.ReadBoolean();
            var_2596 = reader.ReadUInt32();
            var_2596 = var_2596 << 12 | var_2596 >> 20;
            userName = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
            userID = reader.ReadUInt32();
            userID = userID >> 11 | userID << 21;
            y = reader.ReadUInt32();
            y = y << 4 | y >> 28;
        }

        public override void Write()
        {
            return;
        }

    }
}