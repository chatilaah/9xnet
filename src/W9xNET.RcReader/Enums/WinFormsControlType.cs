namespace W9xNET.RcReader.Enums
{
    internal enum WinFormsControlType
    {
        /// <summary>
        /// The control is undefined or unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// Dialog -> Form
        /// Window -> Form
        /// </summary>
        Form,

        /// <summary>
        /// Static -> PictureBox
        /// </summary>
        PictureBox,
        
        /// <summary>
        /// Button -> Button
        /// </summary>
        Button,

        /// <summary>
        /// ComboBox -> ComboBox
        /// </summary>
        ComboBox,

        /// <summary>
        /// DateTimePicker -> DateTimePicker
        /// </summary>
        DateTimePicker,

        /// <summary>
        /// Edit -> TextBox
        /// </summary>
        TextBox,

        /// <summary>
        /// ScrollBar -> ScrollBar
        /// FlatScrollBar -> ScrollBar
        /// </summary>
        ScrollBar,

        HeaderControl,

        HotKey,

        /// <summary>
        /// ImageList -> ImageList
        /// </summary>
        ImageList,

        /// <summary>
        /// ListBox -> ListBox
        /// </summary>
        ListBox,

        /// <summary>
        /// ListView -> ListView
        /// </summary>
        ListView,

        /// <summary>
        /// MonthCalendar -> MonthCalendar
        /// </summary>
        MonthCalendar,

        Pager,

        /// <summary>
        /// ProgressBar -> ProgressBar
        /// </summary>
        ProgressBar,

        /// <summary>
        /// PropertySheet -> PropertyGrid
        /// </summary>
        PropertyGrid,

        Rebar,

        /// <summary>
        /// RichBox -> RichTextBox
        /// </summary>
        RichTextBox,

        /// <summary>
        /// Static -> Label
        /// </summary>
        Label,

        /// <summary>
        /// StatusBar -> StatusStrip
        /// </summary>
        StatusStrip,

        /// <summary>
        /// SysLink -> LinkLabel
        /// </summary>
        LinkLabel,

        /// <summary>
        /// Tab -> TabControl
        /// </summary>
        TabControl,

        TaskDialog,
        
        /// <summary>
        /// Toolbar -> ToolStrip
        /// </summary>
        ToolStrip,

        /// <summary>
        /// Tooltip -> Tooltip
        /// </summary>
        Tooltip,

        /// <summary>
        /// Trackbar -> Trackbar
        /// </summary>
        Trackbar,

        /// <summary>
        /// TreeView -> TreeView
        /// </summary>
        TreeView,

        /// <summary>
        /// UpDownControl -> NumericUpDown
        /// </summary>
        NumericUpDown
    }
}
