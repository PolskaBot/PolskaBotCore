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
        public const ushort ID = 21535;

        public int MaxShield { get; private set; }  //name_101
        public int Shield { get; private set; }     //var_752

        public ShieldUpdated(EndianBinaryReader reader)
        {
            Shield = reader.ReadInt32();
            Shield = (int)((uint)Shield >> 14 | (uint)Shield << 18);
            MaxShield = reader.ReadInt32();
            MaxShield = (int)((uint)MaxShield >> 2 | (uint)MaxShield << 30);
            reader.ReadInt16();
        }
    }
}
