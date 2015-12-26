using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class BoxInit : Command
    {
        public const ushort ID = 20862;

        public string Hash { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public string Type { get; private set; }

        public BoxInit(EndianBinaryReader reader)
        {
            X = reader.ReadInt32();
            X = (int)((uint)X << 5 | (uint)X >> 27);
            reader.ReadInt16();
            Hash = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            Y = reader.ReadInt32();
            Y = (int)((uint)Y << 3 | (uint)Y >> 29);
            Type = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
        }
    }
}
