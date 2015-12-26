using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class FadeDecodePacket : Command
    {
        public const ushort ID = 69;

        public byte[] buffer { get; private set; }

        public FadeDecodePacket(byte[] buffer)
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
