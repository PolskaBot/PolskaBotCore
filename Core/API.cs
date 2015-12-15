using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
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

        // Logic
        public Account account;
        public List<Box> boxes { get; set; } = new List<Box>();
        public List<Ore> ores { get; set; } = new List<Ore>();
        public List<Ship> ships { get; set; } = new List<Ship>();
        public List<Gate> gates { get; set; } = new List<Gate>();
        public List<Building> buildings { get; set; } = new List<Building>();

        public API(Mode mode = Mode.BOT)
        {
            this.mode = mode;

            // Depedency injection
            account = new Account(this);
            vanillaClient = new VanillaClient(this);
            fadeClient = new FadeClient(this);

            account.LoginSucceed += (s, e) => Connect();
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
            fadeClient.OnConnected += (o, e) => vanillaClient.Connect(GetIP(), 8080);
            vanillaClient.OnConnected += (o, e) => vanillaClient.Send(new ClientVersionCheck(Config.MAJOR, Config.MINOR, Config.BUILD));

            fadeClient.Connect(Environment.GetEnvironmentVariable(Config.SERVER_IP_ENV), 8081);
        }

        public string GetIP()
        {
            var webClient = new WebClient();
            var response = webClient.DownloadString($"http://{account.serverID}.darkorbit.bigpoint.com/spacemap/xml/maps.php");
            var match = Regex.Match(response, $"<map id=\"{account.mapID}\"><gameserverIP>([0-9\\.]+)</gameserverIP></map>");
            return match.Groups[1].ToString();
        }
    }
}
