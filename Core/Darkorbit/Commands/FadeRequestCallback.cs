using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class FadeRequestCallback : Command
    {
        public const ushort ID = 201;

        public FadeRequestCallback()
        {
            Write();
        }

        public override void Write()
        {
            short totalLength = (short)2;
            packetWriter.Write(totalLength);
            packetWriter.Write(ID);
        }
    }
}
