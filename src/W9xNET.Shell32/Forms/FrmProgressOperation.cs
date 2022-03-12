using LegalMitigation.Forms;

namespace W9xNET.Shell32.Forms
{
    public sealed class FrmProgressOperation : NativeForm
    {
        #region Constructor(s)

        public FrmProgressOperation() : base(Program.Instance.RC!, 13)
        {
            //AssignHandler(2, BtnCancel_Click);
        }

        #endregion

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}