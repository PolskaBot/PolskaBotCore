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
        public const ushort ID = 7761;

        public string name_138 { get; private set; }        // name_138
        public int var_3377 { get; private set; }           // var_3377
        public int BuildingID { get; private set; }         // assetId
        public int AssetType { get; private set; }
        //public var name_96:package_38.class_940;
        public bool var_3562 { get; private set; }          // var_3562
        //public var type:package_38.class_455;
        public int X { get; private set; }                  // var_4802
        public int name_158 { get; private set; }           // name_158
        public int name_48 { get; private set; }            // name_48
        public int Y { get; private set; }                  // var_2324
        public bool var_4991 { get; private set; }          // var_4991
        public bool var_984 { get; private set; }           // var_984
        //public var var_2742:Vector.<package_38.class_326>;
        public int FactionID { get; private set; }
        public bool var_1531 { get; private set; }          // var_1531
        public string Name { get; private set; }            //var_3497

        public BuildingInit(EndianBinaryReader reader)
        {
            BuildingID = reader.ReadInt32();
            BuildingID = (int)((uint)BuildingID >> 2 | (uint)BuildingID << 30);
            X = reader.ReadInt32();
            X = (int)((uint)X << 8 | (uint)X >> 24);
            name_138 = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
            reader.ReadUInt16();
            var_1531 = reader.ReadBoolean();
            var_984 = reader.ReadBoolean();
            name_48 = reader.ReadInt32();
            name_48 = (int)((uint)name_48 << 2 | (uint)name_48 >> 30);
            FactionID = reader.ReadInt32();
            FactionID = (int)((uint)FactionID >> 13 | (uint)FactionID << 19);
            int length = reader.ReadInt32();
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    reader.ReadInt16();
                    Class326 class326 = new Class326(reader);
                }
            }
            name_158 = reader.ReadInt32();
            name_158 = (int)((uint)name_158 >> 11 | (uint)name_158 << 21);
            var_3377 = reader.ReadInt32();
            var_3377 = (int)((uint)var_3377 >> 6 | (uint)var_3377 << 26);
            //class_455 begin read
            reader.ReadUInt16();
            AssetType = reader.ReadUInt16();
            //class_455 end read
            Y = reader.ReadInt32();
            Y = (int)((uint)Y >> 10 | (uint)Y << 22);
            //class_940 begin read
            reader.ReadUInt16();
            reader.ReadUInt16();
            int type = reader.ReadUInt16();
            //class_940 end read
            Name = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
            var_4991 = reader.ReadBoolean();
            var_3562 = reader.ReadBoolean();
        }
    }
}
