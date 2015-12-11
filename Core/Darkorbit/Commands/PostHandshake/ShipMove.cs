using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class ShipMove : Command
    {
        public const ushort ID = 1771;

        public uint player { get; private set; } //name_125
        public uint x { get; private set; }
        public uint y { get; private set; }
        public uint duration { get; private set; } //var_3506

        public ShipMove(EndianBinaryReader reader)
        {
            reader.ReadInt16();
            reader.ReadInt16();
            y = reader.ReadUInt32();
            y = y << 1 | y >> 31;
            player = reader.ReadUInt32();
            player = player << 10 | player >> 22;
            x = reader.ReadUInt32();
            x = x >> 6 | x << 26;
            duration = reader.ReadUInt32();
            duration = duration >> 11 | duration << 21;
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
