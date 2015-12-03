using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolskaBot.Core;

namespace Headless
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            API api = new API();
            api.Login("178.132.244.66", "");
        }
    }
}
