using System.Diagnostics;
using System.Drawing.Drawing2D;
using W9xNET.Explorer.Forms;
using W9xNET.Explorer.Models;
using W9xNET.User32.Controls;

namespace W9xNET.Explorer.Controls
{
    internal sealed class StartButtonControl : NxnControl
    {
        /// <summary>
        /// An event that triggers a paint event for the specified state.
        /// </summary>
        private delegate void OnCustomPaintEventHandler(PaintEventArgs e);

        /// <summary>
        /// The padding of the Start button control
        /// </summary>
        public new Padding Padding { get => base.Padding; private set => base.Padding = value; }

        private RtFaceData? _rtFace;
        private bool _isPressed = false;

        private new ShellTrayWnd? Parent => (ShellTrayWnd)base.Parent;

        /// <summary>
        /// A paint handler for the two states of the Start button.
        /// One for the button up state, and the second for the button down state.
        /// </summary>
        private readonly Dictionary<bool, OnCustomPaintEventHandler> _handler;

        /// <summary>
        /// Determines whether the Start button is currently pressed or not.
        /// </summary>
        public bool IsPressed
        {
            get => _isPressed;
            private set
            {
                Debug.Assert(Parent != null, "Parent control is null in StartButtonControl");

                ((DesktopHost)GetForm()).Startmenu.Visible = (_isPressed = value);
                Invalidate();
            }
        }

        public StartButtonControl()
        {
            _handler = new Dictionary<bool, OnCustomPaintEventHandler>
            {
                { true, RenderDown },
                { false, RenderNormal }
            };

            Padding = new(3);

            Width = 54;
            Height = ShellTrayWnd.DefaultChildHeight;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            //TODO: Refer to the appropriate rc dll.
            ContextMenu = new ContextMenu();
            ContextMenu.Items.Add("Open");
            ContextMenu.Items.Add("Explore");
            ContextMenu.Items.Add("Find...");
            ContextMenu.Reload();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                IsPressed = !IsPressed;
            }
        }

        /// <summary>
        /// Removes focus from the control.
        /// This does NOT remove the Start menu from the screen.
        /// </summary>
        public void Unfocus()
        {
            // Avoid unnecessary repaint.
            if (IsPressed)
            {
                IsPressed = false;
            }
        }

        /// <summary>
        /// Adds focus to the control.
        /// </summary>
        public new void Focus()
        {
            Debug.Assert(_rtFace != null, "_rtFace is null");

            using var p = new Pen(Color.Black);
            p.DashStyle = DashStyle.Dot;

            var rf = _rtFace.Value;
            Graphics.FromHwnd(Handle).DrawRectangle(p,
                rf.X, rf.Y,
                rf.Width, rf.Height);
        }

        /// <summary>
        /// Renders the down state of the Start button.
        /// </summary>
        private void RenderDown(PaintEventArgs e)
        {
            Debug.Assert(_rtFace != null, "_rtFace is null");

            var rf = _rtFace.Value;
            e.Graphics.DrawImage(rf.Image, rf.X + 1, rf.Y + 1);

            this.Focus();

            GraphicsPath path = new();

            using (Pen p = new(Color.Gray))
            {
                path.Reset();

                path.AddLine(x1: 1,
                             x2: Width - 3,
                             y1: 1,
                             y2: 1);

                path.AddLine(x1: 1,
                             x2: 1,
                             y1: 1,
                             y2: Height - 3);

                e.Graphics.DrawPath(p, path);
            }

            CommonRender(e, true, ref path);
        }

        /// <summary>
        /// Renders the normal state of the Start button.
        /// </summary>
        private void RenderNormal(PaintEventArgs e)
        {
            Debug.Assert(_rtFace != null, "_rtFace is null");
            var rt = _rtFace.Value;

            e.Graphics.DrawImage(rt.Image, rt.X, rt.Y);

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

            CommonRender(e, false, ref path);
        }

        private void CommonRender(PaintEventArgs e, bool isDown, ref GraphicsPath path)
        {
            using (Pen p = new(isDown ? Color.Black : Color.White))
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

            using (Pen p = new(isDown ? Color.White : Color.Black))
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

        /// <summary>
        /// Gets the bitmap face of the start button, which includes the icon that is retrieved from the resource, and a text next to it.
        /// </summary>
        /// <param name="x">X-axis position where the bitmap should be located during the paint event.</param>
        /// <param name="y">Y-axis position where the bitmap should be located during the paint event.</param>
        /// <returns>A bitmap to be displayed on the Start button's face</returns>
        /// <exception cref="NullReferenceException">Throws when the resource reference is null.</exception>
        /// <exception cref="FileNotFoundException">Throws when the resource was not found in the DLL or EXE</exception>
        private RtFaceData GenerateFaceData(PaintEventArgs e)
        {
            var explorerRc = Explorer.Program.Instance.RC;
            Debug.Assert(explorerRc != null, "explorerRc is null");

            var icon = explorerRc.BmpAt(0);
            if (icon == null)
            {
                throw new FileNotFoundException("Failed to locate the bitmap of the Start icon");
            }

            
            const string caption = "Start";
            var font = User32.Properties.Settings.Default.ActiveTitleBar_Font;
            var captionSize = e.Graphics.MeasureString(caption, font);

            RtFaceData rtFace = new(icon.Width + (int)captionSize.Width + 2, icon.Height);

            // Get a handle to the graphics drawing of the bitmap.
            using var gr = Graphics.FromImage(rtFace.Image);

            // Set the text rendering hit the same as the base one
            gr.TextRenderingHint = e.Graphics.TextRenderingHint;
            
            // Clear the entire bitmap
            gr.Clear((Color)User32.Properties.Settings.Default.ThreeDObjects_ItemColor);
            gr.DrawImage(icon, 0, (int)((rtFace.Height * 0.5) - (icon.Height * 0.5)));


            gr.DrawString(caption, (Font)font,
                new SolidBrush((Color)User32.Properties.Settings.Default.ThreeDObjects_FontColor), icon.Width, (int)((rtFace.Height * 0.5) - (captionSize.Height * 0.5)));

            rtFace.X = 5;
            rtFace.Y = (int)((ClientRectangle.Height * 0.5) - (rtFace.Height * 0.5));

            Width = rtFace.Width + (rtFace.X * 2);
            Invalidate();

            return rtFace;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_rtFace == null)
            {
                _rtFace = GenerateFaceData(e);
            }

            _handler[IsPressed](e);
        }
    }
}