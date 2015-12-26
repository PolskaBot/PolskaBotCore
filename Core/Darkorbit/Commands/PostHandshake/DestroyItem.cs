using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class DestroyItem : Command
    {
        public const ushort ID = 24098;

        public bool CollectedByPlayer { get; private set; }
        public string Hash { get; private set; }

        public DestroyItem(EndianBinaryReader reader)
        {
            reader.ReadInt16();
            CollectedByPlayer = reader.ReadBoolean();
            Hash = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
        }
    }
}
