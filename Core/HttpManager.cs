using System.Text;
using System.Net;
using System.IO;

namespace PolskaBot.Core
{
    public class HttpManager
    {
        private CookieContainer cookies = new CookieContainer();

        private WebHeaderCollection headers;

        public string Post(string url, string data)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookies;
            request.Method = "POST";
            byte[] postData = new UTF8Encoding().GetBytes(data);
            request.ContentLength = postData.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(postData, 0, postData.Length);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using(StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

        public string Get(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookies;
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using(StreamReader reader  = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

        public void AddHeader(string header, string value)
        {
            headers.Add(header, value);
        }
    }
}
