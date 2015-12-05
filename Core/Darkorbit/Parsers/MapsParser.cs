using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PolskaBot.Core.Darkorbit.Parsers
{
    class MapsParser
    {
        string content { get; set; }

        public int mapID { get; private set; }
        public string IP { get; private set; }

        public MapsParser(string content, int mapID)
        {
            this.content = content;
            this.mapID = mapID;
            Parse();
        }

        public void Parse()
        {
            Match match = Regex.Match(this.content, $"<map id=\"{this.mapID}\"><gameserverIP>([0-9.]+)</gameserverIP></map>");
            if (match.Success)
                IP = match.Groups[1].ToString();
        }
    }
}
