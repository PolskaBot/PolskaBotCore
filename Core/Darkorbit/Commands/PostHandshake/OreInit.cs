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
        public const ushort ID = 13039;

        public string hash { get; private set; }
        public uint x { get; private set; }
        public uint y { get; private set; }
        public ushort type { get; private set; }

        public OreInit(EndianBinaryReader reader)
        {
            hash = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            x = reader.ReadUInt32();
            x = x >> 12 | x << 20;
            y = reader.ReadUInt32();
            y = y << 12 | y >> 20;
            reader.ReadUInt16(); // id to reader class, skipped just because
            type = reader.ReadUInt16();

        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
