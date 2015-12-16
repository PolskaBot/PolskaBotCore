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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Bot));
            this.minimap = new System.Windows.Forms.PictureBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.changeConfigButton = new System.Windows.Forms.Button();
            this.log = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.minimap)).BeginInit();
            this.SuspendLayout();
            // 
            // minimap
            // 
            this.minimap.BackColor = System.Drawing.Color.Black;
            this.minimap.Location = new System.Drawing.Point(12, 12);
            this.minimap.Name = "minimap";
            this.minimap.Size = new System.Drawing.Size(335, 222);
            this.minimap.TabIndex = 0;
            this.minimap.TabStop = false;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(12, 243);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(108, 23);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(126, 243);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(108, 23);
            this.stopButton.TabIndex = 2;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            // 
            // changeConfigButton
            // 
            this.changeConfigButton.Location = new System.Drawing.Point(240, 243);
            this.changeConfigButton.Name = "changeConfigButton";
            this.changeConfigButton.Size = new System.Drawing.Size(108, 23);
            this.changeConfigButton.TabIndex = 3;
            this.changeConfigButton.Text = "Config";
            this.changeConfigButton.UseVisualStyleBackColor = true;
            // 
            // log
            // 
            this.log.Location = new System.Drawing.Point(353, 12);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.ReadOnly = true;
            this.log.Size = new System.Drawing.Size(264, 254);
            this.log.TabIndex = 4;
            // 
            // Bot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 275);
            this.Controls.Add(this.log);
            this.Controls.Add(this.changeConfigButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.minimap);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Bot";
            this.Text = "PolskaBot";
            ((System.ComponentModel.ISupportInitialize)(this.minimap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox minimap;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button changeConfigButton;
        private System.Windows.Forms.TextBox log;
    }
}

