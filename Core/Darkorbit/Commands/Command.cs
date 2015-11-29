using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MiscUtil.IO;
using MiscUtil.Conversion;
using PolskaBot.Core.Darkorbit.Commands;
using System.Net.Sockets;
using PolskaBot.Core.Darkorbit;

namespace PolskaBot.Core.Darkorbit.Commands
{
    abstract class Command
    {
        protected MemoryStream memoryStream;
        protected EndianBinaryWriter packetWriter;

        public Command()
        {
            memoryStream = new MemoryStream();
            packetWriter = new EndianBinaryWriter(EndianBitConverter.Big, memoryStream);
        }

        public void Write(NetworkStream stream)
        {
            byte[] buffer = memoryStream.ToArray();
            stream.Write(buffer, 0, buffer.Length);
        }

        public abstract void Write();
        public abstract short GetID();
    }
}
