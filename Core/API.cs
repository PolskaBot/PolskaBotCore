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
        public Account Account { get; set; }
        public List<Box> Boxes { get; set; } = new List<Box>();
        public List<Box> MemorizedBoxes { get; set; } = new List<Box>();
        public List<Ore> Ores { get; set; } = new List<Ore>();
        public List<Ship> Ships { get; set; } = new List<Ship>();
        public List<Gate> Gates { get; set; } = new List<Gate>();
        public List<Building> Buildings { get; set; } = new List<Building>();

        public API(Mode mode = Mode.BOT)
        {
            this.mode = mode;

            // Depedency injection
            Account = new Account(this);
            vanillaClient = new VanillaClient(this);
            fadeClient = new FadeClient(this);

            Account.LoginSucceed += (s, e) => Connect();
        }

        public void Login(string username = null, string password = null)
        {
            if(username == null || password == null)
            {
                username = Environment.GetEnvironmentVariable(Config.USERNAME_ENV);
                password = Environment.GetEnvironmentVariable(Config.PASSWORD_ENV);
            }
            Account.SetCredentials(username, password);
            Account.Login();
        }

        public void Connect()
        {
            Console.WriteLine("Connecting");
            fadeClient.OnConnected += (s, args) => ((Client)s).thread.Abort();
            fadeClient.OnConnected += (o, e) => vanillaClient.Connect(GetIP(), 8080);
            vanillaClient.OnConnected += (o, e) => vanillaClient.Send(new ClientVersionCheck(Config.MAJOR, Config.MINOR, Config.BUILD));
            vanillaClient.Disconnected += (o, e) => Reconnect();

            fadeClient.Connect(Environment.GetEnvironmentVariable(Config.SERVER_IP_ENV), 8081);
        }

        public void Reconnect()
        {
            Console.WriteLine("Connection lost. Reconnecting.");
            vanillaClient.pingThread.Abort();
            Boxes.Clear();
            MemorizedBoxes.Clear();
            Ores.Clear();
            Ships.Clear();
            Gates.Clear();
            Buildings.Clear();
            fadeClient.Send(new FadePandoraReset());
            vanillaClient.Disconnect();
            vanillaClient.Connect(GetIP(), 8080);
        }

        public string GetIP()
        {
            var webClient = new WebClient();
            var response = webClient.DownloadString($"http://{Account.Server}.darkorbit.bigpoint.com/spacemap/xml/maps.php");
            var match = Regex.Match(response, $"<map id=\"{Account.Map}\"><gameserverIP>([0-9\\.]+)</gameserverIP></map>");
            return match.Groups[1].ToString();
        }
    }
}
