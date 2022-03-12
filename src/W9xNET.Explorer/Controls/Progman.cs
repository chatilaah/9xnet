using System.Diagnostics;
using W9xNET.Explorer.Forms;
using W9xNET.Rundll32;
using W9xNET.User32.Controls;

namespace W9xNET.Explorer.Controls
{
    internal class Progman : IconContainer<DesktopHost>
    {
        public Progman()
        {
            LoadSystemIcons();
            InitContextMenu();
        }

        private void ContextMenu_OnClickedContextMenuItem(object? sender, EventArgs e)
        {
            var cmi = ((ContextMenuItem)sender!);

            switch (Convert.ToInt32(cmi.Name))
            {
                case 0: // Properties
                    break;
                case 28721: // Auto arrange
                    break;
                case 28722: // Line up Icons
                    LineupIcons();
                    break;
                case 28698: // Paste
                    break;
                case 28700: // Paste Shortcut
                    break;
                case 28699: // Undo
                    break;
            }
        }

        public void InvalidateRect()
        {
            this.Size = new Size(
                Parent.ClientSize.Width,
                Parent.ClientSize.Height - Parent.Taskbar.Height);
        }

        /// <summary>
        /// Loads the core system icons and arranges them.
        /// </summary>
        private void LoadSystemIcons()
        {
            Items.Clear();

            var shell32Rc = Shell32.Program.Instance.RC;
            Debug.Assert(shell32Rc != null, "shell32Rc is null");

            var explorerRc = Explorer.Program.Instance.RC;
            Debug.Assert(explorerRc != null, "explorerRc is null");

            Items.AddRange(new IconElement[]
            {
                new IconElement(this, explorerRc.IconAt(0), "My Computer", PathConstants.ExplorerPath),
                //new IconElement(this, shell32Rc.IconAt(17), "Network Neighborhood"),
                //new IconElement(this, shell32Rc.IconAt(31), "Recycle Bin"),
                //new IconElement(this, shell32Rc.IconAt(3), "Online Services")
            });

            LineupIcons();
        }

        private void InitContextMenu()
        {
            var cm1 = new ContextMenu(false, 9, W9xNET.Shell32.Program.Instance.RC);
            var cm2 = new ContextMenu(false, 12, W9xNET.Shell32.Program.Instance.RC);
            var cm3 = new ContextMenu(false, 13, W9xNET.Shell32.Program.Instance.RC);

            ContextMenu = new();

            int index = -1;
            foreach (var i in cm1.Items[0].Items)
            {
                index++;
                if (index == 0 || index == 1)
                {
                    continue;
                }

                ContextMenu.Items.Add(i);
            }

            ContextMenuItem cmi = ContextMenu.Items[0].Items[0];
            ContextMenu.Items[0].Items.RemoveAt(0);

            foreach (var i in cm3.Items[1].Items)
            {
                ContextMenu.Items[0].Items.Add(i.Text);
            }

            ContextMenu.Items[0].Items.AddSeparator();
            ContextMenu.Items[0].Items.Add(cmi);

            ContextMenu.Items.AddSeparator();

            foreach (var i in cm2.Items[0].Items)
            {
                ContextMenu.Items.Add(i);
            }

            ContextMenu.OnClickedContextMenuItem += ContextMenu_OnClickedContextMenuItem;
            ContextMenu.Reload();
        }
    }
}