using LegalMitigation.Forms;

namespace LegalMitigation
{
    public static class StartupHelper
    {
        public static bool ResolveFromGuiMode(params W9xNETApp[] apps)
        {
            return new FrmLoadSource(apps).ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }
    }
}