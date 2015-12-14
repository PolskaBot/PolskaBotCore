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

        public string hash { get; private set; }
        public Point pos { get; private set; }
        public Type type { get; private set; }

        public enum Type
        {
            PROMETIUM, ENDURIUM, TERBIUM, XENOMIT, PROMETID, DURANIUM, PROMERIUM, SEPROM, PALLADIUM
        }

        public Ore(string hash, int x, int y, int type)
        {
            this.hash = hash;
            pos = new Point(x, y);
            this.type = (Type)type;
        }
    }
}
