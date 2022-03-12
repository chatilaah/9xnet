using LegalMitigation.Forms;

namespace W9xNET.Shell32.Forms
{
    public sealed class FrmMissingShortcut : NativeForm
    {
        #region Constructor(s)

        public FrmMissingShortcut() : base(Program.Instance.RC!, 1)
        {
            //AssignHandler(12288, BtnBrowse_Click);
            //AssignHandler(2, BtnCancel_Click);

            StartPosition = FormStartPosition.CenterScreen;
        }

        #endregion

        private void BtnBrowse_Click(object sender, EventArgs args)
        {
            var ofd = new OpenFileDialog
            {
                Filter = ""
            };

            ofd.ShowDialog();
        }

        private void BtnCancel_Click(object sender, EventArgs args)
        {
            Close();
        }
    }
}