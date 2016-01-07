using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class Ready : Command
    {
        public const ushort ID = 25614;
        public Ready()
        {
            Write();
        }

        public void Write()
        {
            packetWriter.Write((short)6);
            packetWriter.Write(ID);
            packetWriter.Write((short)17356);
            packetWriter.Write((short)-12161);
        }
    }
}
