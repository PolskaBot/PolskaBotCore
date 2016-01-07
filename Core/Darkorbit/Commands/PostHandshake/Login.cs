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

        public int UserID { get; private set; }
        public string SID { get; private set; }
        public short FactionID { get; private set; }
        public int InstanceID { get; private set; }
        public string Version { get; private set; }

        public Login(int userID, string sessionID, short factionID, int instanceID, string version = Config.VERSION)
        {
            UserID = userID;
            SID = sessionID;
            FactionID = factionID;
            InstanceID = instanceID;
            Version = version;
            Write();
        }

        public void Write()
        {
            short totalLength = (short)(SID.Length + Version.Length + 18);
            packetWriter.Write(totalLength);
            packetWriter.Write(ID);
            packetWriter.Write(UserID >> 6 | UserID << 26);
            packetWriter.Write((byte)0);
            packetWriter.Write(Version);
            packetWriter.Write(InstanceID << 7 | InstanceID >> 25);
            packetWriter.Write((byte)0);
            packetWriter.Write(SID);
            packetWriter.Write(FactionID);
        }
    }
}
