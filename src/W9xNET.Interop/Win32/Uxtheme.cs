using System.Runtime.InteropServices;

namespace W9xNET.Interop.Win32
{
    public static class Uxtheme
    {
        /// <summary>
        /// The name of the DLL file that is installed on the primary Windows partition.
        /// </summary>
        public const string UxthemeDll = "uxtheme.dll";


        [DllImport(UxthemeDll, ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);
    }
}