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
        public const ushort ID = 2540;

        public int Damage { get; private set; }
        public int DamageType { get; private set; } //var_458 | check class_640
        public int NanoHP { get; private set; } //Var_1654
        public int Shield { get; private set; } //Var_1574
        public int HP { get; private set; } //Var_2158
        public int UserID { get; private set; } //Var_4879
        public int Name_128 { get; private set; }
        public bool Var_4939 { get; private set; }

        public ShipUpdated(EndianBinaryReader reader)
        {
            Damage = reader.ReadInt32();
            Damage = (int)((uint)Damage >> 8 | (uint)Damage << 24);
            NanoHP = reader.ReadInt32();
            NanoHP = (int)((uint)NanoHP >> 1 | (uint)NanoHP << 31);
            Shield = reader.ReadInt32();
            Shield = (int)((uint)Shield << 6 | (uint)Shield >> 26);
            HP = reader.ReadInt32();
            HP = (int)((uint)HP >> 7 | (uint)HP << 25);
            reader.ReadUInt16(); //get into class_640
            DamageType = reader.ReadUInt16(); //
            UserID = reader.ReadInt32();
            UserID = (int)((uint)UserID << 6 | (uint)UserID >> 26);
            Name_128 = reader.ReadInt32();
            Name_128 = (int)((uint)Name_128 << 2 | (uint)Name_128 >> 30);
            Var_4939 = reader.ReadBoolean();
            reader.ReadUInt16();
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
