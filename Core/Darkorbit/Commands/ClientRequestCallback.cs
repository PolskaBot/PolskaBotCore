using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class ClientRequestCallback : Command
    {
        public const ushort ID = 31232;

        public byte[] callback { get; private set; }

        public ClientRequestCallback(byte[] callback)
        {
            this.callback = callback;
            Write();
        }

        public override void Write()
        {
            short totalLength = (short)(6 + callback.Length);
            packetWriter.Write(totalLength);
            packetWriter.Write(ID);
            packetWriter.Write(callback.Length);
            packetWriter.Write(callback, 0, callback.Length);
        }
    }
}
