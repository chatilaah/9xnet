using System.Drawing;
using W9xNET.User32.Interfaces;
using static W9xNET.Explorer.Renderers.Win9x.Taskbar3dRenderer;

namespace W9xNET.Explorer.Renderers.Win9x
{
    class Taskbar3dRenderer : IRendererWin9x<Taskbar3dRendererType>
    {
        #region Properties

        protected readonly Graphics _graphics;

        #endregion

        #region Initializer

        public Taskbar3dRenderer(Graphics graphics)
        {
            _graphics = graphics;
        }

        #endregion

        #region Enum

        public enum Taskbar3dRendererType
        {
            Normal
        }

        #endregion

        #region Public Constants

        public const int OffsetLeft = 3;
        public const int OffsetRight = 3;
        public const int OffsetTop = 3;
        public const int OffsetBottom = 3;

        #endregion

        #region Functions

        public virtual void Render(Rectangle rect, Taskbar3dRendererType taskbar3dRendererType = Taskbar3dRendererType.Normal)
        {
            switch (taskbar3dRendererType)
            {
                case Taskbar3dRendererType.Normal:
                    RenderNormal(ref rect);
                    break;
            }
        }

        #endregion

        #region Protected Functions

        protected virtual void RenderNormal(ref Rectangle rect)
        {
            using (Pen p = new Pen(Color.FromArgb(223, 223, 223)))
            {
                _graphics.DrawLine(p,
                    x1: rect.X, x2: rect.Width,
                    y1: rect.Y, y2: rect.Y);
            }

            using (Pen p = new Pen(Color.FromArgb(255, 255, 255)))
            {
                _graphics.DrawLine(p,
                    x1: rect.X, x2: rect.Width,
                    y1: rect.Y + 1, y2: rect.Y + 1);
            }
        }

        #endregion
    }
}