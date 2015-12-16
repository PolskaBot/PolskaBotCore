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
        public string Hash { get; private set; }
        public Point Position { get; private set; }
        public string Type { get; private set; }

        public Box(string hash, int x, int y, string type)
        {
            Hash = hash;
            Position = new Point(x, y);
            Type = type;
        }
    }
}
