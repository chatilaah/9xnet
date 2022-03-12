using System.Drawing;
using System.Windows.Forms;

namespace W9xNET.Explorer
{
    /// <summary>
    /// Interface to the Explorer window.
    /// </summary>
    internal interface IExplorer
    {
        /// <summary>
        /// Form's title bar icon.
        /// </summary>
        Icon TitlebarIcon { get; set; }

        /// <summary>
        /// Form's title bar text.
        /// </summary>
        string TitlebarText { get; set; }

        /// <summary>
        /// Form's attached ListView container.
        /// </summary>
        ListView ListView { get; }

        /// <summary>
        /// Handle to the context menu.
        /// </summary>
        ContextMenuStrip ContextMenuOnItem { set; }

        /// <summary>
        /// Navigates to a specific path.
        /// </summary>
        void Navigate(string path, bool newWindow = true);

        /// <summary>
        /// Reloads the existing path.
        /// </summary>
        int Reload();

        /// <summary>
        /// Gets the current path.
        /// </summary>
        /// <returns></returns>
        string GetPath();
    }
}