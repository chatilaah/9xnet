using LegalMitigation.Forms;

namespace W9xNET.Shell32.Forms
{
    public sealed class FrmShutdown : NativeForm
    {
        #region Constructor(s)

        public FrmShutdown() : base(Program.Instance.RC, 26)
        {
            StartPosition = FormStartPosition.CenterScreen;
        }

        #endregion
    }
}
