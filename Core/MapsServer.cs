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
            if (p.http_url == "/spacemap/xml/maps.php")
                handleMaps(p);
            else if (p.http_url == "/indexInternal.es?action=internalMapRevolution")
                handleIndex(p);
            else
            {
                p.writeFailure();
                p.outputStream.WriteLine("Not supported");
            }
        }

        private void handleMaps(HttpProcessor p)
        {

        }

        private void handleIndex(HttpProcessor p)
        {

        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            throw new NotImplementedException();
        }
    }
}
