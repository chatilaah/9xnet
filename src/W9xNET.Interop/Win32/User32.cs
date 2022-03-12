using System.Runtime.InteropServices;
using System.Text;
using static W9xNET.Interop.Win32.Kernel32;
using BYTE = System.Byte;
using DWORD = System.Int32;
using WORD = System.Int16;

namespace W9xNET.Interop.Win32
{
    public static class User32
    {
        /// <summary>
        /// The name of the DLL file that is installed on the primary Windows partition.
        /// </summary>
        public const string User32Dll = "user32.dll";

        #region Enum(s)

        public enum LookupIconIdFromDirectoryExFlags : int
        {
            LR_DEFAULTCOLOR = 0,
            LR_MONOCHROME = 1
        }

        public enum LoadImageTypes : int
        {
            /// <summary>
            /// Load a bitmap
            /// </summary>
            IMAGE_BITMAP = 0,

            /// <summary>
            /// Load an icon
            /// </summary>
            IMAGE_ICON = 1,

            /// <summary>
            /// Load a cursor
            /// </summary>
            IMAGE_CURSOR = 2
        }

        public enum SwCommand : int
        {
            /// <summary>
            /// Hides the window
            /// </summary>
            SW_HIDE = 0,

            /// <summary>
            /// Shows the window
            /// </summary>
            SW_SHOW = 1
        }

        #endregion

        #region Structure(s)

        /// <summary>
        /// Presents an Icon Directory.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 6)]
        public struct IconDir
        {
            public WORD Reserved;   // Reserved (must be 0)
            public WORD Type;       // Resource Type (1 for icons)
            public WORD Count;      // How many images?

            /// <summary>
            /// Converts the current W9xNET.RcReader.IconDir into W9xNET.RcReader.GroupIconDir.
            /// </summary>
            /// <returns>W9xNET.RcReader.GroupIconDir</returns>
            public GroupIconDir ToGroupIconDir()
            {
                GroupIconDir grpDir = new();
                grpDir.Reserved = this.Reserved;
                grpDir.Type = this.Type;
                grpDir.Count = this.Count;
                return grpDir;
            }
        }

        /// <summary>
        /// Presents an Icon Directory Entry.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public struct IconDirEntry
        {
            public BYTE Width;          // Width, in pixels, of the image
            public BYTE Height;         // Height, in pixels, of the image
            public BYTE ColorCount;     // Number of colors in image (0 if >=8bpp)
            public BYTE Reserved;       // Reserved ( must be 0)
            public WORD Planes;         // Color Planes
            public WORD BitCount;       // Bits per pixel
            public DWORD BytesInRes;     // How many bytes in this resource?
            public DWORD ImageOffset;    // Where in the file is this image?

            /// <summary>
            /// Converts the current W9xNET.RcReader.IconDirEntry into W9xNET.RcReader.GroupIconDirEntry.
            /// </summary>
            /// <param name="id">The resource identifier.</param>
            /// <returns>W9xNET.RcReader.GroupIconDirEntry</returns>
            public GroupIconDirEntry ToGroupIconDirEntry(int id)
            {
                GroupIconDirEntry grpEntry = new GroupIconDirEntry
                {
                    Width = this.Width,
                    Height = this.Height,
                    ColorCount = this.ColorCount,
                    Reserved = this.Reserved,
                    Planes = this.Planes,
                    BitCount = this.BitCount,
                    BytesInRes = this.BytesInRes,
                    ID = (short)id
                };

                return grpEntry;
            }
        }

        /// <summary>
        /// Presents a Group Icon Directory.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 6)]
        public struct GroupIconDir
        {
            public WORD Reserved;   // Reserved (must be 0)
            public WORD Type;       // Resource Type (1 for icons)
            public WORD Count;      // How many images?

            /// <summary>
            /// Converts the current W9xNET.RcReader.GroupIconDir into W9xNET.RcReader.IconDir.
            /// </summary>
            /// <returns>W9xNET.RcReader.IconDir</returns>
            public IconDir ToIconDir()
            {
                IconDir dir = new IconDir
                {
                    Reserved = this.Reserved,
                    Type = this.Type,
                    Count = this.Count
                };

                return dir;
            }
        }

        /// <summary>
        /// Presents a Group Icon Directory Entry.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 14)]
        public struct GroupIconDirEntry
        {
            public BYTE Width;          // Width, in pixels, of the image
            public BYTE Height;         // Height, in pixels, of the image
            public BYTE ColorCount;     // Number of colors in image (0 if >=8bpp)
            public BYTE Reserved;       // Reserved ( must be 0)
            public WORD Planes;         // Color Planes
            public WORD BitCount;       // Bits per pixel
            public DWORD BytesInRes;    // How many bytes in this resource?
            public WORD ID;             // the ID

            /// <summary>
            /// Converts the current W9xNET.RcReader.GroupIconDirEntry into W9xNET.RcReader.IconDirEntry.
            /// </summary>
            /// <param name="id">The resource identifier.</param>
            /// <returns>W9xNET.RcReader.IconDirEntry</returns>
            public IconDirEntry ToIconDirEntry(int imageOffiset)
            {
                IconDirEntry entry = new IconDirEntry
                {
                    Width = this.Width,
                    Height = this.Height,
                    ColorCount = this.ColorCount,
                    Reserved = this.Reserved,
                    Planes = this.Planes,
                    BitCount = this.BitCount,
                    BytesInRes = this.BytesInRes,
                    ImageOffset = (DWORD)imageOffiset
                };

                return entry;
            }
        }

        /// <summary>
        /// Presents a Dialog's header information.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 18)]
        public struct DLGTEMPLATE
        {
            public readonly DWORD Style;
            public readonly DWORD DwExtendedStyle;
            public readonly WORD Cdit;
            public readonly short X;
            public readonly short Y;
            public readonly short Cx;
            public readonly short Cy;
        }

        [StructLayout(LayoutKind.Sequential, Size = 18)]
        public struct DLGITEMTEMPLATE
        {
            public readonly DWORD Style;
            public readonly DWORD DwExtendedStyle;
            public readonly short X;
            public readonly short Y;
            public readonly short Cx;
            public readonly short Cy;
            public readonly WORD ID;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct ExMenuItem
        {
            public DWORD DwType;     // MFT_
            public DWORD DwState;    // MFS_
            public DWORD MenuId;
            public WORD bResInfo;
            public string Text = ""; //TODO: Is this even correct?
            public DWORD DwHelpId;
            public WORD wDepth;
        };

        [StructLayout(LayoutKind.Sequential, Size = 14)]
        public struct MENUEX_TEMPLATE_ITEM_HEADER
        {
            public readonly DWORD dwType;         // MFT_
            public readonly DWORD dwState;        // MFS_
            public readonly DWORD menuId;
            public readonly WORD bResInfo;       // 0x80: is it last?, 0x01: popup
                                 //WCHAR szText[];
                                 //DWORD dwHelpId;       // only if popup
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct MenuItem
        {
            public WORD fItemFlags;
            public WORD MenuId; // wMenuID
            public WORD wDepth;
            public string Text = "";
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct MENUEX_TEMPLATE_HEADER
        {
            public readonly WORD wVersion;       // one
            public readonly WORD wOffset;        // offset to items from this structure
            public readonly DWORD dwHelpId;
        };

        [StructLayout(LayoutKind.Sequential, Size = 4)]
        public struct MENUHEADER
        {
            public readonly WORD wVersion;       // zero
            public readonly WORD cbHeaderSize;   // zero
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NORMALMENUITEMHEAD
        {
            public readonly WORD fItemFlags;      // MF_
            public readonly WORD wMenuID;
            //public readonly WCHAR  szItemText[];
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct POPUPMENUITEMHEAD
        {
            public readonly WORD fItemFlags;     // MF_
                                                 //WCHAR  szItemText[];
        };

        #endregion

        //
        // Checkbox check state
        //
        public const int BST_UNCHECKED = 0x0000;
        public const int BST_CHECKED = 0x0001;

        [DllImport(User32Dll, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport(User32Dll, SetLastError = true)]
        public static extern uint GetDlgItemText(IntPtr hDlg, int nIDDlgItem, [Out] StringBuilder lpString, int nMaxCount);

        [DllImport(User32Dll, EntryPoint = "SetDlgItemTextA", SetLastError = true)]
        public static extern bool SetDlgItemText(IntPtr hDlg, int id, string text);

        [DllImport(User32Dll, SetLastError = true)]
        public static extern bool CheckDlgButton(IntPtr hDlg, int nIDButton, int uCheck);

        [DllImport(User32Dll)]
        public static extern uint IsDlgButtonChecked(IntPtr hDlg, int nIDButton);

        [DllImport(User32Dll, SetLastError = true, ExactSpelling = true)]
        public static extern int LookupIconIdFromDirectory(IntPtr presbits, bool fIcon);

        [DllImport(User32Dll, SetLastError = true, ExactSpelling = true)]
        public static extern int LookupIconIdFromDirectoryEx(IntPtr presbits, bool fIcon, int cxDesired, int cyDesired, LookupIconIdFromDirectoryExFlags Flags);

        [DllImport(User32Dll, EntryPoint = "LoadImageW", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr LoadImage(IntPtr hInstance, IntPtr lpszName, LoadImageTypes imageType, int cxDesired, int cyDesired, uint fuLoad);

        [DllImport(User32Dll, SetLastError = true, EntryPoint = "CreateWindowEx")]
        public static extern IntPtr CreateWindowEx(
            int dwExStyle,
            //UInt16 regResult,
            string lpClassName,
            string lpWindowName,
            Int32 dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam);

        [DllImport(User32Dll, EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport(User32Dll, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport(User32Dll)]
        public static extern unsafe IntPtr CreateDialogIndirectParamA(
            IntPtr hInstance,
            DLGTEMPLATE* lpTemplate,
            IntPtr hWndParent,
            Dlgproc lpDialogFunc,
            IntPtr lParamInit);

        [DllImport(User32Dll, SetLastError = true)]
        public static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport(User32Dll, SetLastError = true)]
        public static extern long SetWindowPos(IntPtr hwnd, long hWndInsertAfter, long x, long y, long cx, long cy, long wFlags);

        [DllImport(User32Dll, SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

        [DllImport(User32Dll)]
        public static extern IntPtr SendDlgItemMessage(IntPtr hDlg, int nIDDlgItem, uint Msg, UIntPtr wParam, IntPtr lParam);

        [DllImport(User32Dll, SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport(User32Dll, SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, SwCommand iShowCmd);

        [DllImport(User32Dll, SetLastError = true)]
        public static extern IntPtr GetDlgItem(IntPtr hWnd, int nIDDlgItem);
    }
}