using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    public class Move : Command
    {
        public const ushort ID = 11943;

        public uint targetX { get; private set; }
        public uint targetY { get; private set; }
        public uint currentX { get; private set; }
        public uint currentY { get; private set; }

        public Move(uint targetX, uint targetY, uint currentX, uint currentY)
        {
            this.targetX = targetX;
            this.targetY = targetY;
            this.currentX = currentX;
            this.currentY = currentY;
            Write();
        }

        public override void Write()
        {
            packetWriter.Write(18);
            packetWriter.Write(ID);
            packetWriter.Write(targetY << 6 | targetY >> 26);
            packetWriter.Write(currentY >> 9 | currentY << 23);
            packetWriter.Write(targetX >> 1 | targetX << 31);
            packetWriter.Write(currentX << 10 | currentX >> 22);
        }
    }
}
