using System.Diagnostics;
using System.Drawing.Drawing2D;
using W9xNET.User32.Controls.Interfaces;

namespace W9xNET.User32.Controls
{
    /// <summary>
    /// A desktop container control that is intended to be in use with the DesktopHost form.
    /// </summary>
    public class IconContainer<T> : NxnControl where T : Control
    {
        public delegate void OnItemClickedEventHandler(object? sender, MouseEventArgs e, IconElement icon);
        public delegate void OnItemDoubleClickedEventHandler(object? sender, IconElement icon);

        public event OnItemClickedEventHandler? OnItemClicked;
        public event OnItemDoubleClickedEventHandler? OnItemDoubleClicked;
        public List<IconElement> Items { get; private set; } = new();

        readonly Pen SelectionPen = new(new SolidBrush(Color.FromArgb(255, 127, 127)))
        {
            DashStyle = DashStyle.Dot
        };

        IconElement? _selectedIcon;
        bool _mouseDown = false;
        bool _secondaryClick = false;
        Point _mouseDownPoint = Point.Empty;
        Point _mousePoint = Point.Empty;

        protected new T Parent => (T)base.Parent;

        public IconContainer() => DoubleBuffered = true;

        /// <summary>
        /// An event that triggers when the control's focus is lost.
        /// </summary>
        public void Unfocus()
        {
            OnHideContextMenu();

            foreach (var icon in Items)
            {
                icon.Unselect();
            }

            Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            //if (_selectedIcon != null && _secondaryClick)
            //{
            //    Debug.WriteLine($"Handle right click for item {_selectedIcon.Caption.Text}");
            //}
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);

            if (_selectedIcon != null)
            {
                if (OnItemDoubleClicked == null)
                {
                    ItemDoubleClicked(_selectedIcon);
                }
                else
                {
                    OnItemDoubleClicked.Invoke(this, _selectedIcon);
                }
            }
        }

        /// <summary>
        /// An event that triggers when the mouse button is released.
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            _secondaryClick = false;
            _selectedIcon?.OnMouseUp(e);

            if (OnItemClicked == null)
            {
                ItemClicked(e, _selectedIcon!);
            }
            else
            {
                OnItemClicked.Invoke(this, e, _selectedIcon!);
            }

            Cursor.Clip = Rectangle.Empty;
            _mouseDown = false;

            Invalidate();
        }

        protected virtual void ItemDoubleClicked(IconElement icon)
        {
            // do nothing
        }

        protected virtual void ItemClicked(MouseEventArgs e, IconElement icon)
        {
            if (e.Button == MouseButtons.Right)
            {
                //var oldCm = ContextMenu;

                ////TODO: Set the new one
                //ContextMenu = null;

                //// Show it
                //OnShowContextMenu();

                //// Revert back to the original
                //ContextMenu = oldCm;
            }
        }

        /// <summary>
        /// An event that triggers when the mouse button is pressed down, but not released yet.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            ((INxnForm)Parent).OnRequestFocus(this);

            _mouseDown = true;
            _mousePoint = _mouseDownPoint = e.Location;
            _secondaryClick = e.Button == MouseButtons.Right;
            _selectedIcon = null;

            for (int i = 0; i < Items.Count; i++)
            {
                var icon = Items[i];

                bool isX = e.X >= icon.Left && e.X <= icon.Right;
                bool isY = e.Y >= icon.Top && e.Y <= icon.Bottom;

                if (!isX || !isY) continue;

                var old = Items[^1];
                Items[^1] = icon;
                Items[i] = old;

                _selectedIcon = icon;
                icon.OnMouseDown(e);

                break;
            }

            Invalidate();

            ((INxnForm)Parent).LockCursor();
        }

        /// <summary>
        /// Handles the mouse move event.
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!_mouseDown) return;

            _mousePoint = e.Location;

            _selectedIcon?.OnMouseMove(e);

            Invalidate();
        }

        /// <summary>
        /// Re-aligns the desktop icons to their default locations.
        /// </summary>
        public void ArrangeIcons()
        {
            const int padTop = 2;
            const int padLeft = 2;

            var nextTop = padTop;
            foreach (var i in Items)
            {
                i.Top += nextTop;
                i.Left = padLeft + (int)((IconElement.MaxSize.Width * 0.5) - (i.Width * 0.5));

                nextTop += padTop + IconElement.MaxSize.Height;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (var i in Items)
            {
                i.OnPaint(e);
            }

            if (_mouseDown && _selectedIcon == null)
            {
                e.Graphics.DrawRectangle(SelectionPen,
                    Math.Min(_mouseDownPoint.X, _mousePoint.X),
                    Math.Min(_mouseDownPoint.Y, _mousePoint.Y),
                    Math.Abs(_mouseDownPoint.X - _mousePoint.X),
                    Math.Abs(_mouseDownPoint.Y - _mousePoint.Y));
            }
        }
    }
}