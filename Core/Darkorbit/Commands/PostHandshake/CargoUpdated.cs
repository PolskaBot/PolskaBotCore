using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class CargoUpdated : Command
    {
        public const ushort ID = 25488;

        public double CargoCount { get; private set; }

        public CargoUpdated(EndianBinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    reader.ReadInt16();                                 //class_364
                    reader.ReadInt16();
                    reader.ReadInt16();
                    double Amount = reader.ReadDouble();
                    int OreID = reader.ReadUInt16();                    //class_171
                    reader.ReadUInt16();
                    if ((Ore.OreType)OreID != Ore.OreType.XENOMIT)
                        CargoCount += Amount;
                }
            }
            reader.ReadUInt16();
        }
    }
}
