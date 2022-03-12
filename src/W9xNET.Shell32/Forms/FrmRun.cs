using LegalMitigation.Forms;
using System.Diagnostics;
using System.Text;
using static W9xNET.Interop.Win32.User32;

namespace W9xNET.Shell32.Forms
{
    public sealed class FrmRun : NativeForm
    {
        #region Constructor(s)

        public FrmRun() : base(Program.Instance.RC, 0)
        {
            // do nothing
        }

        #endregion

        const int IdComboBox = 12298;

        string ComboBoxText
        {
            get
            {
                StringBuilder txt = new();
                GetDlgItemText(Handle, IdComboBox, txt, 255);
                return txt.ToString();
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_COMMAND:
                    {
                        int id = (int)m.WParam;
                        switch (id)
                        {
                            case 1: // OK
                                var cmd = ComboBoxText;
                                if (string.IsNullOrEmpty(cmd))
                                {
                                    MessageBox.Show($"Cannot find the file '{cmd}' (or one of its components). Make sure the path and filename are correct and that all required libraries are available.", cmd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    Process.Start(cmd);
                                }
                                break;
                            case 2: // Cancel
                                Close();
                                break;
                            case 12288: // Browse
                                {
                                    var ofd = new OpenFileDialog
                                    {
                                        Filter = string.Empty
                                    };

                                    if (ofd.ShowDialog() == DialogResult.OK)
                                    {
                                        SetDlgItemText(Handle, IdComboBox, ofd.FileName);
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }

            base.WndProc(ref m);
        }
    }
}