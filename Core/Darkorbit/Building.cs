using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PolskaBot.Core.Darkorbit
{
    public class Building
    {
        public string Name { get; set; }
        public Point Position { get; set; }

        public Building(string name, int x, int y)
        {
            Name = name;
            Position = new Point(x, y);
        }
    }
}
