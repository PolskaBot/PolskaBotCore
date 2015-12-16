using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class HeroInit : Command
    {
        public const ushort ID = 11451;

        public float jackpot { get; private set; }
        public uint maxShield { get; private set; } //name_101
        public bool premium { get; private set; }
        public bool var_4831 { get; private set; }
        public double credits { get; private set; }
        public double honor { get; private set; } //var_4055
        public uint clanID { get; private set; } //name_46
        public double uridium { get; private set; }
        public bool var_3674 { get; private set; }
        public uint rank { get; private set; } //name_134
        public bool cloaked { get; private set; }
        public string userName { get; private set; } //var_3497
        public uint speed { get; private set; }
        public uint cargoCapacity { get; private set; } //var_3017
        public uint shield { get; private set; }
        public int x { get; private set; }
        public int y { get; private set; }
        public uint userID { get; private set; } //name_125
        public uint var_3378 { get; private set; }
        public uint galaxyGatesDone { get; private set; } //var_3911
        public uint freeCargoSpace { get; private set; } //var_4295
        public string shipName { get; private set; } //name_122
        public uint hp { get; private set; } //var_1063
        public uint level { get; private set; }
        public uint nanoHP { get; private set; } //var_2222
        public double XP { get; private set; } //var_4549
        public uint mapId { get; private set; }
        public uint factionID { get; private set; }
        public string clanTag { get; private set; } //name_138
        public uint maxHP { get; private set; } //var_1851
        public uint maxNanoHP { get; private set; } //var_1600

        public HeroInit(EndianBinaryReader reader)
        {
            cargoCapacity = reader.ReadUInt32();
            cargoCapacity = cargoCapacity << 12 | cargoCapacity >> 20;
            speed = reader.ReadUInt32();
            speed = speed << 11 | speed >> 21;
            userName = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            var_4831 = reader.ReadBoolean();
            hp = reader.ReadUInt32();
            hp = hp >> 1 | hp << 31;
            jackpot = reader.ReadSingle();
            premium = reader.ReadBoolean();
            clanID = reader.ReadUInt32();
            clanID = clanID << 11 | clanID >> 21;
            y = reader.ReadInt32();
            y = (int)((uint)y >> 6 | (uint)y << 26);
            shipName = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            shield = reader.ReadUInt32();
            shield = shield << 9 | shield >> 23;
            galaxyGatesDone = reader.ReadUInt32();
            galaxyGatesDone = galaxyGatesDone >> 5 | galaxyGatesDone << 27;
            reader.ReadInt16();
            clanTag = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
            mapId = reader.ReadUInt32();
            mapId = mapId << 14 | mapId >> 18;
            maxShield = reader.ReadUInt32();
            maxShield = maxShield >> 9 | maxShield << 23;
            level = reader.ReadUInt32();
            level = level << 7 | level >> 25;
            x = reader.ReadInt32();
            x = (int)((uint)x >> 5 | (uint)x << 27);
            XP = reader.ReadDouble();
            factionID = reader.ReadUInt32();
            factionID = factionID << 5 | factionID >> 27;
            freeCargoSpace = reader.ReadUInt32();
            freeCargoSpace = freeCargoSpace << 3 | freeCargoSpace >> 29;
            honor = reader.ReadDouble();
            uridium = reader.ReadDouble();
            maxNanoHP = reader.ReadUInt32();
            maxNanoHP = maxNanoHP << 5 | maxNanoHP >> 27;
            rank = reader.ReadUInt32();
            rank = rank >> 13 | rank << 19;
            userID = reader.ReadUInt32();
            userID = userID >> 11 | userID << 21;
            maxHP = reader.ReadUInt32();
            maxHP = maxHP >> 14 | maxHP << 18;
            for (int i = 0; i < reader.ReadInt32(); i++)
            {
                class_326 class326 = new class_326(reader);
            }
            cloaked = reader.ReadBoolean();
            var_3378 = reader.ReadUInt32();
            var_3378 = var_3378 << 9 | var_3378 >> 23;
            nanoHP = reader.ReadUInt32();
            nanoHP = nanoHP << 3 | nanoHP >> 29;
            credits = reader.ReadDouble();
            var_3674 = reader.ReadBoolean();
        }

        public override void Write()
        {
            return;
        }

    }
}