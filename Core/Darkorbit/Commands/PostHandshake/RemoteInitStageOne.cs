using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class RemoteInitStageOne : Command
    {
        public const ushort ID = 101;

        public byte[] code { get; private set; }

        public RemoteInitStageOne(byte[] code)
        {
            this.code = code;
            Write();
        }

        public void Write()
        {
            short totalLength = (short)(code.Length + 2);
            packetWriter.Write(totalLength);
            packetWriter.Write(ID);
            packetWriter.Write(code);
        }
    }
}
