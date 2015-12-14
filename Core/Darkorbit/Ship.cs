using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit
{
    public class Ship
    {

        public int userID { get; set; }
        public string userName { get; set; }
        public bool npc { get; set; }

        // Movement
        public int X { get; set; }
        public int Y { get; set; }

        // Ship
        public string shipName { get; set; }

        //Statistics
        public bool cloaked { get; set; }

        // Social
        public int clanID { get; set; }
        public string clanTag { get; set; }
        public int factionID { get; set; }

    }
}
