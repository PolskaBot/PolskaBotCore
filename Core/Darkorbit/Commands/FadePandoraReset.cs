using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class FadePandoraReset : Command
    {
        public const ushort ID = 103;

        public FadePandoraReset()
        {
            Write();
        }

        public void Write()
        {
            short totalLength = (short)2;
            packetWriter.Write(totalLength);
            packetWriter.Write(ID);
        }
    }
}
