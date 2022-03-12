using System.Drawing;
using System.Windows.Forms;
using W9xNET.User32.Interfaces;

namespace W9xNET.User32.Renderers
{
    [Obsolete("An Eternal95 stub")]
    public class Separator3dBorderRenderer : IRenderer
    {
        #region Properties

        private Control ControlHandle { get; set; }

        PictureBox?
            TopLine1,
            TopLine2;

        #endregion

        #region Initializer

        public Separator3dBorderRenderer(Control control)
        {
            ControlHandle = control;
        }

        #endregion

        #region Functions

        public void Render()
        {
            TopLine1 = new PictureBox
            {
                Location = new Point(0, 0),
                Size = new Size(ControlHandle.Width, 1),
                BackColor = Color.FromArgb(128, 128, 128),
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right
            };

            TopLine2 = new PictureBox
            {
                Location = new Point(0, 1),
                Size = new Size(ControlHandle.Width, 1),
                BackColor = Color.FromArgb(255, 255, 255),
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right
            };

            ControlHandle.Controls.Add(TopLine1);
            ControlHandle.Controls.Add(TopLine2);

            // Total count heights of two lines are 2.
            ControlHandle.Height = 2;
        }

        public void DeRender()
        {
            ControlHandle.Controls.Remove(TopLine1);
            ControlHandle.Controls.Remove(TopLine2);

            TopLine1 = null;
            TopLine2 = null;
        }

        public void HandleMouseDown()
        {

        }

        public void HandleMouseUp()
        {

        }

        #endregion
    }
}