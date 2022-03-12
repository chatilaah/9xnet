using System.Windows.Forms;

namespace W9xNET.User32.Utils
{
    /// <summary>
    /// TabControl helper functions
    /// </summary>
    public static class TabControlHelpers
    {
        /// <summary>
        /// Padding number on all sides.
        /// </summary>
        const int TabPagePadding = 7;

        /// <summary>
        /// Sets the tab page with all the necessary properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tp"></param>
        /// <param name="destroy"></param>
        public static void SetTabPage<T>(TabPage tp, bool destroy = false) 
            where T : UserControl, new()
        {
            if (destroy)
            {
                tp.Dispose();
                return;
            }

            tp.Padding = new Padding(TabPagePadding);
            tp.Controls.Add(new T
            {
                Dock = DockStyle.Fill
            });
        }
    }
}