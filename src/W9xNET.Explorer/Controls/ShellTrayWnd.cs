using System.Diagnostics;
using W9xNET.Explorer.Forms;
using W9xNET.User32.Controls;
using W9xNET.User32.Controls.Interfaces;

namespace W9xNET.Explorer.Controls
{
    /// <summary>
    /// Comprehensive taskbar control that keeps track of open windows, holds a Start button, and a SysTray control.
    /// </summary>
    internal sealed class ShellTrayWnd : NxnControl
    {
        /// <summary>
        /// A reference to the StartButtonControl object.
        /// </summary>
        readonly StartButtonControl StartButton = new();

        /// <summary>
        /// A reference to the SysTrayControl object.
        /// </summary>
        internal readonly TrayNotifyWnd Tray = new();

        /// <summary>
        /// A reference to the Tasks object for showing open programs/apps.
        /// </summary>
        internal readonly MSTaskSwWClass Tasks = new();

        /// <summary>
        /// The default height of the Taskbar control.
        /// </summary>
        public const int DefaultHeight = 28;
        public const int DefaultChildHeight = 22;

        private int IncrementedHeight = 0;

        /// <summary>
        /// Simulates a mouse click on the control.
        /// </summary>
        public void PerformClick()
        {
            BringToFront();
            StartButton.Unfocus();
        }

        public new Padding Padding
        {
            get => base.Padding;
            private set { base.Padding = value; }
        }

        /// <summary>
        /// Indicates whether the user is attempting to resize the taskbar's height/width.
        /// </summary>
        public bool IsResizing { get; private set; } = false;

        /// <summary>
        /// Indicates whether the user is attempting to move the taskbar to a different position.
        /// </summary>
        public bool IsMovingPosition { get; private set; } = false;

        public new DesktopHost? Parent
        {
            get => (DesktopHost)base.Parent;
            set => base.Parent = value;
        }

        /// <summary>
        /// Initializes the Taskbar control.
        /// </summary>
        public ShellTrayWnd()
        {
            Height = DefaultHeight;
            Padding = new Padding(2, 4, 2, 0);

            //
            // startButton
            //
            StartButton.Parent = this;

            //
            // tasks
            //
            Tasks.Parent = this;

            //
            // tray
            //
            Tray.Parent = this;

            Controls.AddRange(new Control[] { StartButton, Tasks, Tray });
        }

        protected override void OnShowContextMenu()
        {
            Debug.Assert(ContextMenu != null);
            ContextMenu.Items[0].Visible = false;
            ContextMenu.Reload();

            base.OnShowContextMenu();
        }

        internal int BeginX => StartButton.Location.X + StartButton.Width + 4;

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            ContextMenu = new(false, 3, Explorer.Program.Instance.RC);
            ContextMenu.OnClickedContextMenuItem += ContextMenu_OnClickedContextMenuItem;


            //TODO: HACK, there is an issue with the parser tool. Make sure to fix that in future builds
            ContextMenu.Items.RemoveAt(0);

            // Slip the same context menu into the Systray control ;-)
            Tray.ContextMenu = ContextMenu;
            Tasks.ContextMenu = ContextMenu;
        }

        private void ContextMenu_OnClickedContextMenuItem(object? sender, EventArgs e)
        {
            var cmi = (ContextMenuItem)sender!;

            var dh = GetForm() as DesktopHost;

            Debug.WriteLine($"{cmi.Name}\t{cmi.Text}");

            switch (Convert.ToInt32(cmi.Name))
            {
                case 413: // Properties
                    var frm = new Forms.FrmProperties();
                    dh.Rundll.Run(frm, 5, dh.Container.Height - frm.Height - 4);
                    break;
                case 416: // Undo
                    break;
                case 415: // Minimize All Windows
                    break;
                case 405: // Tile Vertically
                    break;
                case 404: // Tile Horizontally
                    break;
                case 403: // Cascade
                    break;
                case 408: // Adjust Date/Time
                    break;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            //
            // Tray
            //
            Tray.Refresh();
            Tray.BackColor = BackColor;

            //
            // Tasks
            //
            Tasks.Location = new(BeginX, Padding.Top);
            Tasks.Size = new(Width - BeginX - Tray.Width - Padding.Right, Height - (Padding.Top * 2) + 2);
            Tasks.BackColor = BackColor;
            Tasks.Refresh();
        }

        private bool IsCanResize(Point e) => (e.Y >= 0 && e.Y < 5) && !StartButton.IsPressed;

        /// <summary>
        /// Handles the taskbar resize procedure (vertically)
        /// TODO: Need to handle horizontal when placed on the left or right!
        /// </summary>
        /// <param name="p"></param>
        private void InternalHandleResize(Point p)
        {
            BringToFront();

            int y = p.Y;

            if (p.Y < 0) // Are we incrementing?
            {
                y *= -1;

                if (DefaultHeight < y)
                {
                    IncrementHeight();
                }
            }
            else if (p.Y > 0) // Are we decrementing?
            {
                if (DefaultHeight > y)
                {
                    DecrementHeight();
                }
            }

            InvalidateRect();
        }

        /// <summary>
        /// Handles the taskbar movement procedure.
        /// </summary>
        /// <param name="p"></param>
        private void InternalHandleMoving(Point p)
        {
            //TOOD: Implement reposition of the taskbar.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the new location and size for the Taskbar on the screen.
        /// TODO: This eliminates the freedom for the user to manually set the taskbar at different positions,
        /// consider fixing this problem in the future.
        /// </summary>
        internal void InvalidateRect()
        {
            Debug.Assert(Parent != null);

            Width = Parent.ClientSize.Width;
            Top = Parent.ClientSize.Height - Height;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (IsMovingPosition)
            {
                InternalHandleMoving(e.Location);
                return;
            }

            if (IsResizing && !StartButton.IsPressed)
            {
                InternalHandleResize(e.Location);
                return;
            }

            Cursor = IsCanResize(e.Location) ?
                Cursors.SizeNS : Cursors.Default;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            Debug.Assert(Parent != null);

            if (e.Button == MouseButtons.Left)
            {
                PerformClick();

                if ((IsResizing = IsCanResize(e.Location)))
                {
                    ((INxnForm)Parent).OnRequestFocus(this);
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            Debug.Assert(Parent != null);

            if (IsResizing)
            {
                Parent.Container.Invalidate();
                Parent.Container.InvalidateRect();
            }

            IsResizing = false;
            IsMovingPosition = false;
        }

        public new void Refresh()
        {
            base.Refresh();

        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            Cursor = Cursors.Default;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using Pen p = new(Color.White);
            e.Graphics.DrawLine(p,
                x1: 0, x2: Width,
                y1: 1, y2: 1);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Update the Start button control position.
            StartButton.Location = new(Padding.Left, Padding.Top);

            // Update the SysTray control position.
            Tray.Location = new(Width - Tray.Width - Padding.Right, Padding.Top);
        }

        /// <summary>
        /// Increments the height of the Taskbar control.
        /// </summary>
        public void IncrementHeight()
        {
            Debug.Assert(Parent != null);

            if (Height >= (Parent.ClientSize.Height * 0.5)) return;

            Height += (DefaultHeight - MSTaskSwWClass.SpaceBetweenTabs);
            IncrementedHeight++;
        }

        /// <summary>
        /// Decrements the height of the Taskbar control.
        /// </summary>
        public void DecrementHeight()
        {
            if (Height <= DefaultHeight) return;

            Height -= (DefaultHeight - MSTaskSwWClass.SpaceBetweenTabs);
            IncrementedHeight--;
        }
    }
}