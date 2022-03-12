using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using W9xNET.WinCom.Interfaces;
using W9xNET.WinCom.Renderers;

namespace W9xNET.WinCom.Forms
{
    [DesignerCategory("")]
    internal partial class FrmBugCheck : Form, IBugCheckWindow
    {
        #region Properties

        private readonly BugCheckRenderer _renderer;

        public string Message { get; set; }
        
        Color IBugCheckWindow.BackgroundColor { get => BackColor; set => BackColor = value; }
        
        Color IBugCheckWindow.TextColor { get; set; }

        #endregion

        #region Constructor(s)

        public FrmBugCheck()
        {
            InitializeComponent();
#if DEBUG
            TopMost = false;
#else
            Cursor.Hide();
#endif

            _renderer = new Win9xBugCheckRenderer(this);

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            Size = new Size(Width, Height);
            MaximizeBox = false;
        }

#endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
#if !DEBUG
            System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            _renderer.Render(e);
        }

        private void FrmBugCheck_KeyDown(object sender, KeyEventArgs e)
        {
            Close();
        }
    }
}