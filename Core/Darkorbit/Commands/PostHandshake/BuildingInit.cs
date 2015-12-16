using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class BuildingInit : Command
    {
        public const ushort ID = 12699;

        public string name_138 { get; private set; }
        public int var_3378 { get; private set; }
        public int assetID { get; private set; }
        //public var name_95:package_38.class_940;
        public bool var_3562 { get; private set; }
        //public var type:package_38.class_455;
        public int X { get; private set; } //var_4812
        public int name_158 { get; private set; }
        public int name_46 { get; private set; }
        public int Y { get; private set; } //var_2324
        public bool var_4991 { get; private set; }
        public bool var_984 { get; private set; }
        //public var var_2742:Vector.<package_38.class_326>;
        public int factionID { get; private set; }
        public bool var_1531 { get; private set; }
        public string name { get; private set; } //var_3497

        public BuildingInit(EndianBinaryReader reader)
        {
            name_138 = Encoding.UTF8.GetString(reader.ReadBytes((int)reader.ReadUInt16()));
            var_3378 = reader.ReadInt32();
            var_3378 = (int)((uint)var_3378 >> 1 | (uint)var_3378 << 31);
            assetID = reader.ReadInt32();
            assetID = (int)((int)(uint)assetID >> 10 | assetID << 22);
            //class_940 begin read
            reader.ReadUInt16();
            int type = reader.ReadUInt16();
            //class_940 end read
            var_3562 = reader.ReadBoolean();
            //class_455 begin read
            reader.ReadUInt16();
            int secondType = reader.ReadUInt16();
            reader.ReadUInt16();
            //class_455 end read
            X = reader.ReadInt32();
            X = (int)((uint)X >> 2 | (uint)X << 30);
            name_158 = reader.ReadInt32();
            name_158 = (int)((uint)name_158 >> 11 | (uint)name_158 << 21);
            name_46 = reader.ReadInt32();
            name_46 = (int)((uint)name_46 << 9 | (uint)name_46 >> 23);
            Y = reader.ReadInt32();
            Y = (int)((uint)Y >> 10 | (uint)Y << 22);
            var_4991 = reader.ReadBoolean();
            var_984 = reader.ReadBoolean();
            for (int i = 0; i < reader.ReadInt32(); i++)
            {
                class_326 class326 = new class_326(reader);
            }
            factionID = reader.ReadInt32();
            factionID = (int)((uint)factionID >> 3 | (uint)factionID << 29);
            var_1531 = reader.ReadBoolean();
            name = Encoding.UTF8.GetString(reader.ReadBytes((int)reader.ReadUInt16()));
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
