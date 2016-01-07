using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    public class ShipMove : Command
    {
        public const ushort ID = 17441;

        public uint UserID { get; private set; } //name_125
        public int X { get; private set; }
        public int Y { get; private set; }
        public uint Duration { get; private set; } //var_3506

        public ShipMove(EndianBinaryReader reader)
        {
            Duration = reader.ReadUInt32();
            Duration = Duration >> 13 | Duration << 19;
            Y = reader.ReadInt32();
            Y = (int)((uint)Y >> 3 | (uint)Y << 29);
            UserID = reader.ReadUInt32();
            UserID = UserID >> 8 | UserID << 24;
            X = reader.ReadInt32();
            X = (int)((uint)X << 2 | (uint)X >> 30);
        }
    }
}
