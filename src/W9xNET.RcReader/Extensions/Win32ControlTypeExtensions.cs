using W9xNET.RcReader.Enums;

namespace W9xNET.RcReader.Extensions
{
    internal static class Win32ControlTypeExtensions
    {
        /// <summary>
        /// Converts the Win32 element to a WinForms equivalent.
        /// </summary>
        /// <param name="type">The type of the Win32 control.</param>
        /// <returns></returns>
        internal static WinFormsControlType ToWinForms(this Win32ControlType type)
        {
            switch (type)
            {
                case Win32ControlType.Dialog: return WinFormsControlType.Form;
                case Win32ControlType.StaticControl: return WinFormsControlType.PictureBox;
                case Win32ControlType.Button: return WinFormsControlType.Button;
                case Win32ControlType.ComboBox: return WinFormsControlType.ComboBox;
                case Win32ControlType.DateAndTimePicker: return WinFormsControlType.DateTimePicker;
                case Win32ControlType.Edit: return WinFormsControlType.TextBox;
                case Win32ControlType.ScrollBar: return WinFormsControlType.ScrollBar;
                //case Win32ControlType.Unknown: return WinFormsControlType.HeaderControl;  
                case Win32ControlType.HotKey: return WinFormsControlType.HotKey;
                case Win32ControlType.ImageList: return WinFormsControlType.ImageList;
                case Win32ControlType.ListBox: return WinFormsControlType.ListBox;
                case Win32ControlType.ListView: return WinFormsControlType.ListView;
                case Win32ControlType.MonthCalendar: return WinFormsControlType.MonthCalendar;
                //case Win32ControlType.Unknown: return WinFormsControlType.Pager;
                case Win32ControlType.ProgressBar: return WinFormsControlType.ProgressBar;
                case Win32ControlType.PropertySheet: return WinFormsControlType.PropertyGrid;
                //case Win32ControlType.Unknown: return WinFormsControlType.Rebar;
                case Win32ControlType.RichEdit: return WinFormsControlType.RichTextBox;
                //case Win32ControlType.StaticControl: return WinFormsControlType.Label;
                case Win32ControlType.StatusBar: return WinFormsControlType.StatusStrip;
                case Win32ControlType.SysLink: return WinFormsControlType.LinkLabel;
                case Win32ControlType.Tab: return WinFormsControlType.TabControl;
                //case Win32ControlType.Unknown: return WinFormsControlType.TaskDialog;
                case Win32ControlType.Toolbar: return WinFormsControlType.ToolStrip;
                case Win32ControlType.Tooltip: return WinFormsControlType.Tooltip;
                case Win32ControlType.Trackbar: return WinFormsControlType.Trackbar;
                case Win32ControlType.TreeView: return WinFormsControlType.TreeView;
                case Win32ControlType.UpDownControl: return WinFormsControlType.NumericUpDown;
            }

            return WinFormsControlType.Unknown;
        }
    }
}
