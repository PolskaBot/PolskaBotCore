using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class ShipUpdated : Command
    {
        public const ushort ID = 17263;

        public int Damage { get; private set; }
        public int DamageType { get; private set; } //  var_456 | check class_640
        public int NanoHP { get; private set; }     // var_1654
        public int Shield { get; private set; }     // var_1574
        public int HP { get; private set; }         // var_2158
        public int UserID { get; private set; }     // var_4879
        public int Name_128 { get; private set; }
        public bool Var_4939 { get; private set; }

        public ShipUpdated(EndianBinaryReader reader)
        {
            NanoHP = reader.ReadInt32();
            NanoHP = (int)((uint)NanoHP >> 3 | (uint)NanoHP << 29);
            reader.ReadUInt16(); //get into class_640
            DamageType = reader.ReadUInt16(); //
            Damage = reader.ReadInt32();
            Damage = (int)((uint)Damage >> 6 | (uint)Damage << 26);
            Name_128 = reader.ReadInt32();
            Name_128 = (int)((uint)Name_128 >> 7 | (uint)Name_128 << 25);
            UserID = reader.ReadInt32();
            UserID = (int)((uint)UserID >> 9 | (uint)UserID << 23);
            Shield = reader.ReadInt32();
            Shield = (int)((uint)Shield >> 9 | (uint)Shield << 23);
            HP = reader.ReadInt32();
            HP = (int)((uint)HP >> 14 | (uint)HP << 18);
            Var_4939 = reader.ReadBoolean();
        }
    }
}
