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

        public uint TargetX { get; private set; }
        public uint TargetY { get; private set; }
        public uint CurrentX { get; private set; }
        public uint CurrentY { get; private set; }

        public Move(uint targetX, uint targetY, uint currentX, uint currentY)
        {
            TargetX = targetX;
            TargetY = targetY;
            CurrentX = currentX;
            CurrentY = currentY;
            Write();
        }

        public override void Write()
        {
            packetWriter.Write(18);
            packetWriter.Write(ID);
            packetWriter.Write(TargetY << 6 | TargetY >> 26);
            packetWriter.Write(CurrentY >> 9 | CurrentY << 23);
            packetWriter.Write(TargetX >> 1 | TargetX << 31);
            packetWriter.Write(CurrentX << 10 | CurrentX >> 22);
        }
    }
}
