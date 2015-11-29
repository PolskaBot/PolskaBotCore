using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class FadeDecodeBody : Command
    {
        public const short ID = 70;

        public byte[] buffer { get; private set; }

        public FadeDecodeBody(byte[] buffer)
        {
            this.buffer = buffer;
            Write();
        }

        public override void Write()
        {
            short totalLength = (short) (buffer.Length + 2);
            packetWriter.Write(totalLength);
            packetWriter.Write(ID);
            packetWriter.Write(buffer);
        }
    }
}
