using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class ReviveShip : Command
    {
        public const ushort ID = 16044;

        public short Selected { get; private set; }
        public int UserID { get; private set; }
        public string SID { get; private set; }
        public short FactionID { get; private set; }
        public int InstanceID { get; private set; }
        public string Version { get; private set; }

        public ReviveShip(int userID, string sessionID, short factionID, int instanceID, short selected)
        {
            UserID = userID;
            SID = sessionID;
            FactionID = factionID;
            InstanceID = instanceID;
            Version = "";
            Selected = selected;
            Write();
        }

        public override void Write()
        {
            packetWriter.Write((short)60);
            packetWriter.Write(ID);
            packetWriter.Write((short)24278); //class_356.ID
            packetWriter.Write(Selected);
            packetWriter.Write((short)-25050);
            packetWriter.Write(Login.ID);
            packetWriter.Write((short)-19684);
            packetWriter.Write((int)(InstanceID << 6 | InstanceID >> 26));
            packetWriter.Write((short)FactionID);
            packetWriter.Write((int)(UserID >> 3 | UserID << 29));
            packetWriter.Write((UInt16)Version.Length);
            packetWriter.Write(Encoding.UTF8.GetBytes(Version));
            packetWriter.Write((UInt16)SID.Length);
            packetWriter.Write(Encoding.UTF8.GetBytes(SID));
            Console.WriteLine(this.ToArray().Length);
        }
    }
}
