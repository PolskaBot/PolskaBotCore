using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class ClientProxy : Command
    {
        public short ID = 69;

        public short originalLength { get; private set; }
        public short originalID { get; private set; }
        public byte[] buffer { get; private set; }

        public ClientProxy(short originalLength, short originalID, byte[] buffer)
        {
            this.originalLength = originalLength;
            this.originalLength = originalLength;
            this.buffer = buffer;
            Write();
        }

        public override short GetID()
        {
            return ID;
        }

        public override void Write()
        {
            short totalLength = (short) (buffer.Length  + 6);
            packetWriter.Write(totalLength);
            packetWriter.Write(ID);
            packetWriter.Write(originalLength);
            packetWriter.Write(originalID);
            packetWriter.Write(buffer);
        }
    }
}
