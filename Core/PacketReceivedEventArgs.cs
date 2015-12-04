using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolskaBot.Core.Darkorbit.Commands;

namespace PolskaBot.Core
{
    class PacketReceivedEventArgs : EventArgs
    {
        public Command command { get; private set; }

        public PacketReceivedEventArgs(Command command)
        {
            this.command = command;
        }
    }
}
