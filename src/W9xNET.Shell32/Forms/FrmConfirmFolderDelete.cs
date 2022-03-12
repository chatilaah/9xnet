using LegalMitigation.Forms;

namespace W9xNET.Shell32.Forms
{
    public sealed class FrmConfirmFolderDelete : NativeForm
    {
        #region Constructor(s)

        public FrmConfirmFolderDelete() : base(Program.Instance.RC!, 5)
        {
            //AssignHandler(6, BtnYes_Click);
            //AssignHandler(12807, BtnYesToAll_Click);
            //AssignHandler(7, BtnNo_Click);
            //AssignHandler(2, BtnCancel_Click);
        }

        #endregion

        private void BtnYes_Click(object sender, EventArgs e)
        {

        }

        private void BtnYesToAll_Click(object sender, EventArgs e)
        {

        }

        private void BtnNo_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
