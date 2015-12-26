using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class Ping : Command
    {
        public const ushort ID = 20638;

        public Ping()
        {
            Write();
        }

        public void Write()
        {
            packetWriter.Write((short)2);
            packetWriter.Write(ID);
        }
    }
}
