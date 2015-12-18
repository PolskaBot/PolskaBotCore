using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    public class CollectBox : Command
    {
        public const ushort ID = 28222;

        public int BoxPosX { get; private set; } //var_3691
        public string Hash { get; private set; } //var_3882
        public int ShipPosX { get; private set; } //var_4812
        public int BoxPosY { get; private set; } //var_722
        public int ShipPosY { get; private set; } //var_2324

        public CollectBox(string Hash, int BoxPosX, int BoxPosY, int ShipPosX, int ShipPosY)
        {
            this.Hash = Hash;
            this.BoxPosX = BoxPosX;
            this.BoxPosY = BoxPosY;
            this.ShipPosX = ShipPosX;
            this.ShipPosY = ShipPosY;
            Write();
        }

        public override void Write()
        {
            packetWriter.Write(24 + Hash.Length);
            packetWriter.Write(ID);
            packetWriter.Write((int)((uint)BoxPosX >> 2 | (uint)BoxPosX << 30));
            packetWriter.Write((short)-27233);
            packetWriter.Write((UInt16)Hash.Length);
            packetWriter.Write(Encoding.UTF8.GetBytes(Hash));
            packetWriter.Write((int)((uint)ShipPosX << 14 | (uint)ShipPosX >> 18));
            packetWriter.Write((short)31654);
            packetWriter.Write((int)((uint)BoxPosY >> 16 | (uint)BoxPosY << 16));
            packetWriter.Write((int)((uint)ShipPosY >> 6 | (uint)ShipPosY << 26));
        }
    }
}
