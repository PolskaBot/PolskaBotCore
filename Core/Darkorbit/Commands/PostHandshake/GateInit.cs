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

        public int factionID { get; private set; }
        public List<int> var_2358 { get; private set; }
        public int X { get; private set; }
        public int gateType { get; private set; } //name_158
        public bool var_138 { get; private set; }
        public bool var_4991 { get; private set; }
        public int Y { get; private set; }
        public int assetID { get; private set; } //var_5014 (?)

        public GateInit(EndianBinaryReader reader)
        {
            factionID = reader.ReadInt32();
            factionID = (int)((uint)factionID >> 1 | factionID << 31);
            for (int i = 0; i < reader.ReadInt32(); i++)
            {
                int value = reader.ReadInt32();
                value = (int)(value << 8 | (uint)value >> 24);
                var_2358.Add(value);
            }
            X = reader.ReadInt32();
            X = (int)(X << 1 | (uint)X >> 31);
            gateType = reader.ReadInt32();
            gateType = (int)((uint)gateType >> 1 | gateType << 31);
            var_138 = reader.ReadBoolean();
            var_4991 = reader.ReadBoolean();
            Y = reader.ReadInt32();
            Y = (int)(Y << 3 | (uint)Y >> 29);
            assetID = reader.ReadInt32();
            assetID = (int)((uint)assetID >> 3 | assetID << 29);
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
