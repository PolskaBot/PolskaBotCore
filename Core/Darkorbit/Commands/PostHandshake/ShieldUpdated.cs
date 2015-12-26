using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class ShieldUpdated : Command
    {
        public const ushort ID = 5107;

        public int MaxShield { get; private set; } //name_101
        public int Shield { get; private set; } //var_752

        public ShieldUpdated(EndianBinaryReader reader)
        {
            MaxShield = reader.ReadInt32();
            MaxShield = (int)((uint)MaxShield >> 5 | (uint)MaxShield << 27);
            Shield = reader.ReadInt32();
            Shield = (int)((uint)Shield << 11 | (uint)Shield >> 21);
        }
    }
}
