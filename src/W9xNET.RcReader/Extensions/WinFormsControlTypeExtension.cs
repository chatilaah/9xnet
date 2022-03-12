using W9xNET.RcReader.Enums;

namespace W9xNET.RcReader.Extensions
{
    internal static class WinFormsControlTypeExtension
    {
        /// <summary>
        /// Converts the WinForms element to a Win32 equivalent.
        /// </summary>
        /// <param name="type">The type of the WinForms control.</param>
        /// <returns></returns>
        internal static Win32ControlType ToWin32(this WinFormsControlType type)
        {
            switch (type)
            {
                case WinFormsControlType.Form: return Win32ControlType.Dialog;
                case WinFormsControlType.PictureBox: return Win32ControlType.StaticControl;
                case WinFormsControlType.Button: return Win32ControlType.Button;
                case WinFormsControlType.ComboBox: return Win32ControlType.ComboBox;
                case WinFormsControlType.DateTimePicker: return Win32ControlType.DateAndTimePicker;
                case WinFormsControlType.TextBox: return Win32ControlType.Edit;
                case WinFormsControlType.ScrollBar: return Win32ControlType.ScrollBar;
                case WinFormsControlType.HeaderControl: return Win32ControlType.Unknown;
                case WinFormsControlType.HotKey: return Win32ControlType.HotKey;
                case WinFormsControlType.ImageList: return Win32ControlType.ImageList;
                case WinFormsControlType.ListBox: return Win32ControlType.ListBox;
                case WinFormsControlType.ListView: return Win32ControlType.ListView;
                case WinFormsControlType.MonthCalendar: return Win32ControlType.MonthCalendar;
                case WinFormsControlType.Pager: return Win32ControlType.Unknown;
                case WinFormsControlType.ProgressBar: return Win32ControlType.ProgressBar;
                case WinFormsControlType.PropertyGrid: return Win32ControlType.PropertySheet;
                case WinFormsControlType.Rebar: return Win32ControlType.Unknown;
                case WinFormsControlType.RichTextBox: return Win32ControlType.RichEdit;
                case WinFormsControlType.Label: return Win32ControlType.StaticControl;
                case WinFormsControlType.StatusStrip: return Win32ControlType.StatusBar;
                case WinFormsControlType.LinkLabel: return Win32ControlType.SysLink;
                case WinFormsControlType.TabControl: return Win32ControlType.Tab;
                case WinFormsControlType.TaskDialog: return Win32ControlType.Unknown;
                case WinFormsControlType.ToolStrip: return Win32ControlType.Toolbar;
                case WinFormsControlType.Tooltip: return Win32ControlType.Tooltip;
                case WinFormsControlType.Trackbar: return Win32ControlType.Trackbar;
                case WinFormsControlType.TreeView: return Win32ControlType.TreeView;
                case WinFormsControlType.NumericUpDown: return Win32ControlType.UpDownControl;
            }

            return Win32ControlType.Unknown;
        }
    }
}
