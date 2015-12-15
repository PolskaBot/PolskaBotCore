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
        public string username { get; private set; }
        public string password { get; private set; }

        // Server credentials
        public int userID { get; private set; }
        public int instanceID { get; private set; }
        public string sid { get; private set; }
        public string serverID { get; private set; }
        public int mapID { get; private set; }

        // Movement
        public int X { get; set; }
        public int Y { get; set; }
        public int targetX { get; set; }
        public int targetY { get; set; }
        public bool isFlying { get; set; }

        // Map statistics
        public int HP { get; set; }
        public int maxHP { get; set; }
        public int shield { get; set; }
        public int maxShield { get; set; }
        public int nanoHP { get; set; }
        public int maxNanoHP { get; set; }
        public int freeCargoSpace { get; set; }
        public int cargoCapacity { get; set; }

        // Ship
        public string shipName { get; set; }
        public int speed { get; set; }

        // Statistics
        public bool cloaked { get; set; }
        public float jackpot { get; set; }
        public bool premium { get; set; }
        public double credits { get; set; }
        public double honor { get; set; }
        public double uridium { get; set; }
        public double XP { get; set; }
        public int level { get; set; }
        public int rank { get; set; }

        // Social
        public int clanID { get; set; }
        public string clanTag { get; set; }
        public uint factionID { get; set; }

        public bool ready { get; set; } = false;

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
            this.username = username;
            this.password = password;
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

            string loginResponse = httpManager.Post(WebUtility.HtmlDecode(match.Groups[1].ToString()), $"username={username}&password={password}");
            match = Regex.Match(loginResponse, "http://(.*).darkorbit.bigpoint.com");

            if (!match.Success)
            {
                LoginFailed?.Invoke(this, EventArgs.Empty);
                return;
            }

            serverID = match.Groups[1].ToString();

            string mapResponse = httpManager.Get($"http://{serverID}.darkorbit.bigpoint.com/indexInternal.es?action=internalMapRevolution");
            match = Regex.Match(mapResponse, "{\"pid\":([0-9]+),\"uid\":([0-9]+)[\\w,\":]+sid\":\"([0-9a-z]+)\"");

            if (!match.Success)
            {
                LoginFailed?.Invoke(this, EventArgs.Empty);
                return;
            }

            instanceID = int.Parse(match.Groups[1].ToString());
            userID = int.Parse(match.Groups[2].ToString());
            sid = match.Groups[3].ToString();
            match = Regex.Match(mapResponse, "mapID\": \"([0-9]*)\"");
            mapID = int.Parse(match.Groups[1].ToString());

            LoginSucceed?.Invoke(this, EventArgs.Empty);
        }
    }
}
