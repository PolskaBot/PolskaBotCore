using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class Ping : Command
    {
        public const ushort ID = 9448;

        public short tag { get; private set; } = -17906;

        public Ping()
        {
            Write();
        }

        public override void Write()
        {
            packetWriter.Write((short)4);
            packetWriter.Write(ID);
            packetWriter.Write(tag);
        }
    }
}
