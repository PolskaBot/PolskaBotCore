using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class ShipDestroyed : Command
    {
        public const ushort ID = 28834;

        public int UserID { get; private set; } //var_2750
        public int var_1092 { get; private set; } //var_1092

        public ShipDestroyed(EndianBinaryReader reader)
        {
            UserID = reader.ReadInt32();
            UserID = (int)((uint)UserID << 8 | (uint)UserID >> 24);
            var_1092 = reader.ReadInt32();
            var_1092 = (int)((uint)var_1092 << 9 | (uint)var_1092 >> 23);
        }
    }
}
