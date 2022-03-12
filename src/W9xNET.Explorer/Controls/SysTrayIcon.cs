using W9xNET.User32.Controls;

namespace W9xNET.Explorer.Controls
{
    internal class SysTrayIcon : NxnControl
    {
        /// <summary>
        /// The display icon of the tray item
        /// </summary>
        public Icon Icon { get; set; }

        /// <summary>
        /// The display text of the tray item
        /// </summary>
        public new string Text { get => base.Text; set => base.Text = value; }
    }
}