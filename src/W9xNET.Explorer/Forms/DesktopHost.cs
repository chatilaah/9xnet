using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using W9xNET.Explorer.Controls;
using W9xNET.Explorer.Properties;
using W9xNET.RcReader.Icons;
using W9xNET.Rundll32;
using W9xNET.User32.Controls;
using W9xNET.User32.Controls.Interfaces;
using static W9xNET.Interop.Win32.Dwmapi;

namespace W9xNET.Explorer.Forms
{
    /// <summary>
    /// The main entry point to the 9xNET experience.
    /// </summary>
    [DesignerCategory("")]
    internal partial class DesktopHost : Form, INxnForm
    {
        #region Properties

        System.ComponentModel.IContainer components = null;
        internal readonly RunDll Rundll;
        readonly Bitmap? BmpWatermark;

        bool isUserClose = false;
        bool pendingClose = false;

        /// <summary>
        /// Start menu visibility event lock
        /// </summary>
        bool _smEventLock = false;

        /// <summary>
        /// A reference to the DesktopHost's desktop container control.
        /// </summary>
        public new Progman Container = new();

        /// <summary>
        /// Indicates whether the operating system's cursor is locked or not.
        /// </summary>
        public static bool IsCursorLocked => Cursor.Clip == Rectangle.Empty;

        /// <summary>
        /// A reference to DesktopHost's Taskbar control.
        /// </summary>
        public readonly ShellTrayWnd Taskbar = new();

        /// <summary>
        /// A reference to the DesktopHost's Start menu control.
        /// </summary>
        public readonly StartMenuControl Startmenu = new();

        /// <summary>
        /// List of currently open context menus on the foreground
        /// </summary>
        private readonly List<Control> OpenContextMenus = new();

        /// <summary>
        /// Gets or sets the fullscreen mode.
        /// </summary>
        public bool IsFullscreen
        {
            get => FormBorderStyle == FormBorderStyle.None;
            set
            {
                if (value)
                {
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                }
                else
                {
                    FormBorderStyle = FormBorderStyle.FixedDialog;
                    WindowState = FormWindowState.Normal;
                }
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Initializer for the DesktopHost.
        /// All controls are created and set at this point.
        /// </summary>
        public DesktopHost()
        {
            this.SuspendLayout();

            //
            // Taskbar
            //
            this.Taskbar.Parent = this;
            this.Taskbar.BackColor = User32.Properties.Settings.Default.ThreeDObjects_ItemColor;
            this.Taskbar.Name = "taskBar";
            this.Taskbar.TabIndex = 3;

            //
            // Startmenu
            //
            this.Startmenu.Parent = this;
            this.Startmenu.Visible = false;
            this.Startmenu.Name = "strtMnu";
            this.Startmenu.VisibleChanged += Startmenu_VisibleChanged;
            this.Startmenu.OnClickedContextMenuItem += Startmenu_OnClickedContextMenuItem;
            this.Controls.Add(this.Startmenu);
            this.Startmenu.Reload();

            //
            // Container (DesktopContainer)
            //
            this.Container.Parent = this;
            //this.Container.BackColor = Color.Transparent;
            this.Container.Location = new Point(0, 0);
            this.Container.Visible = true;
            //this.Container.OnItemClicked += Container_OnItemClicked;
            //this.Container.OnItemDoubleClicked += Container_OnItemDoubleClicked;
            this.Container.Name = "dskCont";
            this.Controls.Add(this.Container);

            //
            // Rundll
            //
            Rundll = new RunDll(this, Rundll_OnPathResolverStartup);
            //Rundll.OnFormSpawned += OnFormSpawned;
            //Rundll.OnKeepFormBelowTaskbar += OnKeepFormBelowTaskbar;

            //
            // DesktopHost
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new Font("MS Shell Dlg", 8);
            this.Icon = System.Drawing.SystemIcons.WinLogo;
            this.BackColor = User32.Properties.Settings.Default.Desktop_ItemColor;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Controls.Add(this.Taskbar);
            this.MaximizeBox = false;
            this.KeyPreview = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Name = "desktopHost";
            this.Text = LegalMitigation.Properties.Resources.AppName;
            this.ResumeLayout(false);

            if (Settings.Default.IsDev)
            {
                BmpWatermark = MakeWatermark(Settings.Default.WatermarkDevString);
            }
        }



        private Dictionary<string, RunDll.OnInvokedEventHandler> Rundll_OnPathResolverStartup() => new()
        {
            [PathConstants.ExplorerPath] = Explorer.Program.Instance.OnInvoke,
            [PathConstants.VersionPath] = Shell32.Program.Instance.OnInvoke
        };

        private void Container_OnItemDoubleClicked(object? sender, IconElement icon) => Rundll.Run(icon);

        private void Startmenu_OnClickedContextMenuItem(object? sender, EventArgs e)
        {
            var cmi = ((ContextMenuItem)sender!);
            
            Form frm;

            switch (Convert.ToInt32(cmi.Name))
            {
                case 503: // Help
                    break;
                case 401: // Run
                    frm = new Shell32.Forms.FrmRun();
                    Rundll.Run(frm, 5, Container.Height - frm.Height - 4);
                    break;
                case 506: // Shut Down
                    Close();
                    break;
            }
        }

        private void Startmenu_VisibleChanged(object? sender, EventArgs e)
        {
            _smEventLock = true;
            OnRequestFocus(this);
            _smEventLock = false;
        }

        [Obsolete("Implementation relocated inside the IconContainer class")]
        private void Container_OnItemClicked(object? sender, MouseEventArgs e, User32.Controls.IconElement icon)
        {
            if (e.Button == MouseButtons.Right)
            {
                
            }

            Debug.WriteLine(icon.Caption.Text);

            //Taskbar.Tasks.Items.Add(new IconInfo(icon.Icon).Images[1], icon.Caption.Text);
            //Taskbar.Tasks.Refresh();
        }

        /// <summary>
        /// Dismiss all open context menus (including Start menu)
        /// </summary>
        public void OnRequestFocus(object? sender)
        {
            foreach (Control control in OpenContextMenus)
            {
                Controls.Remove(control);
            }

            Container.Unfocus();

            // Do not hide the Start menu if we're coming from the VisibleChanged event of the Start menu
            if (!_smEventLock)
            {
                Startmenu.Hide();
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (e.Control is User32.Controls.ContextMenu && e.Control is not StartMenuControl)
            {
                OpenContextMenus.Add(e.Control);
            }
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            // Release any cursor lock
            Cursor.Clip = Rectangle.Empty;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResizeBegin(e);

            OnRequestFocus(this);

            Taskbar.InvalidateRect();
            Container.InvalidateRect();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (pendingClose) return;

            if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.Enter)
            {
                IsFullscreen = !IsFullscreen;
            }
        }

        /// <summary>
        /// Generates a watermark bitamp
        /// </summary>
        /// <param name="text"></param>
        /// <returns>A bitmap of the watermark</returns>
        private Bitmap MakeWatermark(string text)
        {
            var e = Graphics.FromHwnd(Handle);

            var font = Settings.Default.WatermarkFont;
            var size = e.MeasureString(text, font);
            var shadowColor = new SolidBrush(Color.Black);
            var textColor = new SolidBrush(Color.White);

            var bitmap = new Bitmap((int)Math.Ceiling(size.Width) + 1, (int)Math.Ceiling(size.Height));
            using (var g = Graphics.FromImage(bitmap))
            {
                g.DrawString(text, font, shadowColor, 1, 1);
                g.DrawString(text, font, textColor, 0, 0);
            }

            return bitmap;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            pendingClose = true;

            Taskbar.Refresh();

            if (!(e.Cancel = !isUserClose))
            {
                return;
            }

            var dim = new Panel
            {
                BackColor = Color.Transparent,
                Size = ClientSize,
                Location = new(0, 0),
                Visible = true
            };

            bool dontDraw = false;
            dim.Paint += delegate (object? sender, PaintEventArgs e)
            {
                if (dontDraw) return;

                Pen pen = new(new SolidBrush(Color.Black))
                {
                    DashStyle = DashStyle.Dot
                };

                for (int y = 0; y < ClientSize.Height; y++)
                {
                    int xStart = y % 2;

                    e.Graphics.DrawLine(pen,
                        x1: xStart, y1: y,
                        x2: ClientSize.Width + xStart, y2: y);
                }

                dontDraw = true;
            };

            Controls.Add(dim);
            dim.BringToFront();

            var r = MessageBox.Show("Do you really want to exit to Windows?", "Exit to Windows", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                isUserClose = true;
                Close();
            }

            Controls.Remove(dim);
            pendingClose = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //
            // Draw the watermark if available
            //
            if (BmpWatermark != null)
            {
                var xRight = ClientSize.Width - BmpWatermark.Width;
                var yBottom = ClientSize.Height - Taskbar.Height - BmpWatermark.Height;

                e.Graphics.DrawImage(BmpWatermark, 0, 0);
                e.Graphics.DrawImage(BmpWatermark, xRight, 0);

                // To avoid grpahical anomalies on resize, lets not paint the watermark at the bottom corner(s)
                if (!Taskbar.IsResizing)
                {
                    e.Graphics.DrawImage(BmpWatermark, 0, yBottom);
                    e.Graphics.DrawImage(BmpWatermark, xRight, yBottom);
                }
            }
        }

        /// <summary>
        /// Registers a cursor lock.
        /// </summary>
        public void LockCursor()
        {
            int x = 0;
            int y = 0;

            if (!IsFullscreen)
            {
                var rect = GetWindowRectangle(Handle);

                x = rect.Right - ClientSize.Width;
                y = rect.Bottom - ClientSize.Height;
            }

            Cursor.Clip = new Rectangle(
                x, y,
                ClientSize.Width, ClientSize.Height);
        }

        private static RECT GetWindowRectangle(IntPtr hWnd)
        {
            int size = Marshal.SizeOf(typeof(RECT));
            DwmGetWindowAttribute(hWnd, (int)DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out RECT rect, size);

            return rect;
        }

        public Form GetForm() => this;

        public Control GetContainer() => Container;
    }
}