using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class ServerVersionCheck : Command
    {
        public const ushort ID = 667;

        public int major { get; set; }
        public int minor { get; set; }
        public int build { get; set; }
        public bool compatible { get; set; }

        public ServerVersionCheck(EndianBinaryReader reader)
        {
            major = reader.ReadInt32();
            minor = reader.ReadInt32();
            build = reader.ReadInt32();
            compatible = reader.ReadBoolean();
        }

        public override void Write()
        {
            return;
        }

    }
}
