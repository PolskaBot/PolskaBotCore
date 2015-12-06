using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class Collected : Command
    {
        public const ushort ID = 5677;

        public string hash { get; private set; }

        public Collected(EndianBinaryReader reader)
        {
            hash = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            reader.ReadUInt16();
        }

        public override void Write()
        {
            short totalLength = (short)(hash.Length + 6);
            packetWriter.Write(totalLength);
            packetWriter.Write(ID);
            packetWriter.Write((byte)0);
            packetWriter.Write(hash);
            packetWriter.Write((short)16992);
        }
    }
}
