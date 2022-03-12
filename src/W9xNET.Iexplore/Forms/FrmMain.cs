using System.Drawing;
using System.Windows.Forms;
using W9xNET.Iexplore.Controls;

namespace W9xNET.Iexplore.Forms
{
    public partial class FrmMain : Form
    {
        #region Properties

        ToolStripSpringComboBox toolStripSpringTextBox1 = new ToolStripSpringComboBox();

        #endregion

        #region Constructor(s)

        public FrmMain()
        {
            InitializeComponent();

            //
            // toolStrip3
            //
            toolStrip3.Items.Add(toolStripSpringTextBox1);

            //
            // toolStripSpringTextBox1
            //
            toolStripSpringTextBox1.KeyDown += ToolStripSpringTextBox1_KeyDown;

            this.Icon = SystemIcons.WinLogo;

            //
            // webView
            //
            InitializeAsync();
        }

        #endregion

        async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);
        }

        private void ToolStripSpringTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                webView.CoreWebView2.Navigate(toolStripSpringTextBox1.Text);
            }
        }
    }
}