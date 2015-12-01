using System.IO;
using MiscUtil.IO;
using MiscUtil.Conversion;

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

        public byte[] ToArray()
        {
            return memoryStream.ToArray();
        }

        public abstract void Write();
    }
}
