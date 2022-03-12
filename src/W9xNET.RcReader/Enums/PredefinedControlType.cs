using System;

namespace W9xNET.RcReader.Enums
{
    public enum PredefinedControlType
    {
        Unknown,
        Animation,
        Bitmap,
        Button,
        Checkbox,
        Combobox,
        ComboboxEx,
        Edit,
        Groupbox,
        Header,
        Hotkey,
        Icon,
        IPAddress,
        Link,
        Listbox,
        ListView,
        LText,
        Memo,
        Pager,
        ProgressBar,
        RadioButton,
        ReBar,
        RichEdit,
        RichEdit20W,
        RichEdit50W,
        ScrollBar,
        Static,
        StatusBar,
        TabControl,
        Toolbar,
        Trackbar,
        TreeView,
        UpDown,
        DateTimePicker,
        MonthCalendar
    }

    internal static class PredefinedControlsExtensions
    {
        public const string SysAnimate32 = "SysAnimate32";
        public const string STATIC = "STATIC";
        public const string BUTTON = "BUTTON";
        public const string COMBOBOX = "COMBOBOX";
        public const string ComboBoxEx32 = "ComboBoxEx32";
        public const string EDIT = "EDIT";
        public const string SysHeader32 = "SysHeader32";
        public const string msctls_hotkey32 = "msctls_hotkey32";
        public const string SysIPAddress32 = "SysIPAddress32";
        public const string SysLink = "SysLink";
        public const string LISTBOX = "LISTBOX";
        public const string SysListView32 = "SysListView32";
        public const string SysPager = "SysPager";
        public const string msctls_progress32 = "msctls_progress32";
        public const string ReBarWindow32 = "ReBarWindow32";
        public const string RICHEDIT = "RICHEDIT";
        public const string RichEdit20W = "RichEdit20W";
        public const string RichEdit50W = "RichEdit50W";
        public const string SCROLLBAR = "SCROLLBAR";
        public const string msctls_statusbar32 = "msctls_statusbar32";
        public const string SysTabControl32 = "SysTabControl32";
        public const string ToolbarWindow32 = "ToolbarWindow32";
        public const string msctls_trackbar32 = "msctls_trackbar32";
        public const string SysTreeView32 = "SysTreeView32";
        public const string msctls_updown32 = "msctls_updown32";
        public const string SysDateTimePick32 = "SysDateTimePick32";
        public const string SysMonthCal32 = "SysMonthCal32";

        public static string ToClassName(this PredefinedControlType pdc)
        {
            switch (pdc)
            {
                case PredefinedControlType.Animation: return SysAnimate32;
                case PredefinedControlType.Icon:
                case PredefinedControlType.Bitmap:
                case PredefinedControlType.LText:
                case PredefinedControlType.Static:
                    return STATIC;
                case PredefinedControlType.Button:
                case PredefinedControlType.Checkbox:
                case PredefinedControlType.Groupbox:
                case PredefinedControlType.RadioButton:
                    return BUTTON;
                case PredefinedControlType.Combobox: return COMBOBOX;
                case PredefinedControlType.ComboboxEx: return ComboBoxEx32;
                case PredefinedControlType.Edit:
                case PredefinedControlType.Memo:
                    return EDIT;
                case PredefinedControlType.Header: return SysHeader32;
                case PredefinedControlType.Hotkey: return msctls_hotkey32;
                case PredefinedControlType.IPAddress: return SysIPAddress32;
                case PredefinedControlType.Link: return SysLink;
                case PredefinedControlType.Listbox: return LISTBOX;
                case PredefinedControlType.ListView: return SysListView32;
                case PredefinedControlType.Pager: return SysPager;
                case PredefinedControlType.ProgressBar: return msctls_progress32;
                case PredefinedControlType.ReBar: return ReBarWindow32;
                case PredefinedControlType.RichEdit: return RICHEDIT;
                case PredefinedControlType.RichEdit20W: return RichEdit20W;
                case PredefinedControlType.RichEdit50W: return RichEdit50W;
                case PredefinedControlType.ScrollBar: return SCROLLBAR;
                case PredefinedControlType.StatusBar: return msctls_statusbar32;
                case PredefinedControlType.TabControl: return SysTabControl32;
                case PredefinedControlType.Toolbar: return ToolbarWindow32;
                case PredefinedControlType.Trackbar: return msctls_trackbar32;
                case PredefinedControlType.TreeView: return SysTreeView32;
                case PredefinedControlType.UpDown: return msctls_updown32;
                case PredefinedControlType.DateTimePicker: return SysDateTimePick32;
                case PredefinedControlType.MonthCalendar: return SysMonthCal32;
            }

            return string.Empty;
        }

        private static bool IsMatch(string input, string cmp) => string.Compare(input, cmp, StringComparison.OrdinalIgnoreCase) == 0;

        public static PredefinedControlType FromClassName(string input)
        {
            //
            // msctlsX
            //
            if (IsMatch(input, msctls_hotkey32))
            {
                return PredefinedControlType.Hotkey;
            }
            else if (IsMatch(input, msctls_progress32))
            {
                return PredefinedControlType.ProgressBar;
            }
            else if (IsMatch(input, msctls_statusbar32))
            {
                return PredefinedControlType.StatusBar;
            }
            else if (IsMatch(input, msctls_trackbar32))
            {
                return PredefinedControlType.Trackbar;
            }
            else if (IsMatch(input, msctls_updown32))
            {
                return PredefinedControlType.UpDown;
            }

            //
            // SysX
            //
            else if (IsMatch(input, SysMonthCal32))
            {
                return PredefinedControlType.MonthCalendar;
            }
            else if (IsMatch(input, SysDateTimePick32))
            {
                return PredefinedControlType.DateTimePicker;
            }
            else if (IsMatch(input, SysHeader32))
            {
                return PredefinedControlType.Header;
            }
            else if (IsMatch(input, SysIPAddress32))
            {
                return PredefinedControlType.IPAddress;
            }
            else if (IsMatch(input, SysLink))
            {
                return PredefinedControlType.Link;
            }
            else if (IsMatch(input, SysListView32))
            {
                return PredefinedControlType.ListView;
            }
            else if (IsMatch(input, SysAnimate32))
            {
                return PredefinedControlType.Animation;
            }
            else if (IsMatch(input, SysTabControl32))
            {
                return PredefinedControlType.TabControl;
            }

            return PredefinedControlType.Unknown;
        }
    }
}