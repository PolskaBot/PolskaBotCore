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

        public string hash { get; private set; }
        public int x { get; private set; }
        public int y { get; private set; }
        public string type { get; private set; }

        public BoxInit(EndianBinaryReader reader)
        {
            x = reader.ReadInt32();
            x = (int)((uint)x << 5 | (uint)x >> 27);
            reader.ReadInt16();
            hash = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            y = reader.ReadInt32();
            y = (int)((uint)y << 3 | (uint)y >> 29);
            type = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
