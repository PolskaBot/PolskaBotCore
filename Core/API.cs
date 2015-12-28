using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using PolskaBot.Core.Darkorbit;
using PolskaBot.Core.Darkorbit.Commands;
using PolskaBot.Core.Darkorbit.Commands.PostHandshake;

namespace PolskaBot.Core
{
    public class API
    {
        public enum Mode
        {
            BOT
        };

        public Mode mode;

        private VanillaClient vanillaClient;
        private FadeClient fadeClient;
        private RemoteClient remoteClient;

        // Logic
        public Account Account { get; set; }
        public List<Box> Boxes { get; set; } = new List<Box>();
        public List<Box> MemorizedBoxes { get; set; } = new List<Box>();
        public List<Ore> Ores { get; set; } = new List<Ore>();
        public List<Ship> Ships { get; set; } = new List<Ship>();
        public List<Gate> Gates { get; set; } = new List<Gate>();
        public List<Building> Buildings { get; set; } = new List<Building>();

        public event EventHandler<EventArgs> Connecting;
        public event EventHandler<EventArgs> Disconnected;
        public event EventHandler<EventArgs> HeroInited;
        public event EventHandler<ShipAttacked> Attacked;
        public event EventHandler<ShipMove> ShipMoving;

        public API(Mode mode = Mode.BOT)
        {
            this.mode = mode;

            // Depedency injection
            Account = new Account(this);
            fadeClient = new FadeClient(this);
            remoteClient = new RemoteClient(this);
            vanillaClient = new VanillaClient(this, fadeClient, remoteClient);

            Account.LoginSucceed += (s, e) => Connect();

            vanillaClient.Disconnected += (s, e) => Disconnected?.Invoke(s, e);
            vanillaClient.HeroInited += (s, e) => HeroInited?.Invoke(s, e);
            vanillaClient.Attacked += (s, e) => Attacked?.Invoke(s, e);
            vanillaClient.ShipMoving += (s, e) => ShipMoving?.Invoke(s, e);
        }

        public void Stop()
        {
            vanillaClient.Stop();
            vanillaClient.pingThread?.Abort();
            fadeClient.Stop();
            remoteClient.Stop();
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
            remoteClient.OnConnected += (s, e) =>
            {
                Console.WriteLine("Connected to remoteServer");
                ((Client)s).thread.Abort();
                fadeClient.Connect("127.0.0.1", 8081);
            };
            fadeClient.OnConnected += (s, e) =>
            {
                ((Client)s).thread.Abort();
                vanillaClient.Connect(GetIP(), 8080);
            };

            vanillaClient.OnConnected += (o, e) => vanillaClient.Send(new ClientVersionCheck(Config.MAJOR, Config.MINOR, Config.BUILD));
            vanillaClient.Disconnected += (o, e) => Reconnect();

            Connecting?.Invoke(this, EventArgs.Empty);
            
            remoteClient.Connect("127.0.0.1", 8082);
        }

        public void SendEncoded(Command command)
        {
            vanillaClient.SendEncoded(command);
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
            vanillaClient.thread.Abort();
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
