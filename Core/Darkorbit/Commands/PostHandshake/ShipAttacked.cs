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
        public const ushort ID = 26309;

        public int UserID { get; private set; }         // name_143
        public int Var_250 { get; private set; }
        public bool Var_2969 { get; private set; }
        public int AttackerID { get; private set; }     // name_128
        public bool Var_2560 { get; private set; }

        public ShipAttacked(EndianBinaryReader reader)
        {
            UserID = reader.ReadInt32();
            UserID = (int)((uint)UserID >> 5 | (uint)UserID << 27);
            AttackerID = reader.ReadInt32();
            AttackerID = (int)((uint)AttackerID >> 16 | (uint)AttackerID << 16);
            Var_2560 = reader.ReadBoolean();
            Var_2969 = reader.ReadBoolean();
            Var_250 = reader.ReadInt32();
            Var_250 = (int)((uint)Var_250 >> 10 | (uint)Var_250 << 22);
        }
    }
}
