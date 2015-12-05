using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bend.Util;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO.Compression;
using PolskaBot.Core.Darkorbit.Parsers;

namespace PolskaBot.Core
{
    class MapsServer : HttpServer
    {
        public API api { get; private set; }

        public MapsServer(API api) : base(9000)
        {
            this.api = api;
        }

        public override void handleGETRequest(HttpProcessor p)
        {
            Console.WriteLine(p.http_url);
            if (p.http_url.Contains("/spacemap/xml/maps.php"))
                handleMaps(p);
            else if (p.http_url.Contains("/indexInternal.es?action=internalMapRevolution"))
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
            p.writeSuccess();

            string content = reader.ReadToEnd();
            MapsParser mapsParser = new MapsParser(content, api.mapCredentials.mapID);
            api.IP = mapsParser.IP;
            p.outputStream.Write(Regex.Replace(content, "<gameserverIP>([0-9.]+)</gameserverIP>", "<gameserverIP>127.0.0.1</gameserverIP>"));
        }

        private void handleIndex(HttpProcessor p)
        {
            Console.WriteLine("Parsing index");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(p.http_url);
            request.Proxy = new WebProxy();
            
            foreach(DictionaryEntry entry in p.httpHeaders)
            {
                string header = (string)entry.Key;

                switch(header)
                {
                    case "Host":
                        request.Host = (string)entry.Value;
                        continue;
                    case "User-Agent":
                        request.UserAgent = (string)entry.Value;
                        continue;
                    case "Accept":
                        request.Accept = (string)entry.Value;
                        continue;
                    case "Connection":
                        request.KeepAlive = true;
                        continue;
                    case "Referer":
                        request.Referer = (string)entry.Value;
                        continue;
                }

                request.Headers[(string)entry.Key] = (string)entry.Value;
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Console.WriteLine("Encoding: {0}", response.ContentEncoding);

            Stream responeStream = response.GetResponseStream();

            StreamReader reader;

            if(response.ContentEncoding == "gzip")
            {
                GZipStream gzipStream = new GZipStream(responeStream, CompressionMode.Decompress);
                reader = new StreamReader(gzipStream);
            } else
            {
                reader = new StreamReader(responeStream);
            }

            p.writeSuccess();
            string output = reader.ReadToEnd();
            IndexParser indexParser = new IndexParser(output);
            api.mapCredentials = indexParser.mapCredentials;
            p.outputStream.Write(output);
        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            throw new NotImplementedException();
        }
    }
}
