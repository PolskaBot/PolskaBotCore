using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolskaBot.Core;
using System.Threading;

namespace Headless
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-us");
            API api = new API("178.132.244.66", API.Mode.BOT);
        }
    }
}
