using System.Drawing;
using System.Windows.Forms;
using W9xNET.User32.Renderers.Win95;

namespace W9xNET.User32.Controls.Win95
{
    #region --- Tab Control Button Class ---

    internal class TabControlButton95 : Button95
    {
        #region Properties

        private TabPage95 _tabPageHandle;

        public bool IsHighlighted { private set; get; }

        internal TabPage95 TabPageHandle
        {
            get { return _tabPageHandle; }
            set
            {
                if (_tabPageHandle == null)
                    _tabPageHandle = value;

                _tabPageHandle = value;

                Text = _tabPageHandle.Text;
            }
        }

        #endregion

        #region Initializer

        public TabControlButton95()
        {
            AutoSize = true;
            IsHighlighted = false;
            Height = 20;
            Padding = new Padding(0, 0, 0, 1);
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Button3dRendererType = Button3dRenderer.Button3dRenderType.Normal;
        }

        #endregion

        #region Functions

        protected override void OnPaint(PaintEventArgs p)
        {
            base.OnPaint(p);

            if (IsHighlighted)
            {
                var np = new Pen(Brushes.Black)
                {
                    DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
                };

                p.Graphics.DrawRectangle(np, 3, 3, Width - 6, Height - 6);

                using (var pen = new Pen(Color.FromArgb(192, 192, 192)))
                {
                    p.Graphics.FillRectangle(pen.Brush,
                        x: 0, y: 0,
                        width: 1, height: 1);

                    p.Graphics.FillRectangle(pen.Brush,
                        x: 2, y: ClientRectangle.Height - 1,
                        width: ClientRectangle.Width, height: 1);

                    p.Graphics.FillRectangle(pen.Brush,
                        x: 2, y: ClientRectangle.Height - 2,
                        width: ClientRectangle.Width, height: 1);

                    p.Graphics.FillRectangle(pen.Brush,
                        x: ClientRectangle.Width - 1, y: 1,
                        width: 1, height: 1);

                    p.Graphics.FillRectangle(pen.Brush,
                        x: ClientRectangle.Width - 2, y: 0,
                        width: 1, height: 1);

                    p.Graphics.FillRectangle(pen.Brush,
                        x: ClientRectangle.Width - 1, y: 0,
                        width: 1, height: 1);
                }

                using (var pen = new Pen(Color.FromArgb(223, 223, 223)))
                {
                    p.Graphics.FillRectangle(pen.Brush,
                        x: 1, y: 2,
                        width: 1, height: ClientRectangle.Height);
                }

                p.Graphics.FillRectangle(Brushes.Black,
                    x: ClientRectangle.Width - 2, y: 1,
                    width: 1, height: 1);

                p.Graphics.FillRectangle(Brushes.Black,
                    x: ClientRectangle.Width - 1, y: 2,
                    width: 1, height: ClientRectangle.Height);

                p.Graphics.FillRectangle(Brushes.Gray,
                    x: ClientRectangle.Width - 2, y: 2,
                    width: 1, height: ClientRectangle.Height);

                p.Graphics.FillRectangle(Brushes.White,
                    x: 0, y: ClientRectangle.Height - 1,
                    width: 1, height: 1);

                _tabPageHandle.Invalidate();
            }
            else
            {

                using (var pen = new Pen(Color.FromArgb(192, 192, 192)))
                {
                    p.Graphics.FillRectangle(pen.Brush,
                        x: 0, y: 0,
                        width: 1, height: 1);

                    p.Graphics.FillRectangle(pen.Brush,
                        x: 0, y: 1,
                        width: 1, height: 1);

                    p.Graphics.FillRectangle(pen.Brush,
                    x: 1, y: 0,
                    width: 1, height: 1);

                    p.Graphics.FillRectangle(pen.Brush,
                        x: ClientRectangle.Width - 1, y: 0,
                        width: 1, height: 1);

                    p.Graphics.FillRectangle(pen.Brush,
                        x: ClientRectangle.Width - 1, y: 1,
                        width: 1, height: 1);

                    p.Graphics.FillRectangle(pen.Brush,
                        x: ClientRectangle.Width - 2, y: 0,
                        width: 1, height: 1);
                }

                p.Graphics.FillRectangle(Brushes.White,
                        x: 1, y: 1,
                        width: 1, height: 1);

                p.Graphics.FillRectangle(Brushes.Black,
                    x: ClientRectangle.Width - 2, y: 1,
                    width: 1, height: 1);
            }
        }

        public void SetHighlight(bool highlight)
        {
            IsHighlighted = highlight;
            Update();

            if (highlight)
            {
                Location = new Point(Location.X - 2, 0);
                Height += 2;
                BringToFront();
            }
            else
            {
                Location = new Point(Location.X + 2, 2);
                Height -= 2;
                SendToBack();
            }
        }

        #endregion
    }

    #endregion

    #region --- Tab Page Class ---

    public class TabPage95 : UserControl
    {
        #region Properties

        public string TabTitle { get; set; }

        public bool IsLayoutArranged { get; set; }

        #endregion

        #region Initializer

        public TabPage95()
        {
            IsLayoutArranged = false;
        }

        #endregion
    }

    #endregion

    #region --- Tab Control Class ---

    [Obsolete("An Eternal95 stub")]
    public class TabControl95 : UserControl
    {
        #region Properties

        public int InitialTabPageIndex { get; set; }

        private TabControlButton95? CurrentTabControlButton;

        private const int OffsetLeft = 0;

        private Panel? TabContent;

        public List<TabPage95> TabPages { get; private set; } = new();

        private int OffsetTabContent
        {
            get
            {
                foreach (var control in Controls)
                {
                    if (!control.GetType().Equals(typeof(TabControlButton95)))
                    {
                        continue;
                    }

                    return ((TabControlButton95)control).Height;
                }
                return 0;
            }
        }

        #endregion

        #region Initializer

        public TabControl95()
        {
            DoubleBuffered = true;
            BackColor = Color.FromArgb(192, 192, 192);

            TabContent = new Panel();

            TabContent.Paint += TabContent_Paint;
            Load += Win9xTabControl_Load;
        }

        #endregion

        #region Event Handlers

        private void TabContent_Paint(object? sender, PaintEventArgs e)
        {
            new Button3dRenderer(e.Graphics).Render(TabContent.ClientRectangle, Button3dRenderer.Button3dRenderType.Normal);
        }

        private void Win9xTabControl_Load(object? sender, System.EventArgs e)
        {
            UpdateTabControl();
        }

        #endregion

        #region Functions

        private void UpdateTabControl()
        {
            if (TabPages == null)
            {
                return;
            }

            Controls.Clear();

            var posX = 0;
            var posY = 2;

            var index = 0;

            // ===================================================
            // Set up tab control buttons...
            // ===================================================

            foreach (var tabPage in TabPages)
            {
                if (posX <= 0)
                    posX += OffsetLeft;

                var tcb = new TabControlButton95
                {
                    Location = new Point(posX, posY),
                    TabPageHandle = tabPage
                };

                tabPage.BackColor = BackColor;

                tcb.Text = tabPage.TabTitle;

                tcb.MouseDown += delegate
                {
                    if (tcb.Equals(CurrentTabControlButton))
                        return;

                    if (CurrentTabControlButton != null)
                        CurrentTabControlButton.SetHighlight(false);

                    CurrentTabControlButton = tcb;
                    tcb.SetHighlight(true);
                    ShowTabPage(tabPage);
                };

                Controls.Add(tcb);
                posX += tcb.Width;

                if (index == InitialTabPageIndex)
                {
                    CurrentTabControlButton = tcb;
                }

                tcb.SendToBack();

                index++;
            }

            // ===================================================
            // Set up tab control size...
            // ===================================================

            TabContent.Size = new Size(Width, Height - OffsetTabContent);
            TabContent.Location = new Point(0, OffsetTabContent);
            Controls.Add(TabContent);
            TabContent.BringToFront();

            ShowTabPage(TabPages[InitialTabPageIndex]);
        }

        private void ShowTabPage(TabPage95 tabPage)
        {
            if (!tabPage.IsLayoutArranged)
            {
                tabPage.Size = new Size(TabContent.Width - 3 - 3, TabContent.Height - 3 - 3);
                tabPage.Location = new Point(3, 3);
                tabPage.IsLayoutArranged = true;
                tabPage.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            }

            TabContent.Controls.Clear();
            TabContent.Controls.Add(tabPage);
        }

        #endregion
    }

    #endregion
}