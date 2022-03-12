using LegalMitigation.Forms;
using System.Runtime.InteropServices;
using static W9xNET.Interop.Win32.User32;

namespace W9xNET.Explorer.Dialogs
{
    public sealed class DlgTaskbarOptions : NativeForm
    {
        /// <summary>
        /// Desktop view
        /// </summary>
        IntPtr hwnd1111;

        /// <summary>
        /// Start menu
        /// </summary>
        IntPtr hwnd1112;

        /// <summary>
        /// Taskbar 1
        /// </summary>
        IntPtr hwnd1113;

        /// <summary>
        /// Clock only
        /// </summary>
        IntPtr hwnd1115;

        /// <summary>
        /// Region cropped window
        /// </summary>
        IntPtr hwnd1114;

        bool _alwaysOnTop;
        bool _autoHide;
        bool _startMenuSmallIcons;
        bool _showClock;

        public bool IsAlwaysOnTop
        {
            get => _alwaysOnTop;
            private set
            {
                var hwnd = hwnd1114;

                if (_alwaysOnTop = value)
                {
                    ShowWindow(hwnd, SwCommand.SW_SHOW);
                    CheckDlgButton(Handle, IdAlwaysOnTop, BST_CHECKED);
                }
                else
                {
                    ShowWindow(hwnd, SwCommand.SW_HIDE);
                    CheckDlgButton(Handle, IdAlwaysOnTop, BST_UNCHECKED);
                }
            }
        }

        public bool IsAutoHide
        {
            get => _autoHide;
            private set
            {
                var hwnd = hwnd1113;

                if (_autoHide = value)
                {
                    ShowWindow(hwnd, SwCommand.SW_HIDE);
                    CheckDlgButton(Handle, IdAutoHide, BST_CHECKED);
                }
                else
                {
                    ShowWindow(hwnd, SwCommand.SW_SHOW);
                    CheckDlgButton(Handle, IdAutoHide, BST_UNCHECKED);
                }
            }
        }

        public bool IsStartMenuSmallIcons
        {
            get => _startMenuSmallIcons;
            private set
            {
                var hwnd = hwnd1112;
                
                if (_startMenuSmallIcons = value)
                {
                    ShowWindow(hwnd, SwCommand.SW_HIDE);
                    CheckDlgButton(Handle, IdShowSmallIcons, BST_CHECKED);
                }
                else
                {
                    ShowWindow(hwnd, SwCommand.SW_SHOW);
                    CheckDlgButton(Handle, IdShowSmallIcons, BST_UNCHECKED);
                }
            }
        }

        public bool IsShowClock
        {
            get => _showClock;
            private set
            {
                var hwnd = hwnd1115;
                if (_showClock = value)
                {
                    ShowWindow(hwnd, SwCommand.SW_HIDE);
                    CheckDlgButton(Handle, IdShowClock, BST_CHECKED);
                }
                else
                {
                    ShowWindow(hwnd, SwCommand.SW_SHOW);
                    CheckDlgButton(Handle, IdShowClock, BST_UNCHECKED);
                }
            }
        }

        public DlgTaskbarOptions() : base(Program.Instance.RC, 0)
        {
            hwnd1111 = InitHwnd(1111, 3 /* 149 */, false, false);
            hwnd1112 = InitHwnd(1112, 4 /* 150 */);
            hwnd1113 = InitHwnd(1113, 5 /* 151 */, false);
            hwnd1115 = InitHwnd(1115, 7 /*  153 */);
            hwnd1114 = InitHwnd(1114, 6 /* 152 */);

            IsAlwaysOnTop = true;
            IsAutoHide = false;
            IsStartMenuSmallIcons = false;
            IsShowClock = true;
        }

        private IntPtr InitHwnd(int id, int bmpId, bool hide = true, bool topmost = true)
        {
            IntPtr hwnd = GetDlgItem(Handle, id);
            SendMessage(hwnd, STM_SETIMAGE, (IntPtr)LoadImageTypes.IMAGE_BITMAP, Program.Instance.RC!.BmpAt(bmpId).GetHbitmap());
            ShowWindow(hwnd, hide ? SwCommand.SW_HIDE : SwCommand.SW_SHOW);
            if (topmost)
            {
                BringWindowToTop(hwnd);
            }
            return hwnd;
        }

        const int IdAlwaysOnTop = 1101;
        const int IdAutoHide = 1102;
        const int IdShowSmallIcons = 1130;
        const int IdShowClock = 1103;

        protected override void WndProc(ref Message m)
        {
            // Listen for messages that are sent to the button window. Some messages are sent
            // to the parent window instead of the button's window.

            switch (m.Msg)
            {
                case WM_COMMAND:
                    {
                        int id = (int)m.WParam;
                        var isChecked = IsDlgButtonChecked(Handle, id);

                        switch (id)
                        {
                            case IdAlwaysOnTop: // Always on Top
                                IsAlwaysOnTop = isChecked == 1;
                                break;

                            case IdAutoHide: // Auto hide
                                IsAutoHide = isChecked == 1;
                                break;

                            case IdShowSmallIcons: // Show small icons 
                                IsStartMenuSmallIcons = isChecked == 1;
                                break;

                            case IdShowClock: // Show clock
                                IsShowClock = isChecked == 1;
                                break;
                        }
                    }
                    break;

                    //case WM_ACTIVATEAPP:
                    //    // Do something here in response to messages
                    //    break;
            }

            base.WndProc(ref m);
        }
    }
}