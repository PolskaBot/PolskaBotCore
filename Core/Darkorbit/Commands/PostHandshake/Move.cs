using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class Move : Command
    {
        public const ushort ID = 25229;

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
            packetWriter.Write(20);
            packetWriter.Write(ID);
            packetWriter.Write(targetY >> 6 | targetY << 26);
            packetWriter.Write((short)1668);
            packetWriter.Write(currentY << 15 | currentY >> 17);
            packetWriter.Write(currentX >> 7 | this.currentX << 25);
            packetWriter.Write(targetX >> 9 | targetX << 23);
        }
    }
}
