using LegalMitigation.Forms;
using static W9xNET.Interop.Win32.User32;

namespace W9xNET.Explorer.Dialogs
{
    public sealed class DlgStartMenuPrograms : NativeForm
    {
        public DlgStartMenuPrograms() : base(Program.Instance.RC, 3)
        {
            InitHwnd(650, 6);
            InitHwnd(652, 7);
        }

        private IntPtr InitHwnd(int id, int iconId)
        {
            IntPtr hwnd = GetDlgItem(Handle, id);
            SendMessage(hwnd, STM_SETIMAGE, (IntPtr)LoadImageTypes.IMAGE_ICON, Program.Instance.RC!.IconAt(iconId).Handle);
            return hwnd;
        }
    }
}