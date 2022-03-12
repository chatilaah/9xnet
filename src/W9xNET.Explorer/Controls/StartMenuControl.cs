using System.Diagnostics;
using W9xNET.Explorer.Forms;
using W9xNET.RcReader.Icons;
using W9xNET.User32.Controls;

namespace W9xNET.Explorer.Controls
{
    internal sealed class StartMenuControl : User32.Controls.ContextMenu
    {
        /// <summary>
        /// A run-time banner that holds the front buffer of the banner.
        /// This bitmap is based on the back-buffer's bitmap.
        /// </summary>
        private Bitmap? _banner;

        /// <summary>
        /// The rect where the run-time banner should be painted on the Start menu control.
        /// </summary>
        private RectangleF _rect = Rectangle.Empty;

        /// <summary>
        /// A reference to the parent control (which is the DesktopHost in that case)
        /// </summary>
        private new DesktopHost Parent => (DesktopHost)base.Parent;

        public new bool Visible
        {
            get => base.Visible;
            set
            {
                if (value)
                {
                    InvalidateRect();
                }
                else
                {
                    Parent.Taskbar.PerformClick();
                }

                base.Visible = value;
            }
        }

        /// <summary>
        /// Initializer for the StartMenu control
        /// </summary>
        public StartMenuControl() : base(true /* Use Large */, 2, W9xNET.Explorer.Program.Instance.RC)
        {
            Padding = new Padding(21, 0, 0, 0);
        }

        protected override bool OnLoadMenu(ref ContextMenuItem item, int order)
        {
            var rc = Shell32.Program.Instance.RC;
            Debug.Assert(rc != null, "Shell32 RC instance is null");

            int id = Convert.ToInt32(item.Name);

            switch (id)
            {
                case 0:
                    switch (order)
                    {
                        case 0: return false;
                        case 1: item.Icon = rc.IconAt(19); return true;
                        case 2: item.Icon = rc.IconAt(20); return true;
                        case 3: item.Icon = rc.IconAt(21); return true;
                    }
                    break;
                case 513:
                    break;
                case 413: // Taskbar
                    item.Icon = new IconInfo(rc.IconAt(39)).Images[0]; return true;
                case 510: // Printers
                    item.Icon = new IconInfo(rc.IconAt(37)).Images[1]; return true;
                case 505: // Control Panel
                    item.Icon = new IconInfo(rc.IconAt(35)).Images[1]; return true;
                case 520: // Find
                    item.Icon = rc.IconAt(22); return true;
                case 503: // Help
                    item.Icon = rc.IconAt(23); return true;
                case 401: // Run
                    item.Icon = rc.IconAt(24); return true;
                case 409: // Suspend
                    item.Icon = rc.IconAt(25); return false;
                case 410: // Eject PC
                    item.Icon = rc.IconAt(26); return false;
                case 506: // Shut Down
                    item.Icon = rc.IconAt(27); return true;
            }

            return true;
        }

        /// <summary>
        /// Prepares coordinates/size and shows the StartMenu on the screen.
        /// </summary>
        private void InvalidateRect()
        {
            // Before popping up the Start menu, make sure to get the proper coordinates set.
            Top = (Parent.ClientRectangle.Height - Parent.Taskbar.Height - Height) + Parent.Taskbar.Padding.Top;
            Left = Parent.Taskbar.Padding.Left;
        }

        /// <summary>
        /// Creates a banner based on a source image.
        /// </summary>
        /// <param name="source">The bitmap to be painted on the banner</param>
        /// <returns>The new banner for the Start menu to be used</returns>
        private Bitmap Bannerize(Bitmap source)
        {
            // Create a new template for the bannner.
            var banner = new Bitmap(source.Width, ClientSize.Height - (BorderThickness * 2));

            // Get a handle to the graphics drawing of the bitmap.
            using var gr = Graphics.FromImage(banner);

            // Clear the entire bitmap based on soruce's key color (located at x=0, y=0)
            gr.Clear(source.GetPixel(0, 0));

            // Prepare the rect of the original bitmap to be painted on the new bitmap (banner).
            RectangleF rect = new(0, banner.Height - source.Height,
                source.Width, source.Height);

            // Do it.
            gr.DrawImage(source, rect);

            return banner;
        }

        /// <summary>
        /// Shows or hides the StartMenu
        /// </summary>
        /// <param name="value">True to show. Otherwise, False to hide</param>
        public new void Show() => InvalidateRect();

        public new void Hide() => Visible = false;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Do we have a runtime banner reference?
            if (_banner == null)
            {
                /// A back-buffer banner that contains the main bitmap of the banner
                Bitmap bbBanner = Properties.Resources.Banner;
                if (Explorer.Properties.Settings.Default.IsRedmondBanner)
                {
                    var explorerRc = Explorer.Program.Instance.RC;
                    if (explorerRc == null)
                    {
                        throw new NullReferenceException("explorerRc is null");
                    }

                    bbBanner = explorerRc.BmpAt(8);
                }

                // Make a banner for us please.
                _banner = Bannerize(bbBanner);

                // Get the rect data so that we can know how and where to paint the final bitmap.
                _rect = new(Parent.Taskbar.Padding.Left + 1, BorderThickness, Padding.Left, _banner.Height);
            }

            e.Graphics.DrawImage(_banner, _rect);
        }
    }
}