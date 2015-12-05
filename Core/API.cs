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
        enum Mode
        {
            BOT, PROXY
        };

        Mode mode;

        MergedClient mergedClient = new MergedClient();

        public void Login(string server, string sid)
        {
            mergedClient.Connect(server, sid);
            mergedClient.vanillaClient.Send(new ClientVersionCheck(Config.MAJOR, Config.MINOR, Config.BUILD));
        }
    }
}
