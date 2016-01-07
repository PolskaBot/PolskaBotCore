using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class GateInit : Command
    {
        public const ushort ID = 8519;

        public int FactionID { get; private set; }
        public List<int> var_2358 { get; private set; } = new List<int>();
        public int X { get; private set; }
        public int GateType { get; private set; } //name_158
        public bool var_139 { get; private set; }
        public bool var_4990 { get; private set; }
        public int Y { get; private set; }
        public int AssetID { get; private set; } //var_5014 (?)

        public GateInit(EndianBinaryReader reader)
        {
            var_4990 = reader.ReadBoolean();
            reader.ReadUInt16();
            int length = reader.ReadInt32();
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    int value = reader.ReadInt32();
                    value = (int)((uint)value >> 15 | (uint)value << 17);
                    var_2358.Add(value);
                }
            }
            var_139 = reader.ReadBoolean();
            Y = reader.ReadInt32();
            Y = (int)((uint)Y << 8 | (uint)Y >> 24);
            X = reader.ReadInt32();
            X = (int)((uint)X << 10 | (uint)X >> 22);
            AssetID = reader.ReadInt32();
            AssetID = (int)((uint)AssetID << 5 | (uint)AssetID >> 27);
            FactionID = reader.ReadInt32();
            FactionID = (int)((uint)FactionID >> 15 | (uint)FactionID << 17);
            reader.ReadUInt16();
            GateType = reader.ReadInt32();
            GateType = (int)((uint)GateType << 10 | (uint)GateType >> 22);
        }
    }
}
