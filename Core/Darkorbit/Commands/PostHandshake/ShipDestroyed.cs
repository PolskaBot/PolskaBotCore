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
        public const ushort ID = 649;

        public int UserID { get; private set; } //var_2748
        public int var_1091 { get; private set; } //var_1091

        public ShipDestroyed(EndianBinaryReader reader)
        {
            UserID = reader.ReadInt32();
            UserID = (int)((uint)UserID << 5 | (uint)UserID >> 27);
            int test = reader.ReadInt16();
            var_1091 = reader.ReadInt32();
            var_1091 = (int)((uint)var_1091 >> 4 | (uint)var_1091 << 28);
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
