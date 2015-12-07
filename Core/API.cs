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
            BOT
        };

        public Mode mode;

        public string IP;

        public MergedClient mergedClient;

        public API(Mode mode = Mode.BOT)
        {
            this.mode = mode;
            mergedClient = new MergedClient(this);
            mergedClient.vanillaClient.OnConnected += (o, e) => mergedClient.vanillaClient.Send(new ClientVersionCheck(Config.MAJOR, Config.MINOR, Config.BUILD));
        }

        public void Connect(string server = null)
        {
            mergedClient.Connect(server);
        }
    }
}
