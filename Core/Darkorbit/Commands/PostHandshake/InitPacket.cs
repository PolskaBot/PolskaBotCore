using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class InitPacket : Command
    {
        public const ushort ID = 13686;

        public short num { get; private set; }

        public string message { get; private set; } = "3D 1122x667 .root1.instance190.MainClientApplication0.ApplicationSkin2.Group3.Group4._-O2B5.instance24616 root1";

        public InitPacket(short num)
        {
            this.num = num;
            Write();
        }

        public override void Write()
        {
            short totalLength = (short)(message.Length + 6);
            packetWriter.Write(totalLength);
            packetWriter.Write(ID);
            packetWriter.Write((byte)0);
            packetWriter.Write(message);
            packetWriter.Write(num);
        }
    }
}
