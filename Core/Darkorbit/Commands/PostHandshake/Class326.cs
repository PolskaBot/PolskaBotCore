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
        public ushort var_2742 { get; private set; }
        public string var_2555 { get; private set; }
        public int Attribute { get; private set; }
        public int name_125 { get; private set; }
        public int Count { get; private set; }

        public Class326(EndianBinaryReader reader)
        {
            Attribute = reader.ReadInt32();
            Attribute = Attribute << 2 | Attribute >> 30;
            var_2555 = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
            name_125 = reader.ReadInt32();
            name_125 = name_125 << 5 | name_125 >> 27;
            Count = reader.ReadInt32();
            Count = Count >> 7 | Count >> 25;
            Activated = reader.ReadBoolean();
            var_2742 = reader.ReadUInt16();
        }

        public override void Write()
        {
            return;
        }
    }
}