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

        public uint UserID { get; private set; }

        public DestroyShip(EndianBinaryReader reader)
        {
            reader.ReadInt16();
            UserID = reader.ReadUInt32();
            UserID = UserID << 9 | UserID >> 23;
        }
    }
}
