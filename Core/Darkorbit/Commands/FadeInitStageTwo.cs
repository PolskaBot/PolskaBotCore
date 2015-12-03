using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class FadeInitStageTwo : Command
    {
        public const ushort ID = 102;

        public byte[] secret { get; private set; }

        public FadeInitStageTwo(byte[] secret)
        {
            this.secret = secret;
            Write();
        }

        public override void Write()
        {
            short totalLength = (short)(secret.Length + 2);
            packetWriter.Write(totalLength);
            packetWriter.Write(ID);
            packetWriter.Write(secret);
        }
    }
}
