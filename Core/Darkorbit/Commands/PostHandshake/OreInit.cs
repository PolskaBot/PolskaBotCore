using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class OreInit : Command
    {
        public const ushort ID = 27985;

        public string Hash { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public ushort Type { get; private set; }

        public OreInit(EndianBinaryReader reader)
        {
            X = reader.ReadInt32();
            X = (int)((uint)X << 3 | (uint)X >> 29);
            Y = reader.ReadInt32();
            Y = (int)((uint)Y >> 16 | (uint)Y >> 16);
            Hash = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            reader.ReadUInt16();
            reader.ReadUInt16(); // id to reader class, skipped just because
            Type = reader.ReadUInt16();
            reader.ReadUInt16();
        }
    }
}
