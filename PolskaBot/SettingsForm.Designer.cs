namespace PolskaBot
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.enableCollectorBox = new System.Windows.Forms.CheckBox();
            this.ebBox = new System.Windows.Forms.CheckBox();
            this.bbBox = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.hpSlider = new System.Windows.Forms.TrackBar();
            this.repairLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.repairBase = new System.Windows.Forms.RadioButton();
            this.repairPortal = new System.Windows.Forms.RadioButton();
            this.repairBattle = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hpSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.enableCollectorBox);
            this.groupBox1.Controls.Add(this.ebBox);
            this.groupBox1.Controls.Add(this.bbBox);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 252);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Collector";
            // 
            // enableCollectorBox
            // 
            this.enableCollectorBox.AutoSize = true;
            this.enableCollectorBox.Location = new System.Drawing.Point(6, 19);
            this.enableCollectorBox.Name = "enableCollectorBox";
            this.enableCollectorBox.Size = new System.Drawing.Size(102, 17);
            this.enableCollectorBox.TabIndex = 2;
            this.enableCollectorBox.Text = "Enable collector";
            this.enableCollectorBox.UseVisualStyleBackColor = true;
            // 
            // ebBox
            // 
            this.ebBox.AutoSize = true;
            this.ebBox.Location = new System.Drawing.Point(6, 65);
            this.ebBox.Name = "ebBox";
            this.ebBox.Size = new System.Drawing.Size(119, 17);
            this.ebBox.TabIndex = 1;
            this.ebBox.Text = "Collect event boxes";
            this.ebBox.UseVisualStyleBackColor = true;
            // 
            // bbBox
            // 
            this.bbBox.AutoSize = true;
            this.bbBox.Location = new System.Drawing.Point(6, 42);
            this.bbBox.Name = "bbBox";
            this.bbBox.Size = new System.Drawing.Size(121, 17);
            this.bbBox.TabIndex = 0;
            this.bbBox.Text = "Collect bonus boxes";
            this.bbBox.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(13, 271);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(494, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.repairBattle);
            this.groupBox2.Controls.Add(this.repairPortal);
            this.groupBox2.Controls.Add(this.repairBase);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.hpSlider);
            this.groupBox2.Controls.Add(this.repairLabel);
            this.groupBox2.Location = new System.Drawing.Point(220, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(287, 252);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "General";
            // 
            // hpSlider
            // 
            this.hpSlider.LargeChange = 10;
            this.hpSlider.Location = new System.Drawing.Point(10, 42);
            this.hpSlider.Maximum = 100;
            this.hpSlider.Name = "hpSlider";
            this.hpSlider.Size = new System.Drawing.Size(271, 45);
            this.hpSlider.TabIndex = 1;
            this.hpSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.hpSlider.Value = 60;
            // 
            // repairLabel
            // 
            this.repairLabel.AutoSize = true;
            this.repairLabel.Location = new System.Drawing.Point(7, 20);
            this.repairLabel.Name = "repairLabel";
            this.repairLabel.Size = new System.Drawing.Size(130, 13);
            this.repairLabel.TabIndex = 0;
            this.repairLabel.Text = "Repair when HP less than";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Repair at";
            // 
            // repairBase
            // 
            this.repairBase.AutoSize = true;
            this.repairBase.Location = new System.Drawing.Point(10, 82);
            this.repairBase.Name = "repairBase";
            this.repairBase.Size = new System.Drawing.Size(95, 17);
            this.repairBase.TabIndex = 3;
            this.repairBase.TabStop = true;
            this.repairBase.Text = "Company base";
            this.repairBase.UseVisualStyleBackColor = true;
            // 
            // repairPortal
            // 
            this.repairPortal.AutoSize = true;
            this.repairPortal.Location = new System.Drawing.Point(10, 105);
            this.repairPortal.Name = "repairPortal";
            this.repairPortal.Size = new System.Drawing.Size(52, 17);
            this.repairPortal.TabIndex = 4;
            this.repairPortal.TabStop = true;
            this.repairPortal.Text = "Portal";
            this.repairPortal.UseVisualStyleBackColor = true;
            // 
            // repairBattle
            // 
            this.repairBattle.AutoSize = true;
            this.repairBattle.Location = new System.Drawing.Point(10, 128);
            this.repairBattle.Name = "repairBattle";
            this.repairBattle.Size = new System.Drawing.Size(63, 17);
            this.repairBattle.TabIndex = 5;
            this.repairBattle.TabStop = true;
            this.repairBattle.Text = "In battle";
            this.repairBattle.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 301);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hpSlider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.CheckBox bbBox;
        private System.Windows.Forms.CheckBox ebBox;
        private System.Windows.Forms.CheckBox enableCollectorBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label repairLabel;
        private System.Windows.Forms.TrackBar hpSlider;
        private System.Windows.Forms.RadioButton repairBattle;
        private System.Windows.Forms.RadioButton repairPortal;
        private System.Windows.Forms.RadioButton repairBase;
        private System.Windows.Forms.Label label1;
    }
}