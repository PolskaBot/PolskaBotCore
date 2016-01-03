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
using PolskaBot.Fade;

namespace PolskaBot.Core
{
    public class API
    {
        public enum Mode
        {
            BOT
        };

        public Mode mode;

        private VanillaClient _vanillaClient;
        private FadeProxyClient _proxy;
        private RemoteClient _remoteClient;

        private string _ip;

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
        public event EventHandler<EventArgs> AuthFailed;

        public API(string ip, FadeProxyClient proxy, Mode mode = Mode.BOT)
        {
            _ip = ip;

            this.mode = mode;

            // Depedency injection
            Account = new Account(this);
            _proxy = proxy;
            _remoteClient = new RemoteClient(this);
            _vanillaClient = new VanillaClient(this, proxy, _remoteClient);

            Account.LoginSucceed += (s, e) => Connect();

            _vanillaClient.AuthFailed += (s, e) => AuthFailed?.Invoke(s, e);
            _vanillaClient.Disconnected += (s, e) => Disconnected?.Invoke(s, e);
            _vanillaClient.HeroInited += (s, e) => HeroInited?.Invoke(s, e);
            _vanillaClient.Attacked += (s, e) => Attacked?.Invoke(s, e);
            _vanillaClient.ShipMoving += (s, e) => ShipMoving?.Invoke(s, e);
        }

        public void Stop()
        {
            _vanillaClient.Stop();
            _vanillaClient.pingThread?.Abort();
            _remoteClient.Stop();
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
            _remoteClient.OnConnected += (s, e) =>
            {
                Console.WriteLine("Connected to remoteServer");
                ((Client)s).thread.Abort();
                _vanillaClient.Connect(GetIP(), 8080);
            };

            _vanillaClient.OnConnected += (o, e) => _vanillaClient.Send(new ClientVersionCheck(Config.MAJOR, Config.MINOR, Config.BUILD));
            _vanillaClient.Disconnected += (o, e) => Reconnect();

            Connecting?.Invoke(this, EventArgs.Empty);
            _remoteClient.Connect(_ip, 8082);
        }

        public void SendEncoded(Command command)
        {
            _vanillaClient.SendEncoded(command);
        }

        public void Reconnect()
        {
            Console.WriteLine("Connection lost. Reconnecting.");
            _vanillaClient.pingThread.Abort();
            Boxes.Clear();
            MemorizedBoxes.Clear();
            Ores.Clear();
            Ships.Clear();
            Gates.Clear();
            Buildings.Clear();
            _proxy.Reset();
            _remoteClient.Disconnect();
            _vanillaClient.Disconnect();
            _vanillaClient.thread.Abort();
            _remoteClient.Connect(Environment.GetEnvironmentVariable("PB_SERVER_IP"), 8082);
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
