using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    public class ShipAttacked : Command
    {
        public const ushort ID = 4160;

        public int UserID { get; private set; } //Name_143
        public int Var_250 { get; private set; }
        public bool Var_2969 { get; private set; }
        public int Name_128 { get; private set; }
        public bool Var_2560 { get; private set; }

        public ShipAttacked(EndianBinaryReader reader)
        {
            UserID = reader.ReadInt32();
            UserID = (int)((uint)UserID >> 3 | (uint)UserID << 29);
            reader.ReadUInt16();
            Var_250 = reader.ReadInt32();
            Var_250 = (int)((uint)Var_250 << 8 | (uint)Var_250 >> 24);
            Var_2969 = reader.ReadBoolean();
            Name_128 = reader.ReadInt32();
            Name_128 = (int)((uint)Name_128 >> 7 | (uint)Name_128 << 25);
            Var_2560 = reader.ReadBoolean();
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
