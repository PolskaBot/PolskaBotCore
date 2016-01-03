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
        public int UserID { get; set; }

        public RemoteInitStageOne(byte[] code, int userID)
        {
            this.code = code;
            UserID = userID;
            Write();
        }

        public void Write()
        {
            short totalLength = (short)(code.Length + 6);
            packetWriter.Write(totalLength);
            packetWriter.Write(ID);
            packetWriter.Write(UserID);
            packetWriter.Write(code);
        }
    }
}
