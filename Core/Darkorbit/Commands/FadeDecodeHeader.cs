using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class FadeDecodeHeader : Command
    {
        public const ushort ID = 69;

        public ushort length { get; private set; }
        public ushort id { get; private set; }

        public FadeDecodeHeader(ushort length, ushort id)
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
