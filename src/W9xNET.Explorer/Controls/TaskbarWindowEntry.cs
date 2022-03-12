using System.Drawing.Drawing2D;
using W9xNET.RcReader.Icons;
using W9xNET.User32.Controls;

namespace W9xNET.Explorer.Controls
{
    internal class TaskbarWindowEntry : NxnControl
    {
        internal const int MaxWidth = 160;
        internal const int MinWidth = 22;
        internal const int MaxHeight = 22;


        public new string Text { get => base.Text; set => base.Text = value; }

        public Icon Icon { get; set; }

        public TaskbarWindowEntry()
        {
            Size = new(MaxWidth, MaxHeight);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var font = new Font("MS Shell Dlg", 8);// User32.Properties.Settings.Default.ActiveTitleBar_Font;
            var captionSize = e.Graphics.MeasureString(Text, font);

            e.Graphics.DrawIcon(Icon, 2, (int)((Height * 0.5) - (Icon.Height * 0.5)));

            e.Graphics.DrawString(Text, (Font)font,
                new SolidBrush((Color)User32.Properties.Settings.Default.ThreeDObjects_FontColor), Icon.Width + 1, (int)((Height * 0.5) - (captionSize.Height * 0.5)));

            GraphicsPath path = new();

            using (Pen p = new(Color.Gray))
            {
                path.Reset();

                int w = Width - 2;
                int h = Height - 2;

                path.AddLine(x1: w,
                             x2: w,
                             y1: 0,
                             y2: h);

                path.AddLine(x1: 0,
                             x2: w,
                             y1: h,
                             y2: w);

                e.Graphics.DrawPath(p, path);
            }


            //TODO: From StartButtonControl.CommonRender function

            using (Pen p = new(Color.White))
            {
                path.Reset();

                path.AddLine(x1: 0,
                             x2: Width,
                             y1: 0,
                             y2: 0);

                path.AddLine(x1: 0,
                             x2: 0,
                             y1: 0,
                             y2: Height);

                e.Graphics.DrawPath(p, path);
            }

            using (Pen p = new(Color.Black))
            {
                path.Reset();

                int w = Width - 1;
                int h = Height - 1;

                path.AddLine(x1: w,
                             x2: w,
                             y1: 0,
                             y2: h);

                path.AddLine(x1: 0,
                             x2: w,
                             y1: h,
                             y2: w);

                e.Graphics.DrawPath(p, path);
            }
        }
    }
}