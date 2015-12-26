using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class ClientRequestCallback : Command
    {
        public const ushort ID = 12484;

        public byte[] callback { get; private set; }

        public ClientRequestCallback(byte[] callback)
        {
            this.callback = callback;
            Write();
        }

        public void Write()
        {
            short totalLength = (short)(8 + callback.Length);
            packetWriter.Write(totalLength);
            packetWriter.Write(ID);
            packetWriter.Write(callback.Length);
            packetWriter.Write(callback, 0, callback.Length);
            packetWriter.Write((Int16)27622);
        }
    }
}
