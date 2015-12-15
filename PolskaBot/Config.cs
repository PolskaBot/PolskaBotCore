using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PolskaBot
{
    class Config
    {
        public const int FPS = 60;
        public const float k = 0.015f;
        public const int poizoneSize = 10;
        const byte alpha = 216;

        public static Color neutral = Color.FromArgb(alpha, 102, 102, 102);

        public static Color mapBG = Color.FromArgb(20, 102, 102, 102);
        public static Color hero = Color.FromArgb(alpha, 102, 102, 102);
        public static Color box = Color.FromArgb(alpha, 255, 255, 0);
        public static Color boxPirate = Color.Green;
        public static Color boxMemorised = Color.FromArgb(150, 255, 255, 0);

        public static Color prometium = Color.FromArgb(179, 47, 47);
        public static Color endurium = Color.FromArgb(89, 170, 227);
        public static Color terbium = Color.FromArgb(224, 229, 63);
        public static Color palladium = Color.FromArgb(158, 28, 195);

        public static Color hitpoints = Color.FromArgb(0, 204, 51);
        public static Color shield = Color.FromArgb(51, 143, 204);

        // Ships
        public static Color friend = Color.FromArgb(0, 153, 255);
        public static Color npc = Color.Red;
        public static Color enemy = Color.FromArgb(alpha, 255, 152, 0);

        // Buildings
        public static Color gate = Color.FromArgb(alpha, 255, 255, 255);
        public static Color building = Color.FromArgb(alpha, 203, 46, 255);

        public static Font font = new Font("Arial", 8, FontStyle.Regular);

    }
}
