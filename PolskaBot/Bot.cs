using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using PolskaBot.Core;
using PolskaBot.Core.Darkorbit;
using PolskaBot.Core.Darkorbit.Commands.PostHandshake;
using Glide;

namespace PolskaBot
{
    public partial class Bot : Form
    {
        API api;

        Thread renderer;

        Tweener anim = new Tweener();

        private Stopwatch stopwatch = new Stopwatch();

        public Bot()
        {
            InitializeComponent();
            Load += (s, e) => Init();
            FormClosed += (s, e) => renderer.Abort();
        }

        private void Init()
        {
            minimap.Click += (s, e) =>
            {
                var mouse = e as MouseEventArgs;
                if (api.account.ready && mouse.Button == MouseButtons.Left)
                {
                    FlyWithAnimation(ReverseScale(mouse.X - 10), ReverseScale(mouse.Y - 10));
                }
            };

            api = new API(API.Mode.BOT);

            api.account.LoginFailed += (s, e) => Log("Login failed");
            api.account.LoginSucceed += (s, e) => Log("Login succeed");

            api.vanillaClient.ShipMoving += (s, e) =>
            {
                lock(api.ships)
                {
                    try
                    {
                        anim.Tween(api.ships.Find(ship => ship.userID == e.player), new { X = e.x, Y = e.y }, e.duration);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            };

            api.Login();
            AddContextMenu();
            renderer = new Thread(new ThreadStart(Render));
            renderer.Start();
        }

        private void AddContextMenu()
        {
            var contextMenu = new ContextMenu();

            var drawBackground = new MenuItem();
            drawBackground.Text = "Draw background";
            drawBackground.Checked = Properties.Settings.Default.DrawMap;
            drawBackground.Click += (s, e) =>
            {
                Properties.Settings.Default.DrawMap = !Properties.Settings.Default.DrawMap;
                ((MenuItem)s).Checked = Properties.Settings.Default.DrawMap;
            };

            contextMenu.MenuItems.Add(drawBackground);

            var drawOres = new MenuItem();
            drawOres.Text = "Draw ores";
            drawOres.Checked = Properties.Settings.Default.DrawOres;
            drawOres.Click += (s, e) =>
            {
                Properties.Settings.Default.DrawOres = !Properties.Settings.Default.DrawOres;
                ((MenuItem)s).Checked = Properties.Settings.Default.DrawOres;
            };

            contextMenu.MenuItems.Add(drawOres);
            minimap.ContextMenu = contextMenu;
        }

        private void Render()
        {
            while(true)
            {
                stopwatch.Restart();
                Box[] boxes;
                Ore[] ores;
                Ship[] ships;
                Gate[] gates;
                Building[] buildings;

                lock(api.boxes)
                {
                    boxes = api.boxes.ToArray();
                }

                lock(api.ores)
                {
                    ores = api.ores.ToArray();
                }

                lock(api.ships)
                {
                    ships = api.ships.ToArray();
                }

                lock(api.gates)
                {
                    gates = api.gates.ToArray();
                }

                lock(api.buildings)
                {
                    buildings = api.buildings.ToArray();
                }

                var bitmap = new Bitmap(minimap.Width, minimap.Height);
                using (var g = Graphics.FromImage(bitmap))
                {
                    DrawBackground(g);
                    DrawBorders(g);

                    foreach(Box box in boxes)
                    {
                        DrawBox(g, box);
                    }

                    foreach(Ore ore in ores)
                    {
                        DrawOre(g, ore);
                    }

                    foreach(Ship ship in ships)
                    {
                        DrawShip(g, ship);
                    }

                    foreach(Gate gate in gates)
                    {
                        DrawGate(g, gate);
                    }

                    foreach(Building building in buildings)
                    {
                        DrawBuilding(g, building);
                    }

                    DrawPlayer(g);
                    DrawDetails(g);
                }

                Invoke((MethodInvoker)delegate
                {
                    minimap.Image = bitmap;
                });
                Thread.Sleep(1000 / Config.FPS);
                stopwatch.Stop();
                anim.Update(stopwatch.ElapsedMilliseconds);
            }
        }

        private void DrawBox(Graphics g, Box box)
        {
            g.DrawRectangle(new Pen(Config.box), new Rectangle(Scale(box.pos.X), Scale(box.pos.Y), 1, 1));
        }

        private void DrawOre(Graphics g, Ore ore)
        {
            if (!Properties.Settings.Default.DrawOres)
                return;
            switch(ore.type)
            {
                case Ore.Type.PROMETIUM:
                    g.DrawRectangle(new Pen(Config.prometium), new Rectangle(Scale(ore.pos.X), Scale(ore.pos.Y), 1, 1));
                    break;
                case Ore.Type.ENDURIUM:
                    g.DrawRectangle(new Pen(Config.endurium), new Rectangle(Scale(ore.pos.X), Scale(ore.pos.Y), 1, 1));
                    break;
                case Ore.Type.TERBIUM:
                    g.DrawRectangle(new Pen(Config.terbium), new Rectangle(Scale(ore.pos.X), Scale(ore.pos.Y), 1, 1));
                    break;
                case Ore.Type.PALLADIUM:
                    g.DrawRectangle(new Pen(Config.palladium), new Rectangle(Scale(ore.pos.X), Scale(ore.pos.Y), 1, 1));
                    break;
            }
        }

        private void DrawShip(Graphics g, Ship ship)
        {
            if(ship.npc)
            {
                g.DrawRectangle(new Pen(Config.npc), new Rectangle(Scale(ship.X), Scale(ship.Y), 1, 1));
                return;
            }

            if(ship.factionID != api.account.factionID)
            {
                g.DrawRectangle(new Pen(Config.enemy), new Rectangle(Scale(ship.X), Scale(ship.Y), 1, 1));
                return;
            }

            g.DrawRectangle(new Pen(Config.friend), new Rectangle(Scale(ship.X), Scale(ship.Y), 1, 1));
        }

        private void DrawGate(Graphics g, Gate gate)
        {
            g.DrawEllipse(new Pen(Config.gate), Scale(gate.pos.X) - 5, Scale(gate.pos.Y) - 5, 10, 10);
        }

        private void DrawBuilding(Graphics g, Building building)
        {
            if (building.Name.StartsWith("StationTurret") || building.Name.Equals("HQ") || building.Name.Equals("Healing Pod"))
                return;

            g.DrawEllipse(new Pen(Config.building), Scale(building.Position.X) - 7, Scale(building.Position.Y) - 7, 14, 14);
        }

        private void DrawPlayer(Graphics g)
        {
            if(api.account.ready)
            {
                g.DrawLine(new Pen(Config.hero), new Point(Scale(api.account.X), 0), new Point(Scale(api.account.X), minimap.Height));
                g.DrawLine(new Pen(Config.hero), new Point(0, Scale(api.account.Y)), new Point(minimap.Width, Scale(api.account.Y)));

                if(api.account.isFlying)
                {
                    g.DrawLine(new Pen(Config.hero), new Point(Scale(api.account.X), Scale(api.account.Y)), new Point(Scale(api.account.targetX), Scale(api.account.targetY)));
                }
            }
        }

        private void DrawDetails(Graphics g)
        {
            if (api.account.ready)
            {
                string hpDetails = $"{api.account.HP}/{api.account.maxHP}";
                string shieldDetails = $"{api.account.shield}/{api.account.maxShield}";
                SizeF sizeHP = g.MeasureString(hpDetails, Config.font);
                SizeF sizeShield = g.MeasureString(shieldDetails, Config.font);
                g.DrawString(hpDetails, Config.font, new SolidBrush(Config.hitpoints), minimap.Width - sizeHP.Width - 4 - Config.poizoneSize,
                    4 + Config.poizoneSize);
                g.DrawString(shieldDetails, Config.font, new SolidBrush(Config.shield), minimap.Width - sizeHP.Width - sizeShield.Width - 4 - Config.poizoneSize,
                    4 + Config.poizoneSize);
            }
        }

        private void DrawBorders(Graphics g)
        {
            g.DrawRectangle(new Pen(Config.neutral), new Rectangle(Config.poizoneSize, Config.poizoneSize, minimap.Width - 2 * Config.poizoneSize,
                minimap.Height - 2 * Config.poizoneSize));
        }

        private void DrawBackground(Graphics g)
        {
            if (!Properties.Settings.Default.DrawMap)
                return;

            g.DrawImage(Properties.Resources._1, 0, 0, 315, 202);
        }

        private void FlyWithAnimation(int x, int y)
        {
            api.account.targetX = x;
            api.account.targetY = y;
            api.vanillaClient.SendEncoded(new Move((uint)api.account.targetX, (uint)api.account.targetY, (uint)api.account.X, (uint)api.account.Y));

            api.account.isFlying = true;

            double distance = Math.Sqrt(Math.Pow((api.account.targetX - api.account.X), 2) + Math.Pow((api.account.targetY - api.account.Y), 2));

            double duration = (distance / api.account.speed);

            float durationMS = (float)duration * 1000;
            try
            {
                anim.TargetCancel(api.account);
                anim.Tween(api.account, new { X = api.account.targetX, Y = api.account.targetY }, durationMS).OnComplete(
                    new Action(() => api.account.isFlying = false
                    ));
            } catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private int Scale(int value)
        {
            return (int)(value * Config.k) + Config.poizoneSize;
        }

        private int ReverseScale(int value)
        {
            return (int)(value / Config.k);
        }


        #region Controls
        private void Log(string text)
        {
            log.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}] {text}\n");
        }
        #endregion
    }
}
