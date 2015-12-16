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
        public const ushort ID = 13463;

        public string Hash { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public ushort Type { get; private set; }

        public OreInit(EndianBinaryReader reader)
        {
            X = reader.ReadInt32();
            X = (int)((uint)X << 5 | (uint)X >> 27);
            reader.ReadInt16();
            Hash = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            Y = reader.ReadInt32();
            Y = (int)((uint)Y << 3 | (uint)Y >> 29);
            reader.ReadUInt16(); // id to reader class, skipped just because
            Type = reader.ReadUInt16();
            reader.ReadUInt16();

        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
