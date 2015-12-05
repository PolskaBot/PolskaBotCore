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

        public API(Mode mode = Mode.BOT)
        {
            this.mode = mode;
            mergedClient = new MergedClient(this);
        }

        public void Login(string server, string sid)
        {
            mergedClient.Connect(server, sid);
            mergedClient.vanillaClient.Send(new ClientVersionCheck(Config.MAJOR, Config.MINOR, Config.BUILD));
        }
    }
}
