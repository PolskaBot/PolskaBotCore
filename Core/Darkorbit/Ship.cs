using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit
{
    public class Ship
    {

        public int UserID { get; set; }
        public string Username { get; set; }
        public bool NPC { get; set; }

        // Movement
        public int X { get; set; }
        public int Y { get; set; }

        // Ship
        public string Shipname { get; set; }

        //Statistics
        public bool Cloaked { get; set; }

        // Social
        public int ClanID { get; set; }
        public string ClanTag { get; set; }
        public int FactionID { get; set; }

    }
}
