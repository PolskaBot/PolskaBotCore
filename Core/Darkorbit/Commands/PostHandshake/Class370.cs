using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class Class370 : Command
    {
        public const ushort ID = 18881;

        public string MessageType { get; private set; }

        public Class370(EndianBinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    reader.ReadInt16();                                               //23340 | class_371
                    Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadInt16()));
                    reader.ReadInt16();                                               //3660  | class_124
                    reader.ReadInt16();
                    Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
                }
            }
            reader.ReadInt16();
            reader.ReadInt16();
            reader.ReadInt16();                                                       //3660  | class_124
            reader.ReadInt16();
            MessageType = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
        }
    }
}
