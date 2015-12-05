using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bend.Util;
using System.Net;
using System.Text.RegularExpressions;

namespace PolskaBot.Core
{
    class MapsServer : HttpServer
    {
        public MapsServer(int port) : base(port)
        {

        }

        public override void handleGETRequest(HttpProcessor p)
        {
            Console.WriteLine(p.http_url);
            if (p.http_url.Contains("/spacemap/xml/maps.php"))
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
            Console.WriteLine("Parsing maps");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(p.http_url);
            request.Proxy = new WebProxy();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream responeStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responeStream);

            string line = "";

            p.writeSuccess();

            while(line != null)
            {
                line = reader.ReadLine();
                if(line != null)
                {
                    line = Regex.Replace(line, "<gameserverIP>([0-9.]+)</gameserverIP>", "<gameserverIP>127.0.0.1</gameserverIP>");
                    p.outputStream.Write(line);
                }
            }
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
