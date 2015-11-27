using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Darkorbit.Commands
{
    class ClientRequestCode : Command
    {
        public const short ID = 25221;
        public short length { get; private set; } = 4;

        public Int16 reserved = 31;

        public ClientRequestCode()
        {
            Write();
        }

        public override short GetID()
        {
            return ID;
        }

        public override void Write()
        {
            packetWriter.Write(length);
            packetWriter.Write(ID);
            packetWriter.Write(reserved);
        }
    }
}
