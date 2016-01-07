using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class Ping : Command
    {
        public const ushort ID = 15776;

        public Ping()
        {
            Write();
        }

        public void Write()
        {
            packetWriter.Write((short)4);
            packetWriter.Write(ID);
            packetWriter.Write((short)-8609);
        }
    }
}
