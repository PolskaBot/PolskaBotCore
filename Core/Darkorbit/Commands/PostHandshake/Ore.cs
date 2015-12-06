using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class Ore : Command
    {
        public const ushort ID = 13039;

        public string hash { get; private set; }
        public uint x { get; private set; }
        public uint y { get; private set; }
        
        public enum Type
        {
            PROMETIUM, ENDURIUM, TERBIUM, XENOMIT, PROMETID, DURANIUM, PROMERIUM, SEPROM, PALLADIUM
        }

        public Type type;

        public Ore(EndianBinaryReader reader)
        {
            hash = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            x = reader.ReadUInt32();
            x = x >> 12 | x << 20;
            y = reader.ReadUInt32();
            y = y << 12 | y >> 20;
            reader.ReadUInt16(); // id to reader class, skipped just because
            ushort tempType = reader.ReadUInt16();

            switch(tempType)
            {
                case 0:
                    type = Type.PROMETIUM;
                    break;
                case 1:
                    type = Type.ENDURIUM;
                    break;
                case 2:
                    type = Type.TERBIUM;
                    break;
                case 3:
                    type = Type.XENOMIT;
                    break;
                case 4:
                    type = Type.PROMETID;
                    break;
                case 5:
                    type = Type.DURANIUM;
                    break;
                case 6:
                    type = Type.PROMERIUM;
                    break;
                case 7:
                    type = Type.SEPROM;
                    break;
                case 8:
                    type = Type.PALLADIUM;
                    break;
            }

        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
