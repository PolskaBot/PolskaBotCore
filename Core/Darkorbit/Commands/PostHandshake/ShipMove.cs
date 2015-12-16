using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    public class ShipMove : Command
    {
        public const ushort ID = 1771;

        public uint player { get; private set; } //name_125
        public int x { get; private set; }
        public int y { get; private set; }
        public uint duration { get; private set; } //var_3506

        public ShipMove(EndianBinaryReader reader)
        {
            reader.ReadInt16();
            reader.ReadInt16();
            y = reader.ReadInt32();
            y = (int)((uint)y << 1 | (uint)y >> 31);
            player = reader.ReadUInt32();
            player = player << 10 | player >> 22;
            x = reader.ReadInt32();
            x = (int)((uint)x >> 6 | (uint)x << 26);
            duration = reader.ReadUInt32();
            duration = duration >> 11 | duration << 21;
            Console.WriteLine("Moving to {0}, {1}", x, y);
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
