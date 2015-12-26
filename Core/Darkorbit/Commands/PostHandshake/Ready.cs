using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class Ready : Command
    {
        public const ushort ID = 26385;
        public Ready()
        {
            Write();
        }

        public void Write()
        {
            packetWriter.Write((short)4);
            packetWriter.Write(ID);
            packetWriter.Write((short)-30552);
        }
    }
}
