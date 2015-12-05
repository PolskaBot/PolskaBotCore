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
        public MapCredentials mapCredentials = new MapCredentials();

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
                mapCredentials.instanceID = int.Parse(match.Groups[1].ToString());
            match = Regex.Match(content, "mapID\": \"([0-9]+)");
            if (match.Success)
                mapCredentials.mapID = int.Parse(match.Groups[1].ToString());
            match = Regex.Match(content, "sessionID\": \"([0-9a-z]+)");
            if (match.Success)
                mapCredentials.sid = match.Groups[1].ToString();
            match = Regex.Match(content, "userID\": \"([0-9]+)");
            if (match.Success)
                mapCredentials.userID = int.Parse(match.Groups[1].ToString());
        }
    }
}
