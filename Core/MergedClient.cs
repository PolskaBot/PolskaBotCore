using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core
{
    class MergedClient
    {
        public VanillaClient vanillaClient { get; private set; }
        public FadeClient fadeClient { get; private set; }

        public MergedClient()
        {
            vanillaClient = new VanillaClient(this);
            fadeClient = new FadeClient(this);
        }

        public void Connect(string IP, string sid)
        {
            vanillaClient.Connect(IP, 8080);
            fadeClient.Connect("127.0.0.1", 8081);
        }
    }
}
