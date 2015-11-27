using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;
using MiscUtil.Conversion;

namespace Core.Darkorbit.Commands
{
    class ServerRequestCode : Command
    {
        public const short ID = 14047;

        public int length { get; private set; }
        public byte[] code { get; private set; }

        public ServerRequestCode(EndianBinaryReader reader)
        {
            length = reader.ReadInt32();
            code = reader.ReadBytes(length);
            reader.ReadInt32(); // fakeSize
        }

        public override short GetID()
        {
            return ID;
        }

        public override void Write()
        {
            return;
        }
    }
}
