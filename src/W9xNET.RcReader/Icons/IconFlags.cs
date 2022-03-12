namespace W9xNET.RcReader.Icons
{
    public enum IconFlags : int
    {
        Icon = 0x000000100,     // get icon
        LinkOverlay = 0x000008000,     // put a link overlay on icon
        Selected = 0x000010000,     // show icon in selected state
        LargeIcon = 0x000000000,     // get large icon
        SmallIcon = 0x000000001,     // get small icon
        OpenIcon = 0x000000002,     // get open icon
        ShellIconSize = 0x000000004,     // get shell size icon
    }
}