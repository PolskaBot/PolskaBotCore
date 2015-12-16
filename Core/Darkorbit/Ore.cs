using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PolskaBot.Core.Darkorbit
{
    public class Ore
    {
        public string Hash { get; private set; }
        public Point Position { get; private set; }
        public OreType Type { get; private set; }

        public enum OreType
        {
            PROMETIUM, ENDURIUM, TERBIUM, XENOMIT, PROMETID, DURANIUM, PROMERIUM, SEPROM, PALLADIUM
        }

        public Ore(string hash, int x, int y, int type)
        {
            Hash = hash;
            Position = new Point(x, y);
            Type = (OreType)type;
        }
    }
}
