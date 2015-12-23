using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
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
        SearchingBox, CollectingBox, Escaping, EsapingJumped, Reparing
    }

    public class BotPage : TabPage
    {

        API api;

        public BotSettings Settings { get; private set; } = new BotSettings();

        Thread renderer;
        Thread logic;

        private State state = State.SearchingBox;

        public bool Running { get; set; }
        Random random = new Random();

        Tweener anim = new Tweener();

        Box boxToCollect;
        Gate gateToJump;

        private Stopwatch stopwatch = new Stopwatch();

        private ColorProgressBar hpBar;
        private ColorProgressBar shieldBar;
        private ColorProgressBar cargoBar;
        private PictureBox minimap;

        private string[] displayableBoxes = {
            "BONUS_BOX", "GIFT_BOXES", "EVENT_BOX", "PIRATE_BOOTY", "PIRATE_BOOTY_GOLD", "PIRATE_BOOTY_RED", "PIRATE_BOOTY_BLUE"
            };

        public BotPage(string username, string password)
        {
            Text = "Loading";
            BackColor = Color.White;
            InitializeComponent();
            DrawText("Loading");
            api = new API();
            AddHandlers();
            AddContextMenu();

            Task loginTask = new Task(() =>
            {
                api.Login(username, password);
            });
            loginTask.Start();
        }

        public void Stop()
        {
            Running = false;
            renderer?.Abort();
            logic?.Abort();
            api?.Stop();
        }

        #region Setup

        private void AddHandlers()
        {
            api.Account.LoginSucceed += (s, e) => DrawText("Login succeed");
            api.Account.LoginFailed += (s, e) => DrawText("Login failed");
            api.vanillaClient.HeroInited += (s, e) =>
            {
                // Change tab text
                Invoke((MethodInvoker)delegate
                {
                    Text = $"{api.Account.Username}";
                });

                renderer = new Thread(new ThreadStart(Render));
                renderer.Start();

                logic = new Thread(new ThreadStart(Logic));
                logic.Start();

                FlyWithAnimation(api.Account.X, api.Account.Y); // Stop flying.
            };

            api.vanillaClient.Attacked += (s, e) =>
            {
                lock (api.Ships)
                {
                    var attacker = api.Ships.Find(ship => ship.UserID == e.AttackerID);
                    if (attacker != null && !attacker.NPC && e.UserID == api.Account.UserID)
                    {
                        var targetGate = api.Gates.OrderBy(gate => Math.Sqrt(Math.Pow(gate.Position.X - api.Account.X, 2) + Math.Pow(gate.Position.Y - api.Account.Y, 2))).Where(gate => gate.ID == 1).First();
                        gateToJump = targetGate;
                        state = State.Escaping;
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

            api.vanillaClient.Disconnected += (s, e) =>
            {
                anim.Cancel();
                api.Account.Ready = false;
                api.Account.Flying = false;
                if(renderer != null)
                    renderer.Abort();
                if(logic != null)
                    logic.Abort();
                DrawText("Reconnecting");
            };

            minimap.Click += (s, e) =>
            {
                var mouse = e as MouseEventArgs;
                if (api.Account.Ready && mouse.Button == MouseButtons.Left)
                {
                    FlyWithAnimation(ReverseScale(mouse.X - 10), ReverseScale(mouse.Y - 10));
                }
            };
        }

        private void AddContextMenu()
        {
            var contextMenu = new ContextMenu();

            var drawOres = new MenuItem();
            drawOres.Text = "Draw ores";
            drawOres.Checked = Properties.Settings.Default.DrawOres;
            drawOres.Click += (s, e) =>
            {
                Properties.Settings.Default.DrawOres = !Properties.Settings.Default.DrawOres;
                ((MenuItem)s).Checked = Properties.Settings.Default.DrawOres;
            };
            contextMenu.MenuItems.Add(drawOres);

            contextMenu.MenuItems.Add("-");

            var jump = new MenuItem();
            jump.Text = "Jump";
            jump.Click += (s, e) => Jump();
            contextMenu.MenuItems.Add(jump);

            minimap.ContextMenu = contextMenu;
        }

        #endregion

        #region Logic

        private void Logic()
        {
            while (true)
            {

                if (state == State.Escaping)
                {
                    if (CalculateDistance(gateToJump.Position) < 300)
                    {
                        Jump();
                        state = State.EsapingJumped;
                        continue;
                    }
                    else
                    {
                        Thread.Sleep(50);
                        continue;
                    }
                }

                if(state == State.EsapingJumped && api.Account.Ready)
                {
                    Thread.Sleep(5000);
                    Jump();
                    state = State.SearchingBox;
                }

                if (!api.Account.Ready || !Running)
                {
                    Thread.Sleep(500);
                    continue;
                }

                // Check if there is any task to do.
                if (!Settings.CollectorEnabled)
                {
                    Thread.Sleep(500);
                    continue;
                }

                if (Settings.CollectorEnabled)
                {
                    if (!Settings.CollectBonusBoxes && !Settings.CollectEventBoxes)
                    {
                        Thread.Sleep(500);
                        continue;
                    }
                }

                List<string> collectable;
                List<Box> boxes;
                List<Box> memorizedBoxes;

                lock(Settings.CollectableBoxes)
                {
                    collectable = Settings.CollectableBoxes.ToList();
                }

                lock (api.Boxes)
                {
                    boxes = api.Boxes.ToList().Where(box => collectable.Contains(box.Type)).ToList();
                }

                lock (api.MemorizedBoxes)
                {
                    memorizedBoxes = api.MemorizedBoxes.ToList().Where(box => collectable.Contains(box.Type)).ToList();
                }

                if (state == State.SearchingBox && Running)
                {
                    var nearestBox = boxes.OrderBy(box => CalculateDistance(box.Position)).FirstOrDefault();
                    if (nearestBox == null)
                    {
                        // There are no real boxes nearby.
                        var memorizedNearestBox = memorizedBoxes.Where(box => !boxes.Contains(box)).OrderBy(box => CalculateDistance(box.Position)).FirstOrDefault();
                        if (memorizedNearestBox == null)
                        {
                            // And no memorized ones. If flying continue. flying. If not fly to random position.
                            if (!api.Account.Flying)
                                FlyWithAnimation(random.Next(0, 21000), random.Next(0, 13500));
                        }
                        else
                        {
                            // Fly to memorized box.
                            boxToCollect = memorizedNearestBox;
                            Fly(boxToCollect);
                            state = State.CollectingBox;
                        }
                    }
                    else
                    {
                        // There is real box nearby. Fly to it.
                        boxToCollect = nearestBox;
                        Fly(boxToCollect);
                        state = State.CollectingBox;
                        Thread.Sleep(50);
                        continue;
                    }
                }

                if (state == State.CollectingBox && Running)
                {
                    if (api.Account.Flying)
                    {
                        // We are flying to box. Change target box if there is box closer than target. Otherwise just wait.
                        var flybyBox = memorizedBoxes.OrderBy(box => CalculateDistance(box.Position)).FirstOrDefault();
                        if (flybyBox != null && CalculateDistance(flybyBox.Position) < CalculateDistance(boxToCollect.Position))
                        {
                            boxToCollect = flybyBox;
                            Fly(boxToCollect);
                        }
                        Thread.Sleep(50);
                        continue;
                    }

                    // Finished flying to box.
                    var nearestBox = boxes.OrderBy(box => CalculateDistance(box.Position)).FirstOrDefault();
                    if (nearestBox == null)
                    {
                        // There is no box nearby.
                        if (boxToCollect != null)
                        {
                            //But we had box to collect. Someone collected it. Remove it from memorizedBoxes (real boxes gets removed based on packets).
                            lock (api.MemorizedBoxes)
                            {
                                api.MemorizedBoxes.RemoveAll(box => box.Hash == boxToCollect.Hash);
                            }
                            boxToCollect = null;
                        }
                        // Continue flying.
                        state = State.SearchingBox;
                    }
                    else
                    {
                        // There is box nearby.
                        if (CalculateDistance(nearestBox.Position) < 50)
                        {
                            // We are close to box. Send packet to collect it.
                            var tempShipY = api.Account.Y;
                            if ((api.Account.X + api.Account.Y + nearestBox.Position.Y) % 3 == 0)
                            {
                                tempShipY++;
                            }
                            api.vanillaClient.SendEncoded(new CollectBox(nearestBox.Hash, nearestBox.Position.X, nearestBox.Position.Y, api.Account.X, tempShipY));
                        }
                        lock (api.Boxes) lock (api.MemorizedBoxes)
                            {
                                api.Boxes.RemoveAll(box => box.Hash == nearestBox.Hash);
                                api.MemorizedBoxes.RemoveAll(box => box.Hash == nearestBox.Hash);
                                boxToCollect = null;
                            }
                        state = State.SearchingBox;
                    }
                }
                Thread.Sleep(100);
            }
        }

        #endregion

        #region MapDrawing

        private void Render()
        {
            while (true)
            {
                stopwatch.Restart();
                List<Box> boxes;
                List<Box> memorizedBoxes;
                List<Ore> ores;
                List<Ship> ships;
                List<Gate> gates;
                List<Building> buildings;

                // Locks by copying to list.

                lock (api.Boxes)
                {
                    boxes = api.Boxes.ToList().Where(box => displayableBoxes.Contains(box.Type)).ToList();
                }

                lock (api.MemorizedBoxes)
                {
                    memorizedBoxes = api.MemorizedBoxes.ToList().Where(box => displayableBoxes.Contains(box.Type)).Where(box => !boxes.Contains(box)).ToList();
                }

                lock (api.Ores)
                {
                    ores = api.Ores.ToList();
                }

                lock (api.Ships)
                {
                    ships = api.Ships.ToList();
                }

                lock (api.Gates)
                {
                    gates = api.Gates.ToList();
                }

                lock (api.Buildings)
                {
                    buildings = api.Buildings.ToList();
                }

                var bitmap = new Bitmap(minimap.Width, minimap.Height);
                using (var g = Graphics.FromImage(bitmap))
                {
                    DrawBorders(g);

                    foreach (Box box in boxes)
                    {
                        DrawBox(g, box);
                    }

                    foreach (Box box in memorizedBoxes)
                    {
                        DrawMemorizedBox(g, box);
                    }

                    foreach (Ore ore in ores)
                    {
                        DrawOre(g, ore);
                    }

                    foreach (Ship ship in ships)
                    {
                        DrawShip(g, ship);
                    }

                    foreach (Gate gate in gates)
                    {
                        DrawGate(g, gate);
                    }

                    foreach (Building building in buildings)
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
                }
                catch (Exception ex)
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

        private void DrawText(string text)
        {
            Console.WriteLine(text);
            var bitmap = new Bitmap(minimap.Width, minimap.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                SizeF size = g.MeasureString(text, Config.font);
                g.DrawString(text, Config.font, new SolidBrush(Color.White), minimap.Width / 2 - size.Width / 2,
                    minimap.Height / 2 - size.Height / 2);
            }
            if (minimap.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    minimap.Image = bitmap;
                });
            }
            else
            {
                minimap.Image = bitmap;
            }
        }

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
            switch (ore.Type)
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
            if (ship.NPC)
            {
                g.DrawRectangle(new Pen(Config.npc), new Rectangle(Scale(ship.X), Scale(ship.Y), 1, 1));
                return;
            }

            if (ship.FactionID != api.Account.FactionID)
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
            if (api.Account.Ready)
            {
                g.DrawLine(new Pen(Config.hero), new Point(Scale(api.Account.X), 0), new Point(Scale(api.Account.X), minimap.Height));
                g.DrawLine(new Pen(Config.hero), new Point(0, Scale(api.Account.Y)), new Point(minimap.Width, Scale(api.Account.Y)));

                if (api.Account.Flying)
                {
                    g.DrawLine(new Pen(Config.hero), new Point(Scale(api.Account.X), Scale(api.Account.Y)), new Point(Scale(api.Account.TargetX), Scale(api.Account.TargetY)));
                }
            }
        }

        private void DrawDetails(Graphics g)
        {
            if (api.Account.Ready)
            {
                string uridium = $"Uridium: {api.Account.CollectedUridium}";
                string ee = $"Extra energy: {api.Account.CollectedEE}";
                SizeF uridiumSize = g.MeasureString(uridium, Config.font);
                SizeF eeSize = g.MeasureString(ee, Config.font);

                g.DrawString(uridium, Config.font, new SolidBrush(Color.DarkGray), minimap.Width - Config.poizoneSize - uridiumSize.Width - 4,
                    Config.poizoneSize + 4);
                g.DrawString(ee, Config.font, new SolidBrush(Color.DarkGray), minimap.Width - Config.poizoneSize - eeSize.Width - 4,
                    Config.poizoneSize + uridiumSize.Height + 4);

                if (api.Account.Cloaked)
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

        #endregion

        #region Player controls

        private void Fly(Box box)
        {
            FlyWithAnimation(box.Position.X, box.Position.Y);
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
            }
            catch (Exception ex)
            {
                api.Account.Flying = false;
                Console.WriteLine(ex);
            }
        }

        private void Jump()
        {
            api.vanillaClient.SendEncoded(new Jump());
        }

        private void ChangeConfig()
        {
            int targetConfig = (api.Account.Config == 1) ? 2 : 1;
            api.vanillaClient.SendEncoded(new OldStylePacket($"S|CFG|{targetConfig}|{api.Account.UserID}|{api.Account.SID}"));
        }

        #endregion

        #region Helpers

        private int Scale(int value)
        {
            return (int)(value * Config.k) + Config.poizoneSize;
        }

        private int ReverseScale(int value)
        {
            return (int)(value / Config.k);
        }

        #endregion

        #region GUI

        private void UpdateProgressBars()
        {
            Invoke((MethodInvoker)delegate
            {
                hpBar.UpdateStats(api.Account.HP, api.Account.MaxHP);
                shieldBar.UpdateStats(api.Account.Shield, api.Account.MaxShield);
                cargoBar.UpdateStats(api.Account.CargoCapacity - api.Account.FreeCargoSpace, api.Account.CargoCapacity);
            });
        }

        private void InitializeComponent()
        {
            this.minimap = new System.Windows.Forms.PictureBox();
            this.hpBar = new PolskaBot.ColorProgressBar();
            this.shieldBar = new PolskaBot.ColorProgressBar();
            this.cargoBar = new PolskaBot.ColorProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.minimap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hpBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shieldBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cargoBar)).BeginInit();
            this.SuspendLayout();
            // 
            // minimap
            // 
            this.minimap.BackColor = System.Drawing.Color.Black;
            this.minimap.Location = new System.Drawing.Point(6, 6);
            this.minimap.Name = "minimap";
            this.minimap.Size = new System.Drawing.Size(335, 222);
            this.minimap.TabIndex = 0;
            this.minimap.TabStop = false;
            // 
            // hpBar
            // 
            this.hpBar.FontPrimary = System.Drawing.Color.White;
            this.hpBar.FontSecondary = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.hpBar.Location = new System.Drawing.Point(6, 234);
            this.hpBar.Maximum = 100;
            this.hpBar.Name = "hpBar";
            this.hpBar.PaintColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(209)))), ((int)(((byte)(79)))));
            this.hpBar.Size = new System.Drawing.Size(335, 25);
            this.hpBar.TabIndex = 0;
            this.hpBar.TabStop = false;
            this.hpBar.Value = 0;
            // 
            // shieldBar
            // 
            this.shieldBar.FontPrimary = System.Drawing.Color.White;
            this.shieldBar.FontSecondary = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.shieldBar.Location = new System.Drawing.Point(6, 265);
            this.shieldBar.Maximum = 100;
            this.shieldBar.Name = "shieldBar";
            this.shieldBar.PaintColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(157)))), ((int)(((byte)(209)))));
            this.shieldBar.Size = new System.Drawing.Size(335, 25);
            this.shieldBar.TabIndex = 0;
            this.shieldBar.TabStop = false;
            this.shieldBar.Value = 0;
            // 
            // cargoBar
            // 
            this.cargoBar.FontPrimary = System.Drawing.Color.White;
            this.cargoBar.FontSecondary = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.cargoBar.Location = new System.Drawing.Point(6, 296);
            this.cargoBar.Maximum = 100;
            this.cargoBar.Name = "cargoBar";
            this.cargoBar.PaintColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(179)))), ((int)(((byte)(71)))));
            this.cargoBar.Size = new System.Drawing.Size(335, 25);
            this.cargoBar.TabIndex = 0;
            this.cargoBar.TabStop = false;
            this.cargoBar.Value = 0;
            // 
            // BotPage
            // 
            this.Controls.Add(this.minimap);
            this.Controls.Add(this.hpBar);
            this.Controls.Add(this.shieldBar);
            this.Controls.Add(this.cargoBar);
            ((System.ComponentModel.ISupportInitialize)(this.minimap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hpBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shieldBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cargoBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
