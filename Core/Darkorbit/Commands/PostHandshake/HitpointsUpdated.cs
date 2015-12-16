using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class HitpointsUpdated : Command
    {
        public const ushort ID = 19181;

        public int HP { get; private set; }
        public int MaxHP { get; private set; }
        public int NanoHP { get; private set; }
        public int MaxNanoHP { get; private set; }

        public HitpointsUpdated(EndianBinaryReader reader)
        {
            HP = reader.ReadInt32();
            HP = (int)((uint)HP << 5 | (uint)HP >> 27);
            MaxHP = reader.ReadInt32();
            MaxHP = (int)((uint)MaxHP << 8 | (uint)MaxHP >> 24);
            NanoHP = reader.ReadInt32();
            NanoHP = (int)((uint)NanoHP << 10 | (uint)NanoHP >> 22);
            MaxNanoHP = reader.ReadInt32();
            MaxNanoHP = (int)((uint)MaxNanoHP >> 8 | (uint)MaxNanoHP << 24);
        }

        public override void Write()
        {
            return;
        }
    }
}
