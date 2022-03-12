using System.Diagnostics;

namespace W9xNET.Shell32.Forms
{
    public partial class FrmAbout : Form
    {
        private const int PadLeft = 16;
        private const int PadTop = 96;

        #region Constructor(s)

        public FrmAbout()
        {
            InitializeComponent();
        }

        public FrmAbout(Form parent)
        {
            InitializeComponent();

            StartPosition = FormStartPosition.Manual;

            if (parent.Left > 0)
            {
                Left = Math.Abs(parent.Left);

                if (Left - PadLeft > 0)
                {
                    Left += PadLeft;
                }
            }

            Top = parent.Top + PadTop;
        }

        #endregion

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void llGithubLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(llGithubLink.Text);
        }

        private void UpdateAvailableSystemMemory()
        {
            //label4.Text = new HardwareInfo().TotalPhysicalMemory.ToString() + " Bytes";
        }

        private void FrmAbout_Load(object sender, EventArgs e)
        {
            UpdateAvailableSystemMemory();
        }
    }
}