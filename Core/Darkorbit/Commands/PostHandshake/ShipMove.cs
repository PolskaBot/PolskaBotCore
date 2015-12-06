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
        public const ushort ID = 4985;

        public uint player { get; private set; }
        public uint x { get; private set; }
        public uint y { get; private set; }
        public uint duration { get; private set; }

        public ShipMove(EndianBinaryReader reader)
        {
            reader.ReadInt16();
            y = reader.ReadUInt32();
            y = y << 4 | y >> 28;
            reader.ReadInt16();
            duration = reader.ReadUInt32();
            duration = duration >> 12 | duration << 20;
            player = reader.ReadUInt32();
            player = player >> 7 | player << 25;
            x = reader.ReadUInt32();
            x = x >> 2 | x << 30;
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
