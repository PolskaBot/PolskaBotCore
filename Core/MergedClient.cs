using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core
{
    public class MergedClient
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
            fadeClient.OnConnected += (o, e) => vanillaClient.Connect(IP, 8080);
            fadeClient.Connect("25.139.200.52", 8081);
        }
    }
}
