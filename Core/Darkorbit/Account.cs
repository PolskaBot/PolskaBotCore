using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;

namespace PolskaBot.Core.Darkorbit
{
    public class Account
    {
        public API api;

        // Website credentials
        public string Username { get; private set; }
        public string Password { get; private set; }

        // Server credentials
        public int UserID { get; private set; }
        public int InstanceID { get; private set; }
        public string SID { get; private set; }
        public string Server { get; private set; }
        public int Map { get; private set; }

        // Movement
        public int X { get; set; }
        public int Y { get; set; }
        public int TargetX { get; set; }
        public int TargetY { get; set; }
        public bool Flying { get; set; }

        // Map statistics
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int Shield { get; set; }
        public int MaxShield { get; set; }
        public int NanoHP { get; set; }
        public int MaxNanoHP { get; set; }
        public int FreeCargoSpace { get; set; }
        public int CargoCapacity { get; set; }

        // Ship
        public string Shipname { get; set; }
        public int Speed { get; set; }
        public int Config { get; set; }

        // Statistics
        public bool Cloaked { get; set; }
        public float Jackpot { get; set; }
        public bool Premium { get; set; }
        public double Credits { get; set; }
        public double Honor { get; set; }
        public double Uridium { get; set; }
        public double XP { get; set; }
        public int Level { get; set; }
        public int Rank { get; set; }

        // Social
        public int ClanID { get; set; }
        public string ClanTag { get; set; }
        public uint FactionID { get; set; }

        // Collected
        public double CollectedUridium { get; set; }
        public double CollectedCredits { get; set; }
        public double CollectedXP { get; set; }
        public double CollectedHonor { get; set; }
        public int CollectedEE { get; set; }

        public bool Ready { get; set; } = false;

        public bool JumpAllowed { get; set; }

        HttpManager httpManager;

        public event EventHandler<EventArgs> LoginFailed;
        public event EventHandler<EventArgs> LoginSucceed;

        public Account(API api)
        {
            this.api = api;
            httpManager = new HttpManager();
            httpManager.AddHeader("Upgrade-Insecure-Requests", "1");
            httpManager.AddHeader("Accept-Encoding", "gzip, deflate");
            httpManager.AddHeader("Accept-Language", "en-US;q=0.6,en;q=0.4");
            httpManager.userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.86 Safari/537.36";
        }

        public void SetCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public void UpdateHitpoints(int hp, int maxHP, int nanoHP, int maxNanoHP)
        {
            HP = hp;
            MaxHP = maxHP;
            NanoHP = nanoHP;
            MaxNanoHP = maxNanoHP;
        }

        public void UpdateShield(int shield, int maxShield)
        {
            Shield = shield;
            MaxShield = maxShield;
        }

        public void UpdateHitpointsAndShield(int hp, int shield, int nanoHP)
        {
            HP = hp;
            Shield = shield;
            NanoHP = nanoHP;
        }

        public void Login()
        {
            string homepageResponse = httpManager.Get("http://www.darkorbit.com/");
            Match match = Regex.Match(homepageResponse, "class=\"bgcdw_login_form\" action=\"(.*)\">");

            if(!match.Success)
            {
                LoginFailed?.Invoke(this, EventArgs.Empty);
                return;
            }

            string loginResponse = httpManager.Post(WebUtility.HtmlDecode(match.Groups[1].ToString()), $"username={Username}&password={Password}");
            match = Regex.Match(loginResponse, "http://(.*).darkorbit.bigpoint.com");

            if (!match.Success)
            {
                LoginFailed?.Invoke(this, EventArgs.Empty);
                return;
            }

            Server = match.Groups[1].ToString();

            string mapResponse = httpManager.Get($"http://{Server}.darkorbit.bigpoint.com/indexInternal.es?action=internalMapRevolution");
            match = Regex.Match(mapResponse, "{\"pid\":([0-9]+),\"uid\":([0-9]+)[\\w,\":]+sid\":\"([0-9a-z]+)\"");

            if (!match.Success)
            {
                LoginFailed?.Invoke(this, EventArgs.Empty);
                return;
            }

            InstanceID = int.Parse(match.Groups[1].ToString());
            UserID = int.Parse(match.Groups[2].ToString());
            SID = match.Groups[3].ToString();
            match = Regex.Match(mapResponse, "mapID\": \"([0-9]*)\"");
            Map = int.Parse(match.Groups[1].ToString());

            LoginSucceed?.Invoke(this, EventArgs.Empty);
        }
    }
}
