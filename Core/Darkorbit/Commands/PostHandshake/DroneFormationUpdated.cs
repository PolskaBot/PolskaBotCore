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
        public const ushort ID = 32691;

        public int DroneFormation { get; private set; } //class_411 | var_3642
        public int UID { get; private set; }

        public DroneFormationUpdated(EndianBinaryReader reader)
        {
            reader.ReadUInt16();
            DroneFormation = reader.ReadInt32();
            DroneFormation = (int)((uint)DroneFormation >> 12 | (uint)DroneFormation << 20);
            UID = reader.ReadInt32();
            UID = (int)((uint)UID >> 13 | (uint)UID << 19);
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
