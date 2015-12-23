using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class ActionRequest : Command
    {
        public const ushort ID = 2287;

        public int BarID { get; private set; } //var_2191
        public int Type { get; private set; } //var_2208
        public string Action { get; private set; } //var_2111

        public ActionRequest(string action, int type, int barID)
        {
            Action = action;
            Type = type;
            BarID = barID;
            Write();
        }

        public override void Write()
        {
            packetWriter.Write((short)(10 + Action.Length));
            packetWriter.Write(ID);
            packetWriter.Write((short)-3091);
            packetWriter.Write((short)Type);
            packetWriter.Write((short)BarID);
            packetWriter.Write((short)Action.Length);
            packetWriter.Write(Encoding.UTF8.GetBytes(Action));
        }
    }
}
