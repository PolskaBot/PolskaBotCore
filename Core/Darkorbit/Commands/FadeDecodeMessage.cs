using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class FadeDecodeMessage : Command
    {
        public const short ID = 70;

        public byte[] buffer { get; private set; }

        public FadeDecodeMessage(byte[] buffer)
        {
            this.buffer = buffer;
            Write();
        }

        public override short GetID()
        {
            return ID;
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
