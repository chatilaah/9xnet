namespace W9xNET.RcReader.Enums
{
    /// <summary>
    /// Control Types of a Win32 user-interface element.
    /// </summary>
    public enum Win32ControlType
    {
        /// <summary>
        /// (Custom) The control is undefined or unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// (Custom) The control is a dialog or window.
        /// </summary>
        Dialog,

        /// <summary>
        /// (Custom) The control is a bitmap.
        /// </summary>
        Bitmap,

        /// <summary>
        /// (Custom)) The control is a checkbox.
        /// </summary>
        CheckBox,

        /// <summary>
        /// (Custom) The control is a Blackframe.
        /// </summary>
        BlackFrame,

        /// <summary>
        /// An animation control is a window that displays an Audio-Video Interleaved (AVI) clip. 
        /// This section contains information about the programming elements used with animation controls.
        /// </summary>
        Animation,

        /// <summary>
        /// A button is a control the user can click to provide input to an application. 
        /// This section contains information about the programming elements used with button controls.
        /// </summary>
        Button,

        /// <summary>
        /// A combo box consists of a list and a selection field. 
        /// The list presents the options that a user can select, and the selection field displays the current selection. 
        /// This section contains information about the programming elements used with combo box controls.
        /// </summary>
        ComboBox,

        /// <summary>
        /// A ComboBoxEx control is a combo box control that provides native support for item images. 
        /// This section contains information about the programming elements used with ComboBoxEx controls.
        /// </summary>
        ComboBoxEx,

        /// <summary>
        /// A date and time picker provides a simple and intuitive interface through which to exchange date and time information with a user. 
        /// This section contains information about the programming elements used with date and time picker controls.
        /// </summary>
        DateAndTimePicker,

        /// <summary>
        /// A date and time picker provides a simple and intuitive interface through which to exchange date and time information with a user. 
        /// This section contains information about the programming elements used with date and time picker controls.
        /// </summary>
        Edit,

        /// <summary>
        /// A date and time picker provides a simple and intuitive interface through which to exchange date and time information with a user. 
        /// This section contains information about the programming elements used with date and time picker controls.
        /// </summary>
        FlatScrollBar,

        /// <summary>
        /// A header control is a window that is usually positioned above columns of text or numbers. 
        /// It contains a title for each column, and it can be divided into parts.
        /// The user can drag the dividers that separate the parts to set the width of each column. 
        /// This section contains information about the programming elements used with header controls.
        /// </summary>
        HeaderControl,

        /// <summary>
        /// A hot key control is a window that enables the user to enter a combination of keystrokes to be used as a hot key. 
        /// This section contains information about the programming elements used with hot keys.
        /// </summary>
        HotKey,

        /// <summary>
        /// An image list is a collection of images of the same size, each of which can be referred to by its index. 
        /// This section contains information about the programming elements used with image lists.
        /// </summary>
        ImageList,

        /// <summary>
        /// An IP address control allows the user to enter an Internet Protocol (IP) address in an easily understood format. 
        /// This section contains information about the programming elements used with IP address controls.
        /// </summary>
        IpAddressControl,

        /// <summary>
        /// A list box is a control window that contains a simple list of items from which the user can choose. 
        /// This section contains information about the programming elements used with list boxes.
        /// </summary>
        ListBox,

        /// <summary>
        /// A list-view control is a window that displays a collection of items.
        /// List-view controls provide several ways to arrange and display items and are much more flexible than list boxes. 
        /// This section contains information about the programming elements used with list views.
        /// </summary>
        ListView,

        /// <summary>
        /// A month calendar control implements a calendar-like user interface that provides the user with an intuitive and recognizable method of entering or selecting a date.
        /// This section contains information about the programming elements used with month calendar controls.
        /// </summary>
        MonthCalendar,

        /// <summary>
        /// A pager control is a window container that is used with a window that does not have enough display area to show all of its content. 
        /// This section contains information about the programming elements used with pager controls.
        /// </summary>
        Pager,

        /// <summary>
        /// A progress bar is a window that an application can use to indicate the progress of a lengthy operation. 
        /// This section contains information about the programming elements used with progress bars.
        /// </summary>
        ProgressBar,

        /// <summary>
        /// A property sheet is a window that allows the user to view and edit the properties of an item. 
        /// This section contains information about the programming elements used with property sheets.
        /// </summary>
        PropertySheet,

        /// <summary>
        /// A rebar control acts as a container for child windows. 
        /// This section contains information about the programming elements used with rebar controls.
        /// </summary>
        Rebar,

        /// <summary>
        /// A rich edit control enables the user to enter, edit, print, and save text. 
        /// The text can be assigned character and paragraph formatting, and can include embedded Component Object Model (COM) objects. 
        /// This section contains information about the programming elements used with rich edit controls.
        /// </summary>
        RichEdit,

        /// <summary>
        /// A scroll bar allows the user to bring into view the portions of an object that extend beyond the borders of a window. 
        /// This section contains information about the programming elements used with scroll bars.
        /// </summary>
        ScrollBar,

        /// <summary>
        /// A static control provides the user with informational text and graphics that typically require no response. 
        /// This section contains information about the programming elements used with static controls.
        /// </summary>
        StaticControl,

        /// <summary>
        /// A status bar is a horizontal window at the bottom of a parent window in which an application can display various kinds of status information. 
        /// This section contains information about the programming elements used with status bars.
        /// </summary>
        StatusBar,

        /// <summary>
        /// A SysLink control is a window that renders marked-up text and notifies the application when the user clicks an embedded link. 
        /// This section contains information about the programming elements used with SysLink controls.
        /// </summary>
        SysLink,

        /// <summary>
        /// A tab control is analogous to the dividers in a notebook or the labels in a file cabinet. 
        /// By using a tab control, an application can define multiple pages for the same area of a window or dialog box. 
        /// This section contains information about the programming elements used with tab controls.
        /// </summary>
        Tab,

        /// <summary>
        /// A task dialog is a dialog box that can be used to display information and receive simple input from the user. 
        /// Like a message box, it is formatted by the operating system according to parameters you set. 
        /// However, a task dialog has many more features than a message box. This section contains information about the programming elements used with task dialogs.
        /// </summary>
        TaskDialog,

        /// <summary>
        /// A task dialog is a dialog box that can be used to display information and receive simple input from the user. 
        /// Like a message box, it is formatted by the operating system according to parameters you set. 
        /// However, a task dialog has many more features than a message box. 
        /// This section contains information about the programming elements used with task dialogs.
        /// </summary>
        Toolbar,

        /// <summary>
        /// A tooltip is a small window that appears automatically, or pops up, when the user pauses the mouse pointer over a tool or some other UI element. 
        /// This section contains information about the programming elements used with tooltips.
        /// </summary>
        Tooltip,

        /// <summary>
        /// A trackbar is a window that contains a slider (sometimes called a thumb) in a channel, and optional tick marks. 
        /// When the user moves the slider, using either the mouse or the direction keys, the trackbar sends notification messages to indicate the change. 
        /// This section contains information about the programming elements used with trackbars.
        /// </summary>
        Trackbar,

        /// <summary>
        /// A tree-view control is a window that displays a hierarchical list of items, such as the headings in a document, the entries in an index, or the files and directories on a disk. 
        /// This section contains information about the programming elements used with tree-view controls.
        /// </summary>
        TreeView,

        /// <summary>
        /// An up-down control is a pair of arrow buttons that the user can click to increment or decrement a value, such as a scroll position or a number displayed in a companion control (called a buddy window).
        /// This section contains information about the programming elements used with up-down controls.
        /// </summary>
        UpDownControl
    }
}