using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class Ore : Command
    {
        public const ushort ID = 13944;

        public string hash { get; private set; }
        public uint x { get; private set; }
        public uint y { get; private set; }

        public Ore(EndianBinaryReader reader)
        {
            hash = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            x = reader.ReadUInt32();
            x = x >> 12 | x << 20;
            y = reader.ReadUInt32();
            y = y << 12 | y >> 20;
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
