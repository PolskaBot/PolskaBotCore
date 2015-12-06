using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class OldStylePacket : Command
    {
        public const ushort ID = 7033;

        public string message { get; private set; }

        public OldStylePacket(EndianBinaryReader reader)
        {
            message = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
