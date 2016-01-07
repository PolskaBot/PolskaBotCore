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

        public uint TargetX { get; private set; }   // name_135
        public uint TargetY { get; private set; }   // name_20
        public uint CurrentX { get; private set; }  // var_3310
        public uint CurrentY { get; private set; }  // var_14

        public Move(uint targetX, uint targetY, uint currentX, uint currentY)
        {
            TargetX = targetX;
            TargetY = targetY;
            CurrentX = currentX;
            CurrentY = currentY;
            Write();
        }

        public void Write()
        {
            packetWriter.Write((short)24);
            packetWriter.Write(ID);
            packetWriter.Write((short)-13024);
            packetWriter.Write((short)21768);
            packetWriter.Write(TargetX >> 12 | TargetX << 20);
            packetWriter.Write(CurrentX << 6 | CurrentX >> 26);
            packetWriter.Write(CurrentY >> 12 | CurrentY << 20);
            packetWriter.Write(TargetY >> 6 | TargetY << 26);
        }
    }
}
