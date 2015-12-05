using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PolskaBot.Core.Darkorbit.Parsers
{
    class IndexParser
    {
        public int instanceID { get; private set; }
        public int mapID { get; set; }
        public string sid { get; private set; }
        public int userID { get; private set; }

        public string content { get; private set; }

        public IndexParser(string content)
        {
            this.content = content;
            Parse();
        }

        public void Parse()
        {
            Match match = Regex.Match(content, "pid\": \"([0-9]+)");
            if (match.Success)
                instanceID = int.Parse(match.Groups[1].ToString());
            match = Regex.Match(content, "mapID\": \"([0-9]+)");
            if (match.Success)
                mapID = int.Parse(match.Groups[1].ToString());
            match = Regex.Match(content, "sessionID\": \"([0-9a-z]+)");
            if (match.Success)
                sid = match.Groups[1].ToString();
            match = Regex.Match(content, "userID\": \"([0-9]+)");
            if (match.Success)
                userID = int.Parse(match.Groups[1].ToString());
        }
    }
}
