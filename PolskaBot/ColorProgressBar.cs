using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace PolskaBot
{
    class ColorProgressBar : PictureBox
    {
        public int Value { get; set; } = 0;
        public int Maximum { get; set; } = 100;

        public Color PaintColor { get; set; }
        public Color FontPrimary { get; set; } = Color.White;
        public Color FontSecondary { get; set; } = Color.FromArgb(95, 95, 95);

        private Font font = new Font("Arial", 9, FontStyle.Bold);

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            string text = Value.ToString();
            SizeF size = pe.Graphics.MeasureString(text, font);

            float fill = (Value / (float)Maximum) * Size.Width;

            pe.Graphics.FillRectangle(new SolidBrush(PaintColor), 0, 0, fill, Size.Height);

            if(float.IsNaN(fill) || fill < Size.Width/2 + size.Width/2)
            {
                pe.Graphics.DrawString(text, font, new SolidBrush(FontSecondary), Size.Width / 2 - size.Width / 2,
                Size.Height / 2 - size.Height / 2);
            } else
            {
                pe.Graphics.DrawString(text, font, new SolidBrush(FontPrimary), Size.Width / 2 - size.Width / 2,
                Size.Height / 2 - size.Height / 2);
            }
        }

        public void UpdateStats(int value, int maximum)
        {
            Maximum = maximum;
            Value = value;
            Refresh();
        }
    }
}
