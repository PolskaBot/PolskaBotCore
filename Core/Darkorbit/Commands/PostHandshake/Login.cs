using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class Login : Command
    {
        public const ushort ID = 21821;

        public int userID { get; private set; }
        public string sessionID { get; private set; }
        public short factionID { get; private set; }
        public int instanceID { get; private set; }
        public string version { get; private set; }

        public Login(int userID, string sessionID, short factionID, int instanceID, string version = Config.VERSION)
        {
            this.userID = userID;
            this.sessionID = sessionID;
            this.factionID = factionID;
            this.instanceID = instanceID;
            this.version = version;
            Write();
        }

        public override void Write()
        {
            short totalLength = (short)(sessionID.Length + version.Length + 18);
            packetWriter.Write(totalLength);
            packetWriter.Write(ID);
            packetWriter.Write((short)-19684);
            packetWriter.Write((int)(instanceID << 6 | instanceID >> 26));
            packetWriter.Write((short)factionID);
            packetWriter.Write((int)(userID >> 3 | userID << 29));
            packetWriter.Write((byte)0);
            packetWriter.Write(version);
            packetWriter.Write((byte)0);
            packetWriter.Write(sessionID);
        }
    }
}
