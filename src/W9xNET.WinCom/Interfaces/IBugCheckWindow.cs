using System.Drawing;

namespace W9xNET.WinCom.Interfaces
{
    /// <summary>
    /// Interface to the BugCheck window.
    /// </summary>
    internal interface IBugCheckWindow
    {
        /// <summary>
        /// The back color of the BugCheck screen.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// The fore color of the BugCheck's display text. 
        /// </summary>
        Color TextColor { get; set; }

        /// <summary>
        /// Error message to be presented to the user.
        /// </summary>
        string Message { get; }
    }
}