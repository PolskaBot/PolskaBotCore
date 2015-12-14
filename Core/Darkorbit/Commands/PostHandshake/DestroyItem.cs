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

        public bool name_44 { get; private set; }
        public string hash { get; private set; }

        public DestroyItem(EndianBinaryReader reader)
        {
            reader.ReadInt16();
            name_44 = reader.ReadBoolean();
            hash = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
