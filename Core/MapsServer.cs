using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bend.Util;

namespace PolskaBot.Core
{
    class MapsServer : HttpServer
    {
        public MapsServer(int port) : base(port)
        {

        }

        public override void handleGETRequest(HttpProcessor p)
        {
            Console.WriteLine("Received GET request");
        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            throw new NotImplementedException();
        }
    }
}
