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
        public const ushort ID = 30093;

        public int FactionID { get; private set; }
        public List<int> var_2358 { get; private set; } = new List<int>();
        public int X { get; private set; }
        public int GateType { get; private set; } //name_158
        public bool var_138 { get; private set; }
        public bool var_4991 { get; private set; }
        public int Y { get; private set; }
        public int AssetID { get; private set; } //var_5014 (?)

        public GateInit(EndianBinaryReader reader)
        {
            FactionID = reader.ReadInt32();
            FactionID = (int)((uint)FactionID >> 1 | (uint)FactionID << 31);
            int length = reader.ReadInt32();
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    int value = reader.ReadInt32();
                    value = (int)((uint)value << 8 | (uint)value >> 24);
                    var_2358.Add(value);
                }
            }
            X = reader.ReadInt32();
            X = (int)((uint)X << 1 | (uint)X >> 31);
            GateType = reader.ReadInt32();
            GateType = (int)((uint)GateType >> 1 | (uint)GateType << 31);
            var_138 = reader.ReadBoolean();
            var_4991 = reader.ReadBoolean();
            Y = reader.ReadInt32();
            Y = (int)((uint)Y << 3 | (uint)Y >> 29);
            AssetID = reader.ReadInt32();
            AssetID = (int)((uint)AssetID >> 3 | (uint)AssetID << 29);
        }
    }
}
