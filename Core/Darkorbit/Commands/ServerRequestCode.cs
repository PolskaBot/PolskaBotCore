using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class ServerRequestCode : Command
    {
        public const short ID = 14047;

        public int codeLength { get; private set; }
        public byte[] code { get; private set; }
        public int fakeSize { get; private set; }

        public ServerRequestCode(EndianBinaryReader reader)
        {
            codeLength = reader.ReadInt32();
            code = reader.ReadBytes(codeLength);
            fakeSize = reader.ReadInt32();
        }

        public override void Write()
        {
            return;
        }
    }
}
