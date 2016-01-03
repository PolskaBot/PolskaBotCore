using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class RemoteLicenseCheck : Command
    {
        public const ushort ID = 103;

        public int UserID { get; private set; }

        public RemoteLicenseCheck(int id)
        {
            UserID = id;
            Write();
        }

        public void Write()
        {
            short totalLength = 6;
            packetWriter.Write(totalLength);
            packetWriter.Write(ID);
            packetWriter.Write(UserID);
        }
    }
}
