using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class FadeEncodePacket : Command
    {
        public const ushort ID = 79;

        public byte[] buffer { get; private set; }

        public FadeEncodePacket(byte[] buffer)
        {
            this.buffer = buffer;
            Write();
        }

        public void Write()
        {
            short totalLength = (short) (buffer.Length + 2);
            packetWriter.Write(totalLength);
            packetWriter.Write(ID);
            packetWriter.Write(buffer);
        }
    }
}
