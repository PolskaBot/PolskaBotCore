using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PolskaBot.Core.Darkorbit
{
    public enum Type
    {
        PROMETIUM, ENDURIUM, TERBIUM, XENOMIT, PROMETID, DURANIUM, PROMERIUM, SEPROM, PALLADIUM
    }

    public class Ore
    {
        public string Hash { get; private set; }
        public Point Position { get; private set; }
        public Type Type { get; private set; }

        public Ore(string hash, int x, int y, int type)
        {
            Hash = hash;
            Position = new Point(x, y);
            Type = (Type)type;
        }
    }
}
