using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class FadeInitStageOne : Command
    {
        public const short ID = 101;

        public byte[] code { get; private set; }

        public FadeInitStageOne(byte[] code)
        {
            this.code = code;
            Write();
        }

        public override short GetID()
        {
            return ID;
        }

        public override void Write()
        {
            short totalLength = (short) (code.Length + 2);
            packetWriter.Write(totalLength);
            packetWriter.Write(ID);
            packetWriter.Write(code);
        }
    }
}
