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
        public const ushort ID = 10586;

        public int HP { get; private set; } //name_90
        public int MaxHP { get; private set; } //name_39
        public int NanoHP { get; private set; } //var_2224
        public int MaxNanoHP { get; private set; } //var_3763

        public HitpointsUpdated(EndianBinaryReader reader)
        {
            NanoHP = reader.ReadInt32();
            NanoHP = (int)((uint)NanoHP >> 16 | (uint)NanoHP << 16);
            HP = reader.ReadInt32();
            HP = (int)((uint)HP << 6 | (uint)HP >> 26);
            MaxHP = reader.ReadInt32();
            MaxHP = (int)((uint)MaxHP << 16 | (uint)MaxHP >> 16);
            MaxNanoHP = reader.ReadInt32();
            MaxNanoHP = (int)((uint)MaxNanoHP >> 13 | (uint)MaxNanoHP << 19);
        }
    }
}
