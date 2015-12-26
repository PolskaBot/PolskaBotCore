using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class ClientRequestCode : Command
    {
        public const ushort ID = 25327;
        public short length { get; private set; } = 4;

        public Int16 reserved = 21476;

        public ClientRequestCode()
        {
            Write();
        }

        public void Write()
        {
            packetWriter.Write(length);
            packetWriter.Write(ID);
            packetWriter.Write(reserved);
        }
    }
}
