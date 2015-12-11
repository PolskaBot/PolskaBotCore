using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit
{
    class Config
    {
        public static int MAJOR { get; set; } = 0;
        public static int MINOR { get; set; } = 78;
        public static int BUILD { get; set; } = 4;

        public const string VERSION = "8.3.2";

        public const string USERNAME_ENV = "PB_USERNAME";
        public const string PASSWORD_ENV = "PB_PASSWORD";
    }
}
