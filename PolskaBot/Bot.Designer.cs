namespace PolskaBot
{
    partial class Bot
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Uridium");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Credits");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Experience");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Honor");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Extra energy");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Collected", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Bot));
            this.minimap = new System.Windows.Forms.PictureBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.changeConfigButton = new System.Windows.Forms.Button();
            this.log = new System.Windows.Forms.TextBox();
            this.hpBar = new System.Windows.Forms.TrackBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.UserTab = new System.Windows.Forms.TabPage();
            this.statsView = new System.Windows.Forms.TreeView();
            this.LogTab = new System.Windows.Forms.TabPage();
            this.cargoProgressBar = new PolskaBot.ColorProgressBar();
            this.shieldProgressBar = new PolskaBot.ColorProgressBar();
            this.hpProgressBar = new PolskaBot.ColorProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.minimap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hpBar)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.UserTab.SuspendLayout();
            this.LogTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cargoProgressBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shieldProgressBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hpProgressBar)).BeginInit();
            this.SuspendLayout();
            // 
            // minimap
            // 
            this.minimap.BackColor = System.Drawing.Color.Black;
            this.minimap.Location = new System.Drawing.Point(13, 41);
            this.minimap.Name = "minimap";
            this.minimap.Size = new System.Drawing.Size(335, 222);
            this.minimap.TabIndex = 0;
            this.minimap.TabStop = false;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(13, 12);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(108, 23);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(127, 12);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(108, 23);
            this.stopButton.TabIndex = 2;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            // 
            // changeConfigButton
            // 
            this.changeConfigButton.Location = new System.Drawing.Point(241, 12);
            this.changeConfigButton.Name = "changeConfigButton";
            this.changeConfigButton.Size = new System.Drawing.Size(108, 23);
            this.changeConfigButton.TabIndex = 3;
            this.changeConfigButton.Text = "Config";
            this.changeConfigButton.UseVisualStyleBackColor = true;
            // 
            // log
            // 
            this.log.Location = new System.Drawing.Point(6, 6);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.ReadOnly = true;
            this.log.Size = new System.Drawing.Size(315, 172);
            this.log.TabIndex = 4;
            // 
            // hpBar
            // 
            this.hpBar.AutoSize = false;
            this.hpBar.Location = new System.Drawing.Point(1, 269);
            this.hpBar.Maximum = 100;
            this.hpBar.Name = "hpBar";
            this.hpBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.hpBar.Size = new System.Drawing.Size(360, 26);
            this.hpBar.TabIndex = 8;
            this.hpBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.hpBar.Value = 30;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.UserTab);
            this.tabControl1.Controls.Add(this.LogTab);
            this.tabControl1.Location = new System.Drawing.Point(13, 389);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(335, 210);
            this.tabControl1.TabIndex = 9;
            // 
            // UserTab
            // 
            this.UserTab.Controls.Add(this.statsView);
            this.UserTab.Location = new System.Drawing.Point(4, 22);
            this.UserTab.Name = "UserTab";
            this.UserTab.Padding = new System.Windows.Forms.Padding(3);
            this.UserTab.Size = new System.Drawing.Size(327, 184);
            this.UserTab.TabIndex = 0;
            this.UserTab.Text = "User";
            this.UserTab.UseVisualStyleBackColor = true;
            // 
            // statsView
            // 
            this.statsView.Location = new System.Drawing.Point(7, 7);
            this.statsView.Name = "statsView";
            treeNode1.Name = "UridiumNode";
            treeNode1.Text = "Uridium";
            treeNode2.Name = "CreditsNode";
            treeNode2.Text = "Credits";
            treeNode3.Name = "XPNode";
            treeNode3.Text = "Experience";
            treeNode4.Name = "HonorNode";
            treeNode4.Text = "Honor";
            treeNode5.Name = "EENode";
            treeNode5.Text = "Extra energy";
            treeNode6.Checked = true;
            treeNode6.Name = "collected";
            treeNode6.Text = "Collected";
            this.statsView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6});
            this.statsView.Size = new System.Drawing.Size(314, 171);
            this.statsView.TabIndex = 0;
            // 
            // LogTab
            // 
            this.LogTab.Controls.Add(this.log);
            this.LogTab.Location = new System.Drawing.Point(4, 22);
            this.LogTab.Name = "LogTab";
            this.LogTab.Padding = new System.Windows.Forms.Padding(3);
            this.LogTab.Size = new System.Drawing.Size(327, 184);
            this.LogTab.TabIndex = 1;
            this.LogTab.Text = "Log";
            this.LogTab.UseVisualStyleBackColor = true;
            // 
            // cargoProgressBar
            // 
            this.cargoProgressBar.FontPrimary = System.Drawing.Color.White;
            this.cargoProgressBar.FontSecondary = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.cargoProgressBar.Location = new System.Drawing.Point(12, 357);
            this.cargoProgressBar.Maximum = 100;
            this.cargoProgressBar.Name = "cargoProgressBar";
            this.cargoProgressBar.PaintColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(179)))), ((int)(((byte)(71)))));
            this.cargoProgressBar.Size = new System.Drawing.Size(336, 25);
            this.cargoProgressBar.TabIndex = 7;
            this.cargoProgressBar.TabStop = false;
            this.cargoProgressBar.Value = 0;
            // 
            // shieldProgressBar
            // 
            this.shieldProgressBar.FontPrimary = System.Drawing.Color.White;
            this.shieldProgressBar.FontSecondary = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.shieldProgressBar.Location = new System.Drawing.Point(12, 329);
            this.shieldProgressBar.Maximum = 100;
            this.shieldProgressBar.Name = "shieldProgressBar";
            this.shieldProgressBar.PaintColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(157)))), ((int)(((byte)(209)))));
            this.shieldProgressBar.Size = new System.Drawing.Size(336, 25);
            this.shieldProgressBar.TabIndex = 6;
            this.shieldProgressBar.TabStop = false;
            this.shieldProgressBar.Value = 0;
            // 
            // hpProgressBar
            // 
            this.hpProgressBar.FontPrimary = System.Drawing.Color.White;
            this.hpProgressBar.FontSecondary = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.hpProgressBar.Location = new System.Drawing.Point(12, 301);
            this.hpProgressBar.Maximum = 100;
            this.hpProgressBar.Name = "hpProgressBar";
            this.hpProgressBar.PaintColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(209)))), ((int)(((byte)(79)))));
            this.hpProgressBar.Size = new System.Drawing.Size(336, 25);
            this.hpProgressBar.TabIndex = 5;
            this.hpProgressBar.TabStop = false;
            this.hpProgressBar.Value = 0;
            // 
            // Bot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 604);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.hpBar);
            this.Controls.Add(this.cargoProgressBar);
            this.Controls.Add(this.shieldProgressBar);
            this.Controls.Add(this.hpProgressBar);
            this.Controls.Add(this.changeConfigButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.minimap);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Bot";
            this.Text = "PolskaBot";
            ((System.ComponentModel.ISupportInitialize)(this.minimap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hpBar)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.UserTab.ResumeLayout(false);
            this.LogTab.ResumeLayout(false);
            this.LogTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cargoProgressBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shieldProgressBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hpProgressBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox minimap;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button changeConfigButton;
        private System.Windows.Forms.TextBox log;
        private ColorProgressBar hpProgressBar;
        private ColorProgressBar shieldProgressBar;
        private ColorProgressBar cargoProgressBar;
        private System.Windows.Forms.TrackBar hpBar;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage UserTab;
        private System.Windows.Forms.TabPage LogTab;
        private System.Windows.Forms.TreeView statsView;
    }
}

