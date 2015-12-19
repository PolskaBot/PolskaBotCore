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
using MiscUtil.IO;

namespace PolskaBot
{
    public enum State
    {
        SearchingBox, CollectingBox
    }

    public partial class Bot : Form
    {
        API api;

        Thread renderer;
        Thread logic;

        private State state = State.SearchingBox;

        private bool running = false;
        Random random = new Random();

        Tweener anim = new Tweener();

        private string[] collectable = {
                "BONUS_BOX", "GIFT_BOXES", "EVENT_BOX"
        };

        private Stopwatch stopwatch = new Stopwatch();

        public Bot()
        {
            InitializeComponent();
            Load += (s, e) => Init();
            FormClosed += (s, e) => renderer.Abort();
        }

        private void Init()
        {
            startButton.Click += (s, e) =>
            {
                startButton.Enabled = false;
                stopButton.Enabled = true;
                running = true;
            };

            stopButton.Click += (s, e) =>
            {
                startButton.Enabled = true;
                stopButton.Enabled = false;
                running = false;
            };

            changeConfigButton.Click += (s, e) =>
            {
                ChangeConfig();
            };

            minimap.Click += (s, e) =>
            {
                var mouse = e as MouseEventArgs;
                if (api.Account.Ready && mouse.Button == MouseButtons.Left)
                {
                    FlyWithAnimation(ReverseScale(mouse.X - 10), ReverseScale(mouse.Y - 10));
                }
            };

            api = new API(API.Mode.BOT);

            api.Account.LoginFailed += (s, e) => Log("Login failed");
            api.Account.LoginSucceed += (s, e) => Log("Login succeed");

            api.vanillaClient.Compatible += (s, e) => Log("Bot is compatible");
            api.vanillaClient.NotCompatible += (s, e) => Log("Bot is not compatible. Check forums for new version");

            api.vanillaClient.Attacked += (s, e) =>
            {
                lock (api.Ships)
                {
                    var attacker = api.Ships.Find(ship => ship.UserID == e.AttackerID);
                    if (attacker != null && !attacker.NPC && e.UserID == api.Account.UserID)
                    {
                        var targetGate = api.Gates.OrderBy(gate => Math.Sqrt(Math.Pow(gate.Position.X - api.Account.X, 2) + Math.Pow(gate.Position.Y - api.Account.Y, 2))).Where(gate => gate.ID == 1).First();
                        FlyWithAnimation(targetGate.Position.X, targetGate.Position.Y);
                    }
                }
            };

            api.vanillaClient.ShipMoving += (s, e) =>
            {
                lock (api.Ships)
                {
                    try
                    {
                        var target = api.Ships.Find(ship => ship.UserID == e.UserID);
                        if (target != null)
                            anim.Tween(api.Ships.Find(ship => ship.UserID == e.UserID), new { X = e.X, Y = e.Y }, e.Duration);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            };

            api.vanillaClient.LogMessage += (s, e) => Log(e);

            api.Login();
            AddContextMenu();
            renderer = new Thread(new ThreadStart(Render));
            renderer.Start();

            logic = new Thread(new ThreadStart(Runner));
            logic.Start();
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

        private void Runner()
        {
            while(true)
            {
                if (!api.Account.Ready || !running)
                {
                    Thread.Sleep(50);
                    continue;
                }

                Box[] boxes;
                Box[] memorizedBoxes;

                lock(api.Boxes)
                {
                    boxes = api.Boxes.ToArray();
                }

                lock(api.MemorizedBoxes)
                {
                    memorizedBoxes = api.MemorizedBoxes.ToArray();
                }

                if(state == State.SearchingBox && running)
                {
                    var nearestBox = boxes.Where(box => collectable.Contains(box.Type)).OrderBy(box => CalculateDistance(box.Position)).FirstOrDefault();
                    if (nearestBox == null)
                    {
                        var memorizedNearestBox = memorizedBoxes.Where(box => collectable.Contains(box.Type)).Where(box => !boxes.Contains(box)).OrderBy(box => CalculateDistance(box.Position)).FirstOrDefault();
                        if (memorizedNearestBox == null)
                        {
                            if (api.Account.Flying)
                                Thread.Sleep(50);
                            else
                                FlyWithAnimation(random.Next(0, 21000), random.Next(0, 13500));
                        }
                        else
                        {
                            FlyWithAnimation(memorizedNearestBox.Position.X, memorizedNearestBox.Position.Y);
                            state = State.CollectingBox;
                        }
                    }
                    else
                    {
                        FlyWithAnimation(nearestBox.Position.X, nearestBox.Position.Y);
                        state = State.CollectingBox;
                        Thread.Sleep(50);
                        continue;
                    }
                }

                if(state == State.CollectingBox && running)
                {
                    if (api.Account.Flying)
                    {
                        var flybyBox = memorizedBoxes.Where(box => collectable.Contains(box.Type)).OrderBy(box => CalculateDistance(box.Position)).FirstOrDefault();
                        if (CalculateDistance(flybyBox.Position) < CalculateDistance(api.Account.TargetX, api.Account.TargetY))
                            FlyWithAnimation(flybyBox.Position.X, flybyBox.Position.Y);
                        Thread.Sleep(50);
                        continue;
                    }

                    var nearestBox = boxes.Where(box => collectable.Contains(box.Type)).OrderBy(box => CalculateDistance(box.Position)).FirstOrDefault();
                    if (nearestBox == null)
                        state = State.SearchingBox;
                    else
                    {
                        if (CalculateDistance(nearestBox.Position) < 50)
                        {
                            var tempShipY = api.Account.Y;
                            if((api.Account.X + api.Account.Y + nearestBox.Position.Y) % 3 == 0)
                            {
                                tempShipY++;
                            }
                            api.vanillaClient.SendEncoded(new CollectBox(nearestBox.Hash, nearestBox.Position.X, nearestBox.Position.Y, api.Account.X, tempShipY));
                            lock(api.Boxes) lock (api.MemorizedBoxes)
                            {
                                api.Boxes.RemoveAll(box => box.Hash == nearestBox.Hash);
                                api.MemorizedBoxes.RemoveAll(box => box.Hash == nearestBox.Hash);
                            }
                        }
                        state = State.SearchingBox;
                    }

                    Thread.Sleep(50);
                }
            }
        }

        private void Render()
        {
            while(true)
            {
                stopwatch.Restart();
                Box[] boxes;
                Box[] memorizedBoxes;
                Ore[] ores;
                Ship[] ships;
                Gate[] gates;
                Building[] buildings;

                lock(api.Boxes)
                {
                    boxes = api.Boxes.ToArray();
                }

                lock(api.MemorizedBoxes)
                {
                    memorizedBoxes = api.MemorizedBoxes.Where(box => !boxes.Contains(box)).ToArray();
                }

                lock(api.Ores)
                {
                    ores = api.Ores.ToArray();
                }

                lock(api.Ships)
                {
                    ships = api.Ships.ToArray();
                }

                lock(api.Gates)
                {
                    gates = api.Gates.ToArray();
                }

                lock(api.Buildings)
                {
                    buildings = api.Buildings.ToArray();
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

                    foreach(Box box in memorizedBoxes)
                    {
                        DrawMemorizedBox(g, box);
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
                    UpdateProgressBars();
                    DrawDetails(g);
                }

                Invoke((MethodInvoker)delegate
                {
                    minimap.Image = bitmap;
                });
                Thread.Sleep(1000 / Config.FPS);
                stopwatch.Stop();
                try
                {
                    anim.Update(stopwatch.ElapsedMilliseconds);
                } catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private double CalculateDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        private double CalculateDistance(int x, int y)
        {
            return CalculateDistance(api.Account.X, api.Account.Y, x, y);
        }

        private double CalculateDistance(Point point)
        {
            return CalculateDistance(point.X, point.Y);
        }

        #region MapDrawing

        private void DrawBox(Graphics g, Box box)
        {
            g.DrawRectangle(new Pen(Config.box), new Rectangle(Scale(box.Position.X), Scale(box.Position.Y), 1, 1));
        }

        private void DrawMemorizedBox(Graphics g, Box box)
        {
            g.DrawRectangle(new Pen(Config.boxMemorised), new Rectangle(Scale(box.Position.X), Scale(box.Position.Y), 1, 1));
        }

        private void DrawOre(Graphics g, Ore ore)
        {
            if (!Properties.Settings.Default.DrawOres)
                return;
            switch(ore.Type)
            {
                case Ore.OreType.PROMETIUM:
                    g.DrawRectangle(new Pen(Config.prometium), new Rectangle(Scale(ore.Position.X), Scale(ore.Position.Y), 1, 1));
                    break;
                case Ore.OreType.ENDURIUM:
                    g.DrawRectangle(new Pen(Config.endurium), new Rectangle(Scale(ore.Position.X), Scale(ore.Position.Y), 1, 1));
                    break;
                case Ore.OreType.TERBIUM:
                    g.DrawRectangle(new Pen(Config.terbium), new Rectangle(Scale(ore.Position.X), Scale(ore.Position.Y), 1, 1));
                    break;
                case Ore.OreType.PALLADIUM:
                    g.DrawRectangle(new Pen(Config.palladium), new Rectangle(Scale(ore.Position.X), Scale(ore.Position.Y), 1, 1));
                    break;
            }
        }

        private void DrawShip(Graphics g, Ship ship)
        {
            if(ship.NPC)
            {
                g.DrawRectangle(new Pen(Config.npc), new Rectangle(Scale(ship.X), Scale(ship.Y), 1, 1));
                return;
            }

            if(ship.FactionID != api.Account.FactionID)
            {
                g.DrawRectangle(new Pen(Config.enemy), new Rectangle(Scale(ship.X), Scale(ship.Y), 1, 1));
                return;
            }

            g.DrawRectangle(new Pen(Config.friend), new Rectangle(Scale(ship.X), Scale(ship.Y), 1, 1));
        }

        private void DrawGate(Graphics g, Gate gate)
        {
            g.DrawEllipse(new Pen(Config.gate), Scale(gate.Position.X) - 5, Scale(gate.Position.Y) - 5, 10, 10);
        }

        private void DrawBuilding(Graphics g, Building building)
        {
            if (building.Name.StartsWith("StationTurret") || building.Name.Equals("HQ") || building.Name.Equals("Healing Pod"))
                return;

            g.DrawEllipse(new Pen(Config.building), Scale(building.Position.X) - 7, Scale(building.Position.Y) - 7, 14, 14);
        }

        private void DrawPlayer(Graphics g)
        {
            if(api.Account.Ready)
            {
                g.DrawLine(new Pen(Config.hero), new Point(Scale(api.Account.X), 0), new Point(Scale(api.Account.X), minimap.Height));
                g.DrawLine(new Pen(Config.hero), new Point(0, Scale(api.Account.Y)), new Point(minimap.Width, Scale(api.Account.Y)));

                if(api.Account.Flying)
                {
                    g.DrawLine(new Pen(Config.hero), new Point(Scale(api.Account.X), Scale(api.Account.Y)), new Point(Scale(api.Account.TargetX), Scale(api.Account.TargetY)));
                }
            }
        }

        private void DrawDetails(Graphics g)
        {
            if (api.Account.Ready)
            {
                if(api.Account.Cloaked)
                {
                    string cloaked = "Invisible";
                    g.DrawString(cloaked, Config.font, new SolidBrush(Color.DarkGray), Config.poizoneSize + 4, minimap.Height - 16 - Config.poizoneSize);
                }
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
            api.Account.TargetX = x;
            api.Account.TargetY = y;
            api.vanillaClient.SendEncoded(new Move((uint)api.Account.TargetX, (uint)api.Account.TargetY, (uint)api.Account.X, (uint)api.Account.Y));

            api.Account.Flying = true;

            double distance = Math.Sqrt(Math.Pow((api.Account.TargetX - api.Account.X), 2) + Math.Pow((api.Account.TargetY - api.Account.Y), 2));

            double duration = (distance / api.Account.Speed);

            float durationMS = (float)duration * 1000;
            try
            {
                anim.TargetCancelAndComplete(api.Account);
                anim.Tween(api.Account, new { X = api.Account.TargetX, Y = api.Account.TargetY }, durationMS).OnComplete(
                    new Action(() => api.Account.Flying = false
                    ));
            } catch(Exception ex)
            {
                api.Account.Flying = false;
                Console.WriteLine(ex);
            }
        }

        private void ChangeConfig()
        {
            int targetConfig = (api.Account.Config == 1) ? 2 : 1;
            api.vanillaClient.SendEncoded(new OldStylePacket($"S|CFG|{targetConfig}|{api.Account.UserID}|{api.Account.SID}"));
        }

        private int Scale(int value)
        {
            return (int)(value * Config.k) + Config.poizoneSize;
        }

        private int ReverseScale(int value)
        {
            return (int)(value / Config.k);
        }

        #endregion

        #region Controls

        private void UpdateStats()
        {
            Invoke((MethodInvoker)delegate
            {
                statsView.Nodes.Find("UridiumNode", true).First().Text = $"Uridium: {api.Account.CollectedUridium}";
                statsView.Nodes.Find("CreditsNode", true).First().Text = $"Credits: {api.Account.CollectedCredits}";
                statsView.Nodes.Find("XPNode", true).First().Text = $"XP: {api.Account.CollectedXP}";
                statsView.Nodes.Find("HonorNode", true).First().Text = $"Honor: {api.Account.CollectedHonor}";
                statsView.Nodes.Find("EENode", true).First().Text = $"Extra energy: {api.Account.CollectedEE}";
            });
        }

        private void UpdateProgressBars()
        {
            Invoke((MethodInvoker)delegate
            {
                hpProgressBar.UpdateStats(api.Account.HP, api.Account.MaxHP);
                shieldProgressBar.UpdateStats(api.Account.Shield, api.Account.MaxShield);
                cargoProgressBar.UpdateStats(api.Account.CargoCapacity - api.Account.FreeCargoSpace, api.Account.CargoCapacity);
            });
        }

        private void Log(string text)
        {
            Invoke((MethodInvoker)delegate
            {
                log?.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}] {text}\n");
                UpdateStats();
            });
        }
        #endregion
    }
}
