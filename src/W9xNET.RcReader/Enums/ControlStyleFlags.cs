using System;
using System.Collections.Generic;
using DWORD = System.Int32;
using QWORD = System.Int64;

namespace W9xNET.RcReader.Enums
{
    [Flags]
    public enum StaticStyleFlags : QWORD
    {
        /*
         * Window Styles
         */
        WS_OVERLAPPED = WindowStyleFlags.WS_OVERLAPPED,
        WS_POPUP = WindowStyleFlags.WS_POPUP,
        WS_CHILD = WindowStyleFlags.WS_CHILD,
        WS_MINIMIZE = WindowStyleFlags.WS_MINIMIZE,
        WS_VISIBLE = WindowStyleFlags.WS_VISIBLE,
        WS_DISABLED = WindowStyleFlags.WS_DISABLED,
        WS_CLIPSIBLINGS = WindowStyleFlags.WS_CLIPSIBLINGS,
        WS_CLIPCHILDREN = WindowStyleFlags.WS_CLIPCHILDREN,
        WS_MAXIMIZE = WindowStyleFlags.WS_MAXIMIZE,
        WS_CAPTION = WindowStyleFlags.WS_CAPTION,
        WS_BORDER = WindowStyleFlags.WS_BORDER,
        WS_DLGFRAME = WindowStyleFlags.WS_DLGFRAME,
        WS_VSCROLL = WindowStyleFlags.WS_VSCROLL,
        WS_HSCROLL = WindowStyleFlags.WS_HSCROLL,
        WS_SYSMENU = WindowStyleFlags.WS_SYSMENU,
        WS_THICKFRAME = WindowStyleFlags.WS_THICKFRAME,
        WS_GROUP = WindowStyleFlags.WS_GROUP,
        WS_TABSTOP = WindowStyleFlags.WS_TABSTOP,

        /*
         * Static Control Constants
         */
        SS_LEFT = 0x00000000L,
        SS_CENTER = 0x00000001L,
        SS_RIGHT = 0x00000002L,
        SS_ICON = 0x00000003L,
        SS_BLACKRECT = 0x00000004L,
        SS_GRAYRECT = 0x00000005L,
        SS_WHITERECT = 0x00000006L,
        SS_BLACKFRAME = 0x00000007L,
        SS_GRAYFRAME = 0x00000008L,
        SS_WHITEFRAME = 0x00000009L,
        SS_USERITEM = 0x0000000AL,
        SS_SIMPLE = 0x0000000BL,
        SS_LEFTNOWORDWRAP = 0x0000000CL,
        SS_OWNERDRAW = 0x0000000DL,
        SS_BITMAP = 0x0000000EL,
        SS_ENHMETAFILE = 0x0000000FL,
        SS_ETCHEDHORZ = 0x00000010L,
        SS_ETCHEDVERT = 0x00000011L,
        SS_ETCHEDFRAME = 0x00000012L,
        SS_TYPEMASK = 0x0000001FL,
        SS_REALSIZECONTROL = 0x00000040L,
        SS_NOPREFIX = 0x00000080L, /* Don't do "&" character translation */
        SS_NOTIFY = 0x00000100L,
        SS_CENTERIMAGE = 0x00000200L,
        SS_RIGHTJUST = 0x00000400L,
        SS_REALSIZEIMAGE = 0x00000800L,
        SS_SUNKEN = 0x00001000L,
        SS_EDITCONTROL = 0x00002000L,
        SS_ENDELLIPSIS = 0x00004000L,
        SS_PATHELLIPSIS = 0x00008000L,
        SS_WORDELLIPSIS = 0x0000C000L,
        SS_ELLIPSISMASK = 0x0000C000L,
    }

    [Flags]
    public enum ButtonStyleFlags : QWORD
    {
        /*
         * Window Styles
         */
        WS_OVERLAPPED = WindowStyleFlags.WS_OVERLAPPED,
        WS_POPUP = WindowStyleFlags.WS_POPUP,
        WS_CHILD = WindowStyleFlags.WS_CHILD,
        WS_MINIMIZE = WindowStyleFlags.WS_MINIMIZE,
        WS_VISIBLE = WindowStyleFlags.WS_VISIBLE,
        WS_DISABLED = WindowStyleFlags.WS_DISABLED,
        WS_CLIPSIBLINGS = WindowStyleFlags.WS_CLIPSIBLINGS,
        WS_CLIPCHILDREN = WindowStyleFlags.WS_CLIPCHILDREN,
        WS_MAXIMIZE = WindowStyleFlags.WS_MAXIMIZE,
        WS_CAPTION = WindowStyleFlags.WS_CAPTION,
        WS_BORDER = WindowStyleFlags.WS_BORDER,
        WS_DLGFRAME = WindowStyleFlags.WS_DLGFRAME,
        WS_VSCROLL = WindowStyleFlags.WS_VSCROLL,
        WS_HSCROLL = WindowStyleFlags.WS_HSCROLL,
        WS_SYSMENU = WindowStyleFlags.WS_SYSMENU,
        WS_THICKFRAME = WindowStyleFlags.WS_THICKFRAME,
        WS_GROUP = WindowStyleFlags.WS_GROUP,
        WS_TABSTOP = WindowStyleFlags.WS_TABSTOP,

        /*
         * Button Control Styles
         */
        BS_PUSHBUTTON = 0x00000000L,
        BS_DEFPUSHBUTTON = 0x00000001L,
        BS_CHECKBOX = 0x00000002L,
        BS_AUTOCHECKBOX = 0x00000003L,
        BS_RADIOBUTTON = 0x00000004L,
        BS_3STATE = 0x00000005L,
        BS_AUTO3STATE = 0x00000006L,
        BS_GROUPBOX = 0x00000007L,
        BS_USERBUTTON = 0x00000008L,
        BS_AUTORADIOBUTTON = 0x00000009L,
        BS_PUSHBOX = 0x0000000AL,
        BS_OWNERDRAW = 0x0000000BL,
        BS_TYPEMASK = 0x0000000FL,
        BS_LEFTTEXT = 0x00000020L,
        BS_TEXT = 0x00000000L,
        BS_ICON = 0x00000040L,
        BS_BITMAP = 0x00000080L,
        BS_LEFT = 0x00000100L,
        BS_RIGHT = 0x00000200L,
        BS_CENTER = 0x00000300L,
        BS_TOP = 0x00000400L,
        BS_BOTTOM = 0x00000800L,
        BS_VCENTER = 0x00000C00L,
        BS_PUSHLIKE = 0x00001000L,
        BS_MULTILINE = 0x00002000L,
        BS_NOTIFY = 0x00004000L,
        BS_FLAT = 0x00008000L,
        BS_RIGHTBUTTON = BS_LEFTTEXT,
    }

    [Flags]
    public enum EditStyleFlags : QWORD
    {
        /*
         * Window Styles
         */
        WS_OVERLAPPED = WindowStyleFlags.WS_OVERLAPPED,
        WS_POPUP = WindowStyleFlags.WS_POPUP,
        WS_CHILD = WindowStyleFlags.WS_CHILD,
        WS_MINIMIZE = WindowStyleFlags.WS_MINIMIZE,
        WS_VISIBLE = WindowStyleFlags.WS_VISIBLE,
        WS_DISABLED = WindowStyleFlags.WS_DISABLED,
        WS_CLIPSIBLINGS = WindowStyleFlags.WS_CLIPSIBLINGS,
        WS_CLIPCHILDREN = WindowStyleFlags.WS_CLIPCHILDREN,
        WS_MAXIMIZE = WindowStyleFlags.WS_MAXIMIZE,
        WS_CAPTION = WindowStyleFlags.WS_CAPTION,
        WS_BORDER = WindowStyleFlags.WS_BORDER,
        WS_DLGFRAME = WindowStyleFlags.WS_DLGFRAME,
        WS_VSCROLL = WindowStyleFlags.WS_VSCROLL,
        WS_HSCROLL = WindowStyleFlags.WS_HSCROLL,
        WS_SYSMENU = WindowStyleFlags.WS_SYSMENU,
        WS_THICKFRAME = WindowStyleFlags.WS_THICKFRAME,
        WS_GROUP = WindowStyleFlags.WS_GROUP,
        WS_TABSTOP = WindowStyleFlags.WS_TABSTOP,

        /*
         * Edit Control Styles
         */
        ES_LEFT = 0x0000L,
        ES_CENTER = 0x0001L,
        ES_RIGHT = 0x0002L,
        ES_MULTILINE = 0x0004L,
        ES_UPPERCASE = 0x0008L,
        ES_LOWERCASE = 0x0010L,
        ES_PASSWORD = 0x0020L,
        ES_AUTOVSCROLL = 0x0040L,
        ES_AUTOHSCROLL = 0x0080L,
        ES_NOHIDESEL = 0x0100L,
        ES_OEMCONVERT = 0x0400L,
        ES_READONLY = 0x0800L,
        ES_WANTRETURN = 0x1000L,
        ES_NUMBER = 0x2000L,
    }

    [Flags]
    public enum TabControlStyleFlags : QWORD
    {
        /*
         * Window Styles
         */
        WS_OVERLAPPED = WindowStyleFlags.WS_OVERLAPPED,
        WS_POPUP = WindowStyleFlags.WS_POPUP,
        WS_CHILD = WindowStyleFlags.WS_CHILD,
        WS_MINIMIZE = WindowStyleFlags.WS_MINIMIZE,
        WS_VISIBLE = WindowStyleFlags.WS_VISIBLE,
        WS_DISABLED = WindowStyleFlags.WS_DISABLED,
        WS_CLIPSIBLINGS = WindowStyleFlags.WS_CLIPSIBLINGS,
        WS_CLIPCHILDREN = WindowStyleFlags.WS_CLIPCHILDREN,
        WS_MAXIMIZE = WindowStyleFlags.WS_MAXIMIZE,
        WS_CAPTION = WindowStyleFlags.WS_CAPTION,
        WS_BORDER = WindowStyleFlags.WS_BORDER,
        WS_DLGFRAME = WindowStyleFlags.WS_DLGFRAME,
        WS_VSCROLL = WindowStyleFlags.WS_VSCROLL,
        WS_HSCROLL = WindowStyleFlags.WS_HSCROLL,
        WS_SYSMENU = WindowStyleFlags.WS_SYSMENU,
        WS_THICKFRAME = WindowStyleFlags.WS_THICKFRAME,
        WS_GROUP = WindowStyleFlags.WS_GROUP,
        WS_TABSTOP = WindowStyleFlags.WS_TABSTOP,

        /*
         * Tab Control Styles
         */
        TCS_SCROLLOPPOSITE = 0x0001,
        TCS_BOTTOM = 0x0002,
        TCS_RIGHT = 0x0002,
        TCS_MULTISELECT = 0x0004,
        TCS_FLATBUTTONS = 0x0008,
        TCS_FORCEICONLEFT = 0x0010,
        TCS_FORCELABELLEFT = 0x0020,
        TCS_HOTTRACK = 0x0040,
        TCS_VERTICAL = 0x0080,
        TCS_TABS = 0x0000,
        TCS_BUTTONS = 0x0100,
        TCS_SINGLELINE = 0x0000,
        TCS_MULTILINE = 0x0200,
        TCS_RIGHTJUSTIFY = 0x0000,
        TCS_FIXEDWIDTH = 0x0400,
        TCS_RAGGEDRIGHT = 0x0800,
        TCS_FOCUSONBUTTONDOWN = 0x1000,
        TCS_OWNERDRAWFIXED = 0x2000,
        TCS_TOOLTIPS = 0x4000,
        TCS_FOCUSNEVER = 0x8000,
    }

    public static class DialogStyleFlagsExtension
    {
        public static WindowStyleFlags[] ToArray(this WindowStyleFlags dsf)
        {
            var flags = new List<WindowStyleFlags>();
            foreach (WindowStyleFlags i in Enum.GetValues(typeof(WindowStyleFlags)))
            {
                if ((dsf & i) != 0) flags.Add(i);
            }

            return flags.ToArray();
        }

        //public static ControlStyleFlags[] ToArray(this ControlStyleFlags dsf)
        //{
        //    var flags = new List<ControlStyleFlags>();
        //    foreach (ControlStyleFlags i in Enum.GetValues(typeof(ControlStyleFlags)))
        //    {
        //        if ((dsf & i) != 0) flags.Add(i);
        //    }

        //    return flags.ToArray();
        //}
    }
}