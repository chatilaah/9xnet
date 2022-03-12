using LegalMitigation.Properties;
using LegalMitigation.Utils;

namespace LegalMitigation.Forms
{
    internal partial class FrmLoadSource : Form
    {
        #region Properties

        readonly BlessTool _bt = new();
        readonly W9xNETApp[] m_apps;

        #endregion

        #region Constructor(s)

        public FrmLoadSource()
        {
            InitializeComponent();

            if (!DesignMode)
            {
                throw new System.NotSupportedException();
            }
        }

        public FrmLoadSource(W9xNETApp[] apps)
        {
            InitializeComponent();

            m_apps = apps;

            //
            // label1
            //
            label1.Text = Resources.PleaseInsertYourWindows95CdRomToContinue;

            //
            // this
            //
            ResetStatus();

            //
            // BlessTool
            //
            _bt.OnSearchingSource += BlessTool_OnSearchingSource;
            _bt.OnCheckingDrives += BlessTool_OnCheckingDrives;
            _bt.OnDrivesDetected += BlessTool_OnDrivesDetected;
            _bt.OnSelectWinSource += BTool_OnSelectWinSource;
        }

        #endregion

        void EnableControls(bool enabled = true)
        {
            btnRetry.Enabled = enabled;
            btnCancel.Visible = enabled;
        }

        void UpdateStatus(string message)
        {
            pictureBox1.Hide();
            label1.Text = message;
            EnableControls(false);
        }

        void ResetStatus()
        {
            Text = string.Format(Resources.ProductSetup, Resources.AppName);
            Icon = SystemIcons.WinLogo;
            pictureBox1.Image = SystemIcons.Exclamation.ToBitmap();
            pictureBox1.Show();
            label1.Text = Resources.PleaseInsertYourWindows95CdRomToContinue;
            progressBar1.Value = 0;
            progressBar1.Hide();
            EnableControls(true);
        }

        void SetErrorState(string message)
        {
            ResetStatus();

            MessageBox.Show(message, Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void BlessTool_OnCheckingDrives()
        {
            UpdateStatus("Checking for drives, please wait...");
        }

        private void BlessTool_OnDrivesDetected(IEnumerable<DriveInfo> drives)
        {
            int count = 0;
            foreach (var i in drives)
            {
                UpdateStatus($"Found drive {i.Name}");
                count += 1;
            }

            if (count == 0)
            {
                SetErrorState("Did not find any drive on the system!");
            }
            else
            {
                UpdateStatus($"Detected total drives: {count}");
            }
        }

        private void BlessTool_OnSearchingSource(string path)
        {
            UpdateStatus($"Searching source from \"{path}\"...");
        }

        private int BTool_OnSelectWinSource(IEnumerable<WinSource> sources)
        {
            var frmPicker = new FrmPickSource
            {
                Sources = sources
            };

            if (frmPicker.ShowDialog() == DialogResult.OK)
            {
                return frmPicker.SelectedIndex;
            }

            return -1;
        }

        private void btnRetry_Click(object sender, System.EventArgs e)
        {
            if (_bt.Resolve(m_apps) == Types.BlessToolError.Success)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Could not find the installation media!", Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetStatus();
            }
        }

        private void FrmLoadSource_Load(object sender, System.EventArgs e)
        {

        }
    }
}