using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Darkorbit;
using Core.Darkorbit.Commands;

namespace Core
{
    public class API
    {
        Client client = new Client();

        public void Login(string server, string sid)
        {
            client.Connect(server, sid);
            client.Send(new ClientVersionCheck(Config.MAJOR, Config.MINOR, Config.BUILD));
        }
    }
}
