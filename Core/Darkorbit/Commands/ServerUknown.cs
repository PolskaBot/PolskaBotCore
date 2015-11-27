using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Darkorbit.Commands
{
    class ServerUknown : Command
    {
        public short ID { get; private set; }

        public ServerUknown(short ID)
        {
            this.ID = ID;
        }

        public override short GetID()
        {
            return ID;
        }

        public override void Write()
        {
            return;
        }
    }
}
