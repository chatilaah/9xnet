using System.Drawing;
using System.Windows.Forms;
using W9xNET.User32.Renderers.Win95;

namespace W9xNET.User32.Controls.Win95
{
    [Obsolete("An Eternal95 stub")]
    public class Button95 : Button
    {
        #region Properties

        public Button3dRenderer.Button3dRenderType Button3dRendererType;

        #endregion

        #region Initializer

        public Button95()
        {
            DoubleBuffered = true;
            BackColor = Color.FromArgb(192, 192, 192);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.MouseDownBackColor = BackColor;
            FlatAppearance.MouseOverBackColor = BackColor;
            Button3dRendererType = Button3dRenderer.Button3dRenderType.Normal;
        }

        #endregion

        protected override void OnPaint(PaintEventArgs p)
        {
            base.OnPaint(p);
            new Button3dRenderer(p.Graphics).Render(ClientRectangle, Button3dRendererType);
        }
    }
}