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

        public VanillaClient vanillaClient;
        public FadeClient fadeClient;
        public Account account;

        public API(Mode mode = Mode.BOT)
        {
            this.mode = mode;

            // Depedency injection
            account = new Account(this);
            vanillaClient = new VanillaClient(this);
            fadeClient = new FadeClient(this);

            account.OnLoggedIn += (s, e) => Connect();
        }

        public void Login(string username = null, string password = null)
        {
            if(username == null || password == null)
            {
                username = Environment.GetEnvironmentVariable(Config.USERNAME_ENV);
                password = Environment.GetEnvironmentVariable(Config.PASSWORD_ENV);
            }
            account.SetCredentials(username, password);
            account.Login();
        }

        public void Connect()
        {
            Console.WriteLine("Connecting");
            fadeClient.OnConnected += (s, args) => ((Client)s).thread.Abort();
            fadeClient.OnConnected += (o, e) => vanillaClient.Connect("178.132.244.66", 8080);
            vanillaClient.OnConnected += (o, e) => vanillaClient.Send(new ClientVersionCheck(Config.MAJOR, Config.MINOR, Config.BUILD));

            fadeClient.Connect("25.139.200.52", 8081);
        }
    }
}
