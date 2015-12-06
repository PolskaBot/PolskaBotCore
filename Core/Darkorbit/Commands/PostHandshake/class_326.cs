using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class class_326 : Command
    {
        public const ushort ID = 18044;

        public bool activated { get; private set; }
        public uint var_2742 { get; private set; }
        public string var_2554 { get; private set; }
        public int attribute { get; private set; }
        public int name_125 { get; private set; }
        public int count { get; private set; }

        public class_326(EndianBinaryReader reader)
        {
            this.activated = reader.ReadBoolean();
            reader.ReadUInt16();
            this.var_2742 = reader.ReadUInt32();
            this.var_2554 = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
            this.attribute = reader.ReadInt32();
            this.attribute = this.attribute << 6 | this.attribute >> 26;
            this.name_125 = reader.ReadInt32();
            this.name_125 = this.name_125 << 5 | this.name_125 >> 27;
            this.count = reader.ReadInt32();
            this.count = this.count << 9 | this.count >> 23;
        }

        public override void Write()
        {
            return;
        }
    }
}