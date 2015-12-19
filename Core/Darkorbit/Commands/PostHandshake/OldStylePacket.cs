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
        public const ushort ID = 32601;

        public string Message { get; set; }

        public OldStylePacket(EndianBinaryReader reader = null)
        {
            if(reader != null)
                Message = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
        }

        public override void Write()
        {
            packetWriter.Write(4 + Message.Length);
            packetWriter.Write(ID);
            packetWriter.Write((UInt16)Message.Length);
            packetWriter.Write(Encoding.UTF8.GetBytes(Message));
        }
    }
}
