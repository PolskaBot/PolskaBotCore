using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PolskaBot.Core.Darkorbit
{
    public class Gate
    {
        public int ID { get; set; }
        public Point pos { get; set; }

        public Gate(int ID, int x, int y)
        {
            this.ID = ID;
            pos = new Point(x, y);
        }
    }
}
