using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace PolskaBot
{
    public partial class Bot : Form
    {

        Thread renderer;

        int FPS = 60;

        const byte alpha = 216;
        static Color mapBG = Color.FromArgb(20, 102, 102, 102);
        static Color hero = Color.FromArgb(alpha, 102, 102, 102);
        static Color box = Color.FromArgb(alpha, 255, 255, 0);
        static Color boxPirate = Color.Green;
        static Color boxMemorised = Color.FromArgb(150, 255, 255, 0);

        public Bot()
        {
            InitializeComponent();
            Load += (s, e) => Init();
            FormClosed += (s, e) => renderer.Abort();
        }

        private void Init()
        {
            AddContextMenu();
            renderer = new Thread(new ThreadStart(Render));
            renderer.Start();
        }

        private void AddContextMenu()
        {
            var contextMenu = new ContextMenu();

            var drawBackground = new MenuItem();
            drawBackground.Text = "Draw background";
            drawBackground.Checked = true;
            drawBackground.Click += (s, e) =>
            {
                Properties.Settings.Default.DrawMap = !Properties.Settings.Default.DrawMap;
                ((MenuItem)s).Checked = Properties.Settings.Default.DrawMap;
            };

            contextMenu.MenuItems.Add(drawBackground);
            minimap.ContextMenu = contextMenu;
        }

        private void Render()
        {
            while(true)
            {
                var bitmap = new Bitmap(minimap.Width, minimap.Height);
                using (var g = Graphics.FromImage(bitmap))
                {
                    DrawBackground(g);
                    DrawPlayer(g, new Point(80, 40));
                    g.DrawRectangle(new Pen(box), new Rectangle(66, 36, 1, 1));
                    g.DrawRectangle(new Pen(boxMemorised), new Rectangle(42, 36, 1, 1));
                    g.DrawRectangle(new Pen(boxPirate), new Rectangle(50, 36, 1, 1));
                }

                Invoke((MethodInvoker)delegate
                {
                    minimap.Image = bitmap;
                });
                Task.Delay(1000 / FPS);
            }
        }

        private void DrawPlayer(Graphics g, Point point)
        {
            g.DrawLine(new Pen(hero), new Point(0, point.X), new Point(minimap.Width, point.X));
            g.DrawLine(new Pen(hero), new Point(point.Y, 0), new Point(point.Y, minimap.Height));
        }

        private void DrawBackground(Graphics g)
        {
            if (!Properties.Settings.Default.DrawMap)
                return;

            g.DrawImage(Properties.Resources._1, 0, 0, 315, 202);
        }
    }
}
