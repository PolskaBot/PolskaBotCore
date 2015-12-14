using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class DestroyShip : Command
    {
        public const ushort ID = 14332;

        public uint id { get; private set; }

        public DestroyShip(EndianBinaryReader reader)
        {
            reader.ReadInt16();
            id = reader.ReadUInt32();
            id = id << 9 | id >> 23;
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
