using W9xNET.User32.Interfaces;
using static W9xNET.User32.Renderers.Win95.Button3dRenderer;

namespace W9xNET.User32.Renderers.Win95
{
    [Obsolete("An Eternal95 stub")]
    public class Button3dRenderer : IRendererWin9x<Button3dRenderType>
    {
        #region Properties

        protected readonly Graphics _graphics;

        #endregion

        #region Initializer

        public Button3dRenderer(Graphics graphics)
        {
            _graphics = graphics;
        }

        #endregion

        #region Enum

        public enum Button3dRenderType
        {
            None,
            Normal,
            Dotted,
            Focused,
            Down
        }

        #endregion

        #region Public Constants

        public const int OffsetLeft = 3;
        public const int OffsetRight = 3;
        public const int OffsetTop = 3;
        public const int OffsetBottom = 3;

        #endregion

        #region Functions

        public virtual void Render(Rectangle rect, Button3dRenderType button3DRenderType = Button3dRenderType.Normal)
        {
            switch (button3DRenderType)
            {
                case Button3dRenderType.None:
                    break;

                case Button3dRenderType.Dotted:
                    RenderDotted(ref rect);
                    break;

                case Button3dRenderType.Down:
                    RenderDown(ref rect);
                    break;

                case Button3dRenderType.Normal:
                    RenderNormal(ref rect);
                    break;

                case Button3dRenderType.Focused:
                    RenderFocused(ref rect);
                    break;
            }
        }

        #endregion

        #region Protected Functions

        protected virtual void RenderDown(ref Rectangle rect)
        {

        }

        protected virtual void RenderDotted(ref Rectangle rect)
        {
            var width = rect.Width - OffsetLeft;
            var height = rect.Height - OffsetTop;

            using var p = new Pen(Color.Black);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            _graphics.DrawRectangle(p, OffsetLeft, OffsetTop, width - OffsetLeft, height - OffsetTop);
        }

        protected virtual void RenderNormal(ref Rectangle rect)
        {
            using (Pen p = new(Color.White))
            {
                _graphics.DrawLine(p,
                    x1: rect.X, x2: rect.X,
                    y1: rect.Y, y2: rect.Height - 2);

                _graphics.DrawLine(p,
                    x1: rect.X, x2: rect.Right - 2,
                    y1: rect.Y, y2: rect.Y);
            }

            using (Pen p = new(Color.Black))
            {
                _graphics.DrawLine(p,
                    x1: rect.Width - 1, x2: rect.Width - 1,
                    y1: rect.Y, y2: rect.Height);

                _graphics.DrawLine(p,
                    x1: rect.X, x2: rect.Width - 1,
                    y1: rect.Height - 1, y2: rect.Height - 1);
            }

            using (Pen p = new(Color.FromArgb(128, 128, 128)))
            {
                _graphics.DrawLine(p,
                    x1: rect.Width - 2, x2: rect.Width - 2,
                    y1: rect.Y + 1, y2: rect.Height - 2);

                _graphics.DrawLine(p,
                    x1: rect.X + 1, x2: rect.Width - 2,
                    y1: rect.Height - 2, y2: rect.Height - 2);
            }

            using (Pen p = new(Color.FromArgb(223, 223, 223)))
            {
                _graphics.DrawLine(p,
                    x1: rect.X + 1, x2: rect.Width - 3,
                    y1: rect.Y + 1, y2: rect.Y + 1);

                _graphics.DrawLine(p,
                    x1: rect.X + 1, x2: rect.X + 1,
                    y1: rect.Y + 1, y2: rect.Height - 3);
            }
        }

        protected virtual void RenderFocused(ref Rectangle rect)
        {

        }

        #endregion
    }
}
