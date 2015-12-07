using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class Ready : Command
    {
        public const ushort ID = 9448;
        public Ready()
        {
            Write();
        }

        public override void Write()
        {
            packetWriter.Write((short)4);
            packetWriter.Write(ID);
            packetWriter.Write((short)-17906);
        }
    }
}
