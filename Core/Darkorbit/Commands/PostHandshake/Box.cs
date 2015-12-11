using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class Box : Command
    {
        public const ushort ID = 18357;

        public string hash { get; private set; }
        public uint x { get; private set; }
        public uint y { get; private set; }
        public string type { get; private set; }

        public Box(EndianBinaryReader reader)
        {
            x = reader.ReadUInt32();
            x = x << 5 | x >> 27;
            reader.ReadInt16();
            hash = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            y = reader.ReadUInt32();
            y = y << 3 | y >> 29;
            type = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            reader.ReadInt16();
            reader.ReadInt16();
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
