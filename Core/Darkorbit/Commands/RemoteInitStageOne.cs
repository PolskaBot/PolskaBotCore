using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class RemoteInitStageOne : Command
    {
        public const ushort ID = 101;

        private int _userID;
        private byte[] _code;

        public RemoteInitStageOne(byte[] code, int userID)
        {
            _code = code;
            _userID = userID;
            Write();
        }

        public void Write()
        {
            short totalLength = (short)(_code.Length + 6);
            packetWriter.Write(totalLength);
            packetWriter.Write(ID);
            packetWriter.Write(_userID);
            packetWriter.Write(_code);
        }
    }
}
