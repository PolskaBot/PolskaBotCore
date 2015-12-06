using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class Move : Command
    {
        public const ushort ID = 20808;

        public uint x { get; private set; }
        public uint y { get; private set; }

        public Move(uint x, uint y)
        {
            this.x = x;
            this.y = y;
            Write();
        }

        public override void Write()
        {
            packetWriter.Write((short)10);
            packetWriter.Write(ID);
            uint tempX = x >> 2 | x << 30;
            uint tempY = y << 12 | y >> 20;
            Console.WriteLine($"Calculated X {tempX} Y {tempY}");
            packetWriter.Write(tempX);
            packetWriter.Write(tempY);
            packetWriter.Write((short)-1675);
        }
    }
}
