using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Darkorbit.Commands
{
    class ClientProxy : Command
    {
        public short ID = 69;

        public byte[] buffer { get; private set; }

        public ClientProxy(byte[] buffer)
        {
            this.buffer = buffer;
            Write();
        }

        public override short GetID()
        {
            return ID;
        }

        public override void Write()
        {
            packetWriter.Write((short) buffer.Length);
            packetWriter.Write(ID);
            packetWriter.Write(buffer);
        }
    }
}
