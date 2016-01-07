using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    public class ReviveShip : Command
    {
        public const ushort ID = 23242;

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

        public void Write()
        {
            packetWriter.Write((short)(30 + Version.Length + SID.Length));
            packetWriter.Write(ID);
            packetWriter.Write((short)-22573);
            packetWriter.Write((short)2455); //class_356.ID
            packetWriter.Write(Selected);
            packetWriter.Write((short)2306);
            packetWriter.Write((short)24292);
            packetWriter.Write((short)13280);
            packetWriter.Write(Login.ID);
            packetWriter.Write((int)((uint)UserID >> 6 | (uint)UserID << 26));
            packetWriter.Write((short)Version.Length);
            packetWriter.Write(Encoding.UTF8.GetBytes(Version));
            packetWriter.Write((int)((uint)InstanceID << 7 | (uint)InstanceID >> 25));
            packetWriter.Write((short)SID.Length);
            packetWriter.Write(Encoding.UTF8.GetBytes(SID));
            packetWriter.Write(FactionID);

        }
    }
}
