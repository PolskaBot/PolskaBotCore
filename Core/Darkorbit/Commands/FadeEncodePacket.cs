using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class FadeEncodePacket : Command
    {
        public const short ID = 79;

        public short length { get; private set; }
        public short id { get; private set; }

        public FadeEncodePacket(short length, short id)
        {
            this.length = length;
            this.id = id;
            Write();
        }

        public override void Write()
        {
            packetWriter.Write((short) 6);
            packetWriter.Write(ID);
            packetWriter.Write(length);
            packetWriter.Write(id);
        }
    }
}
