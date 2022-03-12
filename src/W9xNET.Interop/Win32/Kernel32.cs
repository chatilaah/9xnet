using System.Runtime.InteropServices;
using System.Text;

namespace W9xNET.Interop.Win32
{
    public static class Kernel32
    {
        public const string Kernel32Dll = "kernel32.dll";

        #region Delegate(s)

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Auto)]
        public delegate bool EnumResNameProc(IntPtr hModule, ResourceTypes lpszType, IntPtr lpszName, IntPtr lParam);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Auto)]
        public delegate bool Dlgproc(IntPtr unnamedParam1, int unnamedParam2, IntPtr unnamedParam3, IntPtr unnamedParam4);

        #endregion

        #region Enum(s)

        public enum ResourceTypes : int
        {
            RT_ACCELERATOR = 9,
            RT_ANICURSOR = 21,
            RT_ANIICON = 22,
            RT_BITMAP = 2,
            RT_CURSOR = 1,
            RT_DIALOG = 5,
            RT_DLGINCLUDE = 5,
            RT_FONT = 8,
            RT_FONTDIR = 7,
            RT_GROUP_ICON = 14,
            RT_HTML = 23,
            RT_ICON = 3,
            RT_MANIFEST = 24,
            RT_MENU = 4,
            RT_MESSAGETABLE = 11,
            RT_PLUGPLAY = 19,
            RT_RCDATA = 10,
            RT_STRING = 6,
            RT_VERSION = 16,
            RT_VXD = 20
        }

        [Flags]
        public enum LoadLibraryExFlags : int
        {
            DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
            LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
            LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008
        }

        public enum GetLastErrorResult : int
        {
            ERROR_SUCCESS = 0,
            ERROR_FILE_NOT_FOUND = 2,
            ERROR_BAD_EXE_FORMAT = 193,
            ERROR_RESOURCE_TYPE_NOT_FOUND = 1813
        }

        #endregion


        [DllImport(Kernel32Dll, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport(Kernel32Dll, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, LoadLibraryExFlags dwFlags);

        [DllImport(Kernel32Dll, SetLastError = true, ExactSpelling = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport(Kernel32Dll, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);

        [DllImport(Kernel32Dll, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool EnumResourceNames(IntPtr hModule, ResourceTypes lpszType, EnumResNameProc lpEnumFunc, IntPtr lParam);

        [DllImport(Kernel32Dll, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, ResourceTypes lpType);

        [DllImport(Kernel32Dll, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport(Kernel32Dll, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr LockResource(IntPtr hResData);

        [DllImport(Kernel32Dll, SetLastError = true, ExactSpelling = true)]
        public static extern int SizeofResource(IntPtr hModule, IntPtr hResInfo);
    }
}