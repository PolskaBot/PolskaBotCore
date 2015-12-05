using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolskaBot.Core.Darkorbit;
using PolskaBot.Core.Darkorbit.Commands;

namespace PolskaBot.Core
{
    public class API
    {
        public enum Mode
        {
            BOT, PROXY
        };

        public Mode mode;

        MergedClient mergedClient;

        MapsServer mapsServer;

        public API(Mode mode = Mode.BOT)
        {
            this.mode = mode;
            mergedClient = new MergedClient(this);

            if(mode == Mode.PROXY)
            {
                mapsServer = new MapsServer(9000);
            }
        }

        public void Login(string server, string sid)
        {
            if(mode == Mode.PROXY) {
                throw new NotSupportedException("Login cannot be called when in PROXY mode");
            }
            mergedClient.Connect(server, sid);
            mergedClient.vanillaClient.Send(new ClientVersionCheck(Config.MAJOR, Config.MINOR, Config.BUILD));
        }
    }
}
