using System;
using System.Drawing;
using System.Windows.Forms;
using W9xNET.Explorer.Types;

namespace W9xNET.Explorer.Models
{
    internal class ControlPanelModel
    {
        public Icon? DisplayIcon { get; set; }

        public ControlPanelItemType CplItemType { get; set; }

        public string? DisplayName { get; set; }


        /// <summary>
        /// Refreshes the Explorer window to display Control Panel applets.
        /// </summary>
        /// <param name="listView"></param>
        /// <returns>The number of objects displayed</returns>
        internal static int Populate(IExplorer iec)
        {
            iec.TitlebarText = Properties.Resources.ControlPanel;
            //iec.TitlebarIcon = W9xNET.Shell32.Assets.Icons.Idi_137;

            iec.ListView.Clear();
            iec.ListView.LargeImageList = new ImageList
            {
                ImageSize = new Size(32, 32)
            };
            iec.ListView.SmallImageList = new ImageList
            {
                ImageSize = new System.Drawing.Size(16, 16)
            };

            //
            // cmStrip
            //
            var cmStrip = new ContextMenuStrip
            {
                RenderMode = ToolStripRenderMode.System
            };

            //
            // cmStrip::openToolStripMenuItem
            //
            ToolStripMenuItem openToolStripMenuItem = new();
            openToolStripMenuItem.Text = "&Open";
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Click += delegate (object? sender, EventArgs e)
            {
                throw new NotImplementedException();
            };

            //
            // cmStrip::createShortcutToolStripMenuItem 
            //
            ToolStripMenuItem createShortcutToolStripMenuItem = new();
            createShortcutToolStripMenuItem.Text = "Create &Shortcut";
            createShortcutToolStripMenuItem.Name = "createShortcutToolStripMenuItem";
            createShortcutToolStripMenuItem.Click += delegate (object? sender, EventArgs e)
            {
                throw new NotImplementedException();
            };

            cmStrip.Items.AddRange(new ToolStripItem[] {
                openToolStripMenuItem,
                new System.Windows.Forms.ToolStripSeparator(),
                createShortcutToolStripMenuItem
            });

            iec.ContextMenuOnItem = cmStrip;

            foreach (ControlPanelItemType i in Enum.GetValues(typeof(ControlPanelItemType)))
            {
                var cplItem = i.GetCpl();

                iec.ListView.LargeImageList.Images.Add(cplItem.DisplayName, cplItem.DisplayIcon);
                iec.ListView.SmallImageList.Images.Add(cplItem.DisplayName, cplItem.DisplayIcon);
                
                var lvItem = new ListViewItem
                {
                    ImageKey = cplItem.DisplayName,
                    Text = cplItem.DisplayName,
                    Tag = (int)i
                };

                iec.ListView.DoubleClick += delegate (object? _sender, EventArgs e)
                {
                    if (iec.ListView.SelectedItems.Count == 0)
                    {
                        return;
                    }

                    cplItem = ((ControlPanelItemType)iec.ListView.SelectedItems[0].Tag).GetCpl();
                    iec.ListView.SelectedItems.Clear();

                    cplItem.FormEntry.ShowDialog();
                };

                iec.ListView.Items.Add(lvItem);
            }

            return iec.ListView.Items.Count;
        }
    }
}