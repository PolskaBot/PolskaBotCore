using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class MapChangeConfirmation : Command
    {
        public const ushort ID = 20064;

        public bool Close { get; private set; }

        public MapChangeConfirmation(bool close = false)
        {
            Close = close;
            Write();
        }

        public override void Write()
        {
            packetWriter.Write((short)3);
            packetWriter.Write(ID);
            packetWriter.Write(Close);
        }
    }
}
