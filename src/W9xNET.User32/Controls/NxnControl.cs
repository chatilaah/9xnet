using System.Drawing.Drawing2D;
using System.Drawing.Text;
using W9xNET.User32.Controls.Interfaces;
using W9xNET.User32.Properties;

namespace W9xNET.User32.Controls
{
    public class NxnControl : System.Windows.Forms.Control
    {
        public ContextMenu? ContextMenu { get; set; }

        /// <summary>
        /// An event that triggers when the user clicks on the blank area.
        /// </summary>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            switch (e.Button)
            {
                case MouseButtons.Right:
                    OnShowContextMenu(/*e.Location*/);
                    break;

                case MouseButtons.Left:
                    break;
            }
        }

        /// <summary>
        /// Hides the context menu from the view.
        /// </summary>
        protected virtual void OnHideContextMenu() => Parent.Controls.Remove(ContextMenu);
        
        /// <summary>
        /// Retrieves the Form of the residing control
        /// </summary>
        /// <returns>The form where the control is located</returns>
        /// <exception cref="NullReferenceException">An exception is thrown if one or more parent(s) are null</exception>
        protected virtual Form GetForm()
        {
            Control topLevel = this;
            while (topLevel is not Form)
            {
                if (topLevel.Parent == null)
                {
                    throw new NullReferenceException("Could not find the parent of the NxnControl");
                }

                topLevel = topLevel.Parent;
            }

            return (Form)topLevel;
        }

        /// <summary>
        /// Shows the context menu on the blank area.
        /// </summary>
        /// <param name="location">Starting location where the context menu is supposed to appear.</param>
        protected virtual void OnShowContextMenu()
        {
            var topLevel = GetForm();

            ((INxnForm)topLevel).OnRequestFocus(this);

            Point location = topLevel.PointToClient(Cursor.Position);

            ContextMenu?.PointToArea(topLevel.ClientSize, null, location);
            topLevel.Controls.Add(ContextMenu);
            ContextMenu?.Show();
        }

        internal static void InternalSetUserGraphics(Graphics gr)
        {
            if (!Settings.Default.IsClearType)
            {
                gr.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                gr.SmoothingMode = SmoothingMode.HighSpeed;
                gr.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            InternalSetUserGraphics(e.Graphics);
        }

        /// <summary>
        /// Don't use this control!
        /// </summary>
        public override ContextMenuStrip ContextMenuStrip
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }
    }
}