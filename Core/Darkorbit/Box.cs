using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PolskaBot.Core.Darkorbit
{
    public class Box
    {
        public string hash { get; private set; }
        public Point pos { get; private set; }
        public string type { get; private set; }

        public Box(string hash, int x, int y, string type)
        {
            this.hash = hash;
            pos = new Point(x, y);
            this.type = type;
        }
    }
}
