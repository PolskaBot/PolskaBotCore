using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class Class326 : Command
    {
        public const ushort ID = 18881;

        public bool Activated { get; private set; }
        public ushort var_2744 { get; private set; }
        public string var_2555 { get; private set; }
        public int Attribute { get; private set; }
        public int name_125 { get; private set; }
        public int Count { get; private set; }

        public Class326(EndianBinaryReader reader)
        {
            var_2555 = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
            Attribute = reader.ReadInt32();
            Attribute = Attribute >> 13 | Attribute << 19;
            var_2744 = reader.ReadUInt16();
            Activated = reader.ReadBoolean();
            name_125 = reader.ReadInt32();
            name_125 = name_125 << 13 | name_125 >> 19;
            Count = reader.ReadInt32();
            Count = Count << 13 | Count >> 19;
        }
    }
}