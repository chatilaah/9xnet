using System.Runtime.InteropServices;

namespace W9xNET.Interop.Win32
{
    public static class Cabinet
    {
        public const string CabinetDll = "cabinet.dll";

        #region Delegate(s)

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr FdiMemAllocDelegate(int numBytes);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FdiMemFreeDelegate(IntPtr mem);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr FdiFileOpenDelegate(string fileName, int oflag, int pmode);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 FdiFileReadDelegate(IntPtr hf,
                                                   [In, Out] [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2,
                                                       ArraySubType = UnmanagedType.U1)] byte[] buffer, int cb);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 FdiFileWriteDelegate(IntPtr hf,
                                                    [In] [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2,
                                                        ArraySubType = UnmanagedType.U1)] byte[] buffer, int cb);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 FdiFileCloseDelegate(IntPtr hf);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 FdiFileSeekDelegate(IntPtr hf, int dist, int seektype);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr FdiNotifyDelegate(
            FdiNotificationType fdint, [In][MarshalAs(UnmanagedType.LPStruct)] FdiNotification fdin);

        #endregion

        #region Enum(s)

        public enum FdiNotificationType
        {
            CabinetInfo,
            PartialFile,
            CopyFile,
            CloseFileInfo,
            NextCabinet,
            Enumerate
        }

        #endregion

        #region Structure(s)

        [StructLayout(LayoutKind.Sequential)]
        public class FdiNotification //Cabinet API: "FDINOTIFICATION"
        {
            public int cb;
            //not sure if this should be a IntPtr or a strong
            public IntPtr psz1;
            public IntPtr psz2;
            public IntPtr psz3;
            public IntPtr pv;
            public IntPtr hf;
            public short date;
            public short time;
            public short attribs;
            public short setID;
            public short iCabinet;
            public short iFolder;
            public int fdie;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class CabError //Cabinet API: "ERF"
        {
            public int erfOper;
            public int erfType;
            public int fError;
        }

        #endregion

        [DllImport(CabinetDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FDICreate", CharSet = CharSet.Ansi)]
        public static extern IntPtr FdiCreate(
            FdiMemAllocDelegate fnMemAlloc,
            FdiMemFreeDelegate fnMemFree,
            FdiFileOpenDelegate fnFileOpen,
            FdiFileReadDelegate fnFileRead,
            FdiFileWriteDelegate fnFileWrite,
            FdiFileCloseDelegate fnFileClose,
            FdiFileSeekDelegate fnFileSeek,
            int cpuType,
            [MarshalAs(UnmanagedType.LPStruct)] CabError erf);

        [DllImport(CabinetDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FDIDestroy", CharSet = CharSet.Ansi)]
        public static extern bool FdiDestroy(IntPtr hfdi);

        [DllImport(CabinetDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FDICopy", CharSet = CharSet.Ansi)]
        public static extern bool FdiCopy(
            IntPtr hfdi,
            string cabinetName,
            string cabinetPath,
            int flags,
            FdiNotifyDelegate fnNotify,
            IntPtr fnDecrypt,
            IntPtr userData);
    }
}
