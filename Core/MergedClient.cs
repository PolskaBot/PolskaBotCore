using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core
{
    class MergedClient
    {
        public API api { get; private set; }
        public VanillaClient vanillaClient { get; private set; }
        public FadeClient fadeClient { get; private set; }

        public MergedClient(API api)
        {
            this.api = api;
            vanillaClient = new VanillaClient(this);
            fadeClient = new FadeClient(this);
            fadeClient.OnConnected += (s, args) => ((Client)s).thread.Abort();
        }

        public void Connect(string IP)
        {
            vanillaClient.Connect(IP, 8080);
            fadeClient.Connect("127.0.0.1", 8081);
        }
    }
}
