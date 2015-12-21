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
    
    public partial class Bot : Form
    {
        public int AccountsCount { get; set; }

        private List<BotPage> pages = new List<BotPage>();

        public Bot()
        {
            InitializeComponent();
            Load += (s, e) => Init();
        }

        private void Init()
        {
            AcceptButton = loginButton;
            loginButton.Click += (s, e) =>
            {
                if(!usernameBox.Text.Equals("") || !passwordBox.Text.Equals(""))
                {
                    usernameBox.Text = "";
                    passwordBox.Text = "";
                    var botPage = new BotPage(usernameBox.Text, passwordBox.Text);
                    pages.Add(botPage);
                    botTabs.Controls.Add(botPage);
                    botTabs.SelectedIndex = ++AccountsCount;
                }
            };

            startButton.Click += (s, e) =>
            {
                pages[botTabs.SelectedIndex - 1].Running = true;
            };

            stopButton.Click += (s, e) =>
            {
                pages[botTabs.SelectedIndex - 1].Running = false;
            };
        }
    }
}
