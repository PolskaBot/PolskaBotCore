using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolskaBot
{
    public partial class SettingsForm : Form
    {
        private BotPage botPage;

        public SettingsForm(BotPage botPage)
        {
            this.botPage = botPage;
            InitializeComponent();

            LoadSettings();

            saveButton.Click += (s, e) =>
            {
                botPage.Settings.CollectorEnabled = enableCollectorBox.Checked;
                botPage.Settings.CollectBonusBoxes = bbBox.Checked;
                botPage.Settings.CollectEventBoxes = ebBox.Checked;
                Close();
            };
        }

        private void LoadSettings()
        {
            enableCollectorBox.Checked = botPage.Settings.CollectorEnabled;
            bbBox.Checked = botPage.Settings.CollectBonusBoxes;
            ebBox.Checked = botPage.Settings.CollectEventBoxes;
        }
    }
}
