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
        public const ushort ID = 16273;

        public double CargoCount { get; private set; }

        public CargoUpdated(EndianBinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    reader.ReadInt16();                                 //16273  | class_919
                    reader.ReadInt16();                                 //6823   | class_364
                    int OreID = reader.ReadUInt16();                    //26978  | class_171
                    double Amount = reader.ReadDouble();                //6823   | class_364
                    if ((Ore.OreType)OreID != Ore.OreType.XENOMIT)
                        CargoCount += Amount;
                }
            }
            reader.ReadUInt16();
        }
    }
}
