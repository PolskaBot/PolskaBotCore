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

        public string hash { get; private set; }
        public uint x { get; private set; }
        public uint y { get; private set; }
        public ushort type { get; private set; }

        public OreInit(EndianBinaryReader reader)
        {
            x = reader.ReadUInt32();
            x = x << 5 | x >> 27;
            reader.ReadInt16();
            hash = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            y = reader.ReadUInt32();
            y = y << 3 | y >> 29;
            reader.ReadUInt16(); // id to reader class, skipped just because
            type = reader.ReadUInt16();
            reader.ReadUInt16();

        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
