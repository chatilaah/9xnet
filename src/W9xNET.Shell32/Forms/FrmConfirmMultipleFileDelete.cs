using LegalMitigation.Forms;

namespace W9xNET.Shell32.Forms
{
    public sealed class FrmConfirmMultipleFileDelete : NativeForm
    {
        #region Constructor(s)

        public FrmConfirmMultipleFileDelete() : base(Program.Instance.RC!, 6)
        {
            //AssignHandler(6, BtnYes_Click);
            //AssignHandler(7, BtnNo_Click);
        }

        #endregion

        private void BtnNo_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnYes_Click(object sender, EventArgs e)
        {

        }
    }
}