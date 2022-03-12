using System.Drawing;
using W9xNET.User32.Renderers.Win95;

namespace W9xNET.Explorer.Renderers.Win9x
{
    internal class StartButton3dRenderer : Button3dRenderer
    {
        #region Initializer

        internal StartButton3dRenderer(Graphics graphics)
            : base(graphics)
        {
        }

        #endregion

        protected override void RenderDown(ref Rectangle rect)
        {
            _graphics.Clear(Color.FromArgb(192, 192, 192));

            //using (var src = new Bitmap(Properties.Resources.Ms_StartButton_Img))
            //    _graphics.DrawImage(src, new Point(5, 5));

            using (Pen p = new Pen(Color.Black))
            {
                _graphics.DrawLine(p,
                    x1: rect.X, x2: rect.X,
                    y1: rect.Y, y2: rect.Height - 2);

                _graphics.DrawLine(p,
                    x1: rect.X, x2: rect.Right - 2,
                    y1: rect.Y, y2: rect.Y);
            }

            using (Pen p = new Pen(Color.White))
            {
                _graphics.DrawLine(p,
                    x1: rect.Width - 1, x2: rect.Width - 1,
                    y1: rect.Y, y2: rect.Height);

                _graphics.DrawLine(p,
                    x1: rect.X, x2: rect.Width - 1,
                    y1: rect.Height - 1, y2: rect.Height - 1);
            }

            using (Pen p = new Pen(Color.FromArgb(223, 223, 223)))
            {
                _graphics.DrawLine(p,
                    x1: rect.Width - 2, x2: rect.Width - 2,
                    y1: rect.Y + 1, y2: rect.Height - 2);

                _graphics.DrawLine(p,
                    x1: rect.X + 1, x2: rect.Width - 2,
                    y1: rect.Height - 2, y2: rect.Height - 2);
            }

            using (Pen p = new Pen(Color.FromArgb(128, 128, 128)))
            {
                _graphics.DrawLine(p,
                    x1: rect.X + 1, x2: rect.Width - 3,
                    y1: rect.Y + 1, y2: rect.Y + 1);

                _graphics.DrawLine(p,
                    x1: rect.X + 1, x2: rect.X + 1,
                    y1: rect.Y + 1, y2: rect.Height - 3);
            }

            base.RenderDotted(ref rect);
        }

        protected override void RenderFocused(ref Rectangle rect)
        {
            base.RenderFocused(ref rect);
        }

        protected override void RenderNormal(ref Rectangle rect)
        {
            base.RenderNormal(ref rect);

            //using (var src = new Bitmap(Properties.Resources.Ms_StartButton_Img))
            //    _graphics.DrawImage(src, new Point(4, 4));

        }
    }
}