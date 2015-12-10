using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit
{
    class Account
    {
        // Website credentials
        public string username { get; private set; }
        public string password { get; private set; }

        // Server credentials
        public int userID { get; private set; }
        public int instanceID { get; private set; }
        public string sid { get; private set; }
        public string serverID { get; private set; }
        public int mapID { get; private set; }

        public Account()
        {

        }

        public void SetCredentials(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
