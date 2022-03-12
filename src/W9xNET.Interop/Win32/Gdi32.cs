using System.Runtime.InteropServices;

namespace W9xNET.Interop.Win32
{
    public static class Gdi32
    {
        /// <summary>
        /// The name of the DLL file that is installed on the primary Windows partition.
        /// </summary>
        public const string Gdi32Dll = "gdi32.dll";

        [DllImport(Gdi32Dll)]
        public static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        [DllImport(Gdi32Dll)]
        public static extern IntPtr CreateFont(int nHeight, int nWidth, int nEscapement,
            int nOrientation, int fnWeight, uint fdwItalic, uint fdwUnderline, uint fdwStrikeOut, uint fdwCharSet, uint fdwOutputPrecision, uint fdwClipPrecision, uint fdwQuality, uint fdwPitchAndFamily, string lpszFace);
    }
}