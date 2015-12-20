using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    public class Jump : Command
    {
        public const short ID = 571;

        public Jump()
        {
            Write();
        }

        public override void Write()
        {
            packetWriter.Write(6);
            packetWriter.Write(ID);
            packetWriter.Write((short)17370);
            packetWriter.Write((short)1032);
        }
    }
}
