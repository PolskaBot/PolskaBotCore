using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class DroneFormationUpdated : Command
    {
        public const ushort ID = 20561;

        public int DroneFormation { get; private set; } //class_411 | var_3644
        public int UID { get; private set; }

        public DroneFormationUpdated(EndianBinaryReader reader)
        {
            UID = reader.ReadInt32();
            UID = (int)((uint)UID << 8 | (uint)UID >> 24);
            reader.ReadUInt16();
            DroneFormation = reader.ReadInt32();
            DroneFormation = (int)((uint)DroneFormation << 14 | (uint)DroneFormation >> 18);
        }
    }
}
