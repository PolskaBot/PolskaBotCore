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
        public const ushort ID = 1771;

        public uint UserID { get; private set; } //name_125
        public int X { get; private set; }
        public int Y { get; private set; }
        public uint Duration { get; private set; } //var_3506

        public ShipMove(EndianBinaryReader reader)
        {
            reader.ReadInt16();
            reader.ReadInt16();
            Y = reader.ReadInt32();
            Y = (int)((uint)Y << 1 | (uint)Y >> 31);
            UserID = reader.ReadUInt32();
            UserID = UserID << 10 | UserID >> 22;
            X = reader.ReadInt32();
            X = (int)((uint)X >> 6 | (uint)X << 26);
            Duration = reader.ReadUInt32();
            Duration = Duration >> 11 | Duration << 21;
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
