using W9xNET.User32.Interfaces;
using static W9xNET.User32.Renderers.Win95.Sunken3dRenderer;

namespace W9xNET.User32.Renderers.Win95
{
    [Obsolete("An Eternal95 stub")]
    public class Sunken3dRenderer : IRendererWin9x<Sunken3dRenderType>
    {
        #region Properties

        private readonly Graphics _graphics;

        #endregion

        #region Initializer

        public Sunken3dRenderer(Graphics graphics)
        {
            _graphics = graphics;
        }

        #endregion

        #region Enum

        public enum Sunken3dRenderType
        {
            Normal,
            Dotted
        }

        #endregion

        #region Functions

        public void Render(Rectangle rect, Sunken3dRenderType sunken3DRenderType = Sunken3dRenderType.Normal)
        {
            switch (sunken3DRenderType)
            {
                case Sunken3dRenderType.Dotted:
                    RenderDotted(ref rect);
                    break;

                case Sunken3dRenderType.Normal:
                    RenderNormal(ref rect);
                    break;
            }
        }

        #endregion

        #region Private Functions

        protected virtual void RenderNormal(ref Rectangle rect)
        {
            using (Pen p = new(Color.FromArgb(128, 128, 128)))
            {
                _graphics.DrawLine(p,
                    x1: rect.X, x2: rect.Width,
                    y1: rect.Y, y2: rect.Y);

                _graphics.DrawLine(p,
                    x1: rect.X, x2: rect.X,
                    y1: rect.Y, y2: rect.Height);
            }

            using (Pen p = new(Color.FromArgb(255, 255, 255)))
            {
                _graphics.DrawLine(p,
                    x1: rect.Width - 1, x2: rect.Width - 1,
                    y1: rect.Y, y2: rect.Height);

                _graphics.DrawLine(p,
                    x1: rect.X, x2: rect.Width - 1,
                    y1: rect.Height - 1, y2: rect.Height - 1);
            }
        }

        protected virtual void RenderDotted(ref Rectangle rect)
        {

        }

        #endregion

    }
}
