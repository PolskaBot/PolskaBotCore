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

        public VanillaClient vanillaClient;
        public FadeClient fadeClient;

        public API(string IP, Mode mode = Mode.BOT)
        {
            this.IP = IP;
            this.mode = mode;

            // Depedency injection
            vanillaClient = new VanillaClient(this);
            fadeClient = new FadeClient(this);

            fadeClient.OnConnected += (s, args) => ((Client)s).thread.Abort();
            fadeClient.OnConnected += (o, e) => vanillaClient.Connect(IP, 8080);
            vanillaClient.OnConnected += (o, e) => vanillaClient.Send(new ClientVersionCheck(Config.MAJOR, Config.MINOR, Config.BUILD));

            fadeClient.Connect("25.139.200.52", 8081);
        }
    }
}
