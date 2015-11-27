using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MiscUtil.IO;
using MiscUtil.Conversion;
using Core.Darkorbit.Commands;
using System.Net.Sockets;
using Core.Darkorbit;

namespace Core.Darkorbit.Commands
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

        public static Command Read(byte[] buffer)
        {
            MemoryStream memoryStream = new MemoryStream(buffer);
            EndianBinaryReader reader = new EndianBinaryReader(EndianBitConverter.Big, memoryStream);

            short length = reader.ReadInt16();
            short id = reader.ReadInt16();

            switch(id)
            {
                case ServerVersionCheck.ID:
                    return new ServerVersionCheck(reader);
                case ServerRequestCode.ID:
                    return new ServerRequestCode(reader);
                default:
                    return new ServerUknown(id);
            }
        }

        public abstract void Write();
        public abstract short GetID();
    }
}
