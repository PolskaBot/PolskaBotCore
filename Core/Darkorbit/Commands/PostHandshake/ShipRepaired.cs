using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class ShipRepaired : Command
    {
        public const ushort ID = 19181;

        public int HP { get; private set; }
        public int MaxHP { get; private set; }
        public int NanoHP { get; private set; }
        public int MaxNanoHP { get; private set; }

        public ShipRepaired(EndianBinaryReader reader)
        {
            HP = reader.ReadInt32();
            HP = (int)(HP << 5 | (uint)HP >> 27);
            MaxHP = reader.ReadInt32();
            MaxHP = (int)(MaxHP << 8 | (uint)MaxHP >> 24);
            NanoHP = reader.ReadInt32();
            NanoHP = (int)(NanoHP << 10 | (uint)NanoHP >> 22);
            MaxNanoHP = reader.ReadInt32();
            MaxNanoHP = (int)((uint)MaxNanoHP >> 8 | MaxNanoHP << 24);
        }

        public override void Write()
        {
            return;
        }
    }
}
