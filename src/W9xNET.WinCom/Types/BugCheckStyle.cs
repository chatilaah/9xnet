namespace W9xNET.WinCom.Types
{
    /// <summary>
    /// BugCheck Style enumeration.
    /// </summary>
    internal enum BugCheckStyle
    {
        /// <summary>
        /// Uses the Windows 3.1, Windows 95, Windows 98, and Windows Me bugcheck style.
        /// </summary>
        Win9xStyle,

        /// <summary>
        /// Uses the Windows 2000, Windows XP, Windows Vista, and Windows 7 bugcheck style.
        /// </summary>
        Win2kStyle,

        /// <summary>
        /// Uses the Windows 8, Windows 8.1, and Windows 10 bugcheck style.
        /// </summary>
        Win8Style,
    }
}