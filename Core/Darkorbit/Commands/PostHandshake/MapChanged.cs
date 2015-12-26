using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class MapChanged : Command
    {
        public const ushort ID = 28554;

        public int MapID { get; private set; }
        public int var_294 { get; private set; }

        public MapChanged(EndianBinaryReader reader)
        {
            MapID = reader.ReadInt32();
            MapID = (int)((uint)MapID << 14 | (uint)MapID >> 18);
            reader.ReadUInt16();
            reader.ReadUInt16();
            var_294 = reader.ReadInt32();
            var_294 = (int)((uint)var_294 >> 12 | (uint)var_294 << 20);
        }
    }
}
