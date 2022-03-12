using LegalMitigation;
using System.Diagnostics;
using W9xNET.User32.Controls.Interfaces;

namespace W9xNET.User32.Controls
{
    public class ContextMenu : NxnControl
    {
        public delegate void OnClickedContextMenuItemEventHandler(object? sender, EventArgs e);

        ContextMenu? _activeMenu;

        public bool IsInverted { get; internal set; } = false;

        /// <summary>
        /// Items that contain the menu items to be displayed when visible
        /// </summary>
        public ContextMenuItemCollection Items { get; protected set; } = new();

        /// <summary>
        /// The thichkness of the border frame
        /// </summary>
        protected int BorderThickness { get; set; } = 3;

        /// <summary>
        /// An event that triggers when a ContextMenuItem object is clicked.
        /// </summary>
        public event OnClickedContextMenuItemEventHandler? OnClickedContextMenuItem;

        /// <summary>
        /// Indicates whether the contents are large or not
        /// </summary>
        public readonly bool IsLarge;

        /// <summary>
        /// Gets or sets whether the context menu should be shown or hidden.
        /// </summary>
        public new bool Visible
        {
            get => base.Visible;
            set
            {
                if (base.Visible = value)
                {
                    BringToFront();
                }
                else
                {
                    _activeMenu?.Hide();
                    SendToBack();
                }
            }
        }

        /// <summary>
        /// Shows the context menu
        /// </summary>
        public new void Show() => Visible = true;

        /// <summary>
        /// Hides the context menu
        /// </summary>
        public new void Hide() => Visible = false;

        public int ItemHeightTemplate => IsLarge ?
            ContextMenuItem.LargeMenuHeight :
            ContextMenuItem.SmallMenuHeight;

        public ContextMenu(bool isLarge = false, int menuId = -1, RcObject? rc = null)
        {
            IsLarge = isLarge;
            BackColor = User32.Properties.Settings.Default.ThreeDObjects_ItemColor;

            if (rc != null)
            {
                if (menuId < 0)
                {
                    throw new IndexOutOfRangeException("menuId should be greater than or equal to 0");
                }

                InitRcMenu(this, rc, menuId);
            }
        }

        /// <summary>
        /// Initializes the Win32 menu resource
        /// </summary>
        /// <param name="rc">Instance of the RcObject</param>
        /// <param name="menuId">The menu index</param>
        private static void InitRcMenu(ContextMenu cm, RcObject rc, int menuId)
        {
            var items = rc.MenuAt(menuId);

            int i = -1;

            Stack<ContextMenuItemCollection> popups = new();
            popups.Push(cm.Items);

            int wDepth = 0;
            foreach (var item in items)
            {
                while (item.wDepth < wDepth)
                {
                    --wDepth;
                    popups.Pop();
                }

                var cmi = new ContextMenuItem()
                {
                    Text = item.Text.Replace("&", String.Empty),
                    Name = item.MenuId.ToString()
                };

                ContextMenuItemCollection current = popups.Peek();

                wDepth = item.wDepth;
                if ((item.bResInfo & 0x01) == 1)
                {
                    i++;

                    if (cm.OnLoadMenu(ref cmi, i))
                    {
                        current.Add(cmi);
                        popups.Push(cmi.Items);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(item.Text) || item.MenuId == -1)
                    {
                        current.AddSeparator();
                    }
                    else
                    {
                        if (cm.OnLoadMenu(ref cmi, i))
                        {
                            current.Add(cmi);
                        }
                    }
                }
            }

            while (0 < wDepth)
            {
                --wDepth;
            }

            cm.Reload();
        }

        /// <summary>
        /// A routine that is called upon creation of the menu
        /// </summary>
        /// <param name="item">The item that is about to be created</param>
        /// <param name="order">The order which is currently being processed</param>
        /// <returns>True if the specified menu is to be present on the ContextMenu. False otherwise.</returns>
        protected virtual bool OnLoadMenu(ref ContextMenuItem item, int order) => true;

        internal virtual void OnDisplaySubmenu(ContextMenuItem sender, ContextMenu subContextMenu)
        {
            _activeMenu?.Hide();

            if (sender.Items.Count == 0) return;

            subContextMenu.Visible = false;
            Parent.Controls.Add(subContextMenu);
            subContextMenu.Reload();

            int x = Right - (BorderThickness * 2);

            //TODO: HACK: Figure out why we need to statically set the value 3
            int y = (Top + sender.Top) - 3;

            subContextMenu.PointToArea(Parent.ClientSize, sender.Parent, new Point(x, y));
            subContextMenu.Show();

            _activeMenu = subContextMenu;
        }

        /// <summary>
        /// Measures the appropriate maximum width to be used for the context menu.
        /// </summary>
        /// <returns>The maximum width within the context menu.</returns>
        private float GetApproxMaxWidth()
        {
            float max = 0;
            foreach (var item in Items)
            {
                item.Parent = this;

                if (max < item.ContentWidth)
                {
                    max = item.ContentWidth;
                }
            }

            return max;
        }

        public virtual void Reload()
        {
            Controls.Clear();

            int pX = Padding.Left;
            int pY = 0;

            float maxWidth = GetApproxMaxWidth();

            foreach (var i in Items)
            {
                //TODO: Causes a "Parameter is not valid" exception when showing the Start menu
                //if (!i.Visible) continue;

                i.Parent = this;
                i.Top = pY + BorderThickness;
                i.Left = pX + BorderThickness;

                if (i.GetType() != typeof(SeparatorMenuItem))
                {
                    i.Height = ItemHeightTemplate;
                    i.Click += I_Click;
                }

                pY += i.Height;

                i.Width = (int)maxWidth - (BorderThickness * 2) - Padding.Left;

                Controls.Add(i);
            }

            ClientSize = new Size((int)maxWidth, pY + (BorderThickness * 2));
        }

        bool _alreadyClicked = false;

        private void I_Click(object? sender, EventArgs e)
        {
            if (!_alreadyClicked)
            {
                var cmi = (ContextMenuItem)sender!;

                if (cmi.Items.Count == 0)
                {
                    ((INxnForm)(GetForm())).OnRequestFocus(this);
                    OnClickedContextMenuItem?.Invoke(sender, e);
                }

                _alreadyClicked = true;
            }
            else
            {
                _alreadyClicked = false;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Pen p = new(Color.Black))
            {
                e.Graphics.DrawLine(p,
                    x1: ClientRectangle.Width - 1, x2: ClientRectangle.Width - 1,
                    y1: ClientRectangle.Y, y2: ClientRectangle.Height);

                e.Graphics.DrawLine(p,
                    x1: ClientRectangle.X, x2: ClientRectangle.Width - 1,
                    y1: ClientRectangle.Height - 1, y2: ClientRectangle.Height - 1);
            }

            using (Pen p = new(Color.FromArgb(128, 128, 128)))
            {
                e.Graphics.DrawLine(p,
                    x1: ClientRectangle.Width - 2, x2: ClientRectangle.Width - 2,
                    y1: ClientRectangle.Y + 1, y2: ClientRectangle.Height - 2);

                e.Graphics.DrawLine(p,
                    x1: ClientRectangle.X + 1, x2: ClientRectangle.Width - 2,
                    y1: ClientRectangle.Height - 2, y2: ClientRectangle.Height - 2);
            }

            using (Pen p = new(Color.White))
            {
                e.Graphics.DrawLine(p,
                    x1: ClientRectangle.X + 1, x2: ClientRectangle.Width - 3,
                    y1: ClientRectangle.Y + 1, y2: ClientRectangle.Y + 1);

                e.Graphics.DrawLine(p,
                    x1: ClientRectangle.X + 1, x2: ClientRectangle.X + 1,
                    y1: ClientRectangle.Y + 1, y2: ClientRectangle.Height - 3);
            }
        }
    }

    public static class ContextMenuExtensions
    {
        /// <summary>
        /// Points the specified context menu to the appropriate location that is relative to the client size.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="clientSize"></param>
        /// <param name="e"></param>
        public static void PointToArea(this ContextMenu self, Size clientSize, Control? descendingCtrl, Point e)
        {
            int x = e.X;
            int y = e.Y;

            bool hasDesc = descendingCtrl is ContextMenu;
            self.IsInverted = false;

            if (e.X + self.Width > clientSize.Width)
            {
                x = e.X - self.Width;  //clientSize.Width - self.Width;
                if (hasDesc)
                {
                    x = (descendingCtrl!.Left - self.Width) + 3;
                    self.IsInverted = true;
                }
            }

            if (e.Y + self.Height > clientSize.Height)
            {
                y = clientSize.Height - self.Height; // e.Y - self.Height;
                //if (hasDesc)
                //{
                //    y = e.Y - self.Height;
                //}
            }

            self.Location = new Point(x, y);
        }
    }
}