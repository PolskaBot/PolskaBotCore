using System;
using System.Net;
using System.IO;


namespace PolskaBot.Core
{
    public class HttpManager
    {
        private CookieContainer cookies = new CookieContainer();

        private WebHeaderCollection headers = new WebHeaderCollection();

        private string lastURL = "https://www.google.pl/?gfe_rd=cr&ei=p9JpVuDNAeOv8wfZ85XgCA";

        public string userAgent { get; set; }

        public string Post(string url, string data)
        {
            Console.WriteLine(url);
            Console.WriteLine(data);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.None;
            request.CookieContainer = cookies;
            request.UserAgent = userAgent;
            request.Headers = headers;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Referer = lastURL;

            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(data);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            lastURL = response.ResponseUri.ToString();
            using(StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

        public string Get(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.None;
            request.CookieContainer = cookies;
            request.UserAgent = userAgent;
            request.Headers = headers;
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            lastURL = response.ResponseUri.ToString();
            using (StreamReader reader  = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

        public void AddHeader(string header, string value)
        {
            if(header == "User-Agent")
            {
                userAgent = value;
                return;
            }
            headers.Add(header, value);
        }
    }
}
