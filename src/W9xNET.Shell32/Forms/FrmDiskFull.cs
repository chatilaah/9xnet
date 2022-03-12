using LegalMitigation.Forms;

namespace W9xNET.Shell32.Forms
{
    public sealed class FrmDiskFull : NativeForm
    {
        #region Constructor(s)

        public FrmDiskFull() : base(Program.Instance.RC!, 2)
        {
            //AssignHandler(12816, BtnOpen_Click);
            //AssignHandler(12817, BtnEmpty_Click);
            //AssignHandler(2, BtnCancel_Click);
        }

        #endregion
        
        private void BtnEmpty_Click(object sender, EventArgs e)
        {

        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
