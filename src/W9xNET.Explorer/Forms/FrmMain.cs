using LegalMitigation.Forms;
using System.Diagnostics;
using W9xNET.Explorer.Models;

namespace W9xNET.Explorer.Forms
{
    internal partial class FrmMain : LegalForm, IExplorer
    {
        #region Properties

        private ContextMenuStrip _cmOnItem;
        public readonly string Path = "";
        private readonly Form? PrevForm;

        #endregion

        #region Constructor(s)

        public FrmMain()
        {
            InitializeComponent();
            
            CommonInit();
        }

        public FrmMain(params string[] args)
        {
            InitializeComponent();

            if (args.Length > 0)
            {
                Path = args[0];
            }

            CommonInit();
        }

        public FrmMain(Form prev, string path)
        {
            InitializeComponent();

            PrevForm = prev;
            Path = path;

            CommonInit();
        }

        #endregion

        private void CommonInit()
        {
            //
            // menuStrip1 & contextMenuStrip1
            //
            largeIconsToolStripMenuItem.Tag = largeIconsToolStripMenuItem1.Tag = (int)global::System.Windows.Forms.View.LargeIcon;
            smallIconsToolStripMenuItem.Tag = smallIconsToolStripMenuItem1.Tag = (int)global::System.Windows.Forms.View.SmallIcon;
            detailsToolStripMenuItem.Tag = detailsToolStripMenuItem1.Tag = (int)global::System.Windows.Forms.View.Details;
            listToolStripMenuItem.Tag = listToolStripMenuItem1.Tag = (int)global::System.Windows.Forms.View.List;
            //
            // listView1
            //
            ViewMode = View.LargeIcon;

            Reload();
        }

        View ViewMode
        {
            get { return listView1.View; }
            set
            {
                largeIconsToolStripMenuItem.Checked = largeIconsToolStripMenuItem1.Checked = false;
                detailsToolStripMenuItem.Checked = detailsToolStripMenuItem1.Checked = false;
                smallIconsToolStripMenuItem.Checked = smallIconsToolStripMenuItem1.Checked = false;
                listToolStripMenuItem.Checked = listToolStripMenuItem1.Checked = false;

                switch (value)
                {
                    case View.LargeIcon:
                        largeIconsToolStripMenuItem.Checked = largeIconsToolStripMenuItem1.Checked = true;
                        break;
                    case View.Details:
                        detailsToolStripMenuItem.Checked = detailsToolStripMenuItem1.Checked = true;
                        break;
                    case View.SmallIcon:
                        smallIconsToolStripMenuItem.Checked = smallIconsToolStripMenuItem1.Checked = true;
                        break;
                    case View.List:
                        listToolStripMenuItem.Checked = listToolStripMenuItem1.Checked = true;
                        break;
                    case View.Tile:
                        break;
                }

                listView1.View = value;
            }
        }

        #region Implementation for IExplorerContainer

        public Icon TitlebarIcon { get => Icon; set => Icon = value ?? SystemIcons.WinLogo; }

        public string TitlebarText { get => Text; set => Text = value; }

        public ListView ListView => listView1;

        public ContextMenuStrip ContextMenuOnItem
        {
            set
            {
                _cmOnItem = value;
                _cmOnItem.Closed += delegate (object? sender, ToolStripDropDownClosedEventArgs e)
                {
                    // Immediately revert back to the default context menu.
                    listView1.ContextMenuStrip = contextMenuStrip1;
                };

            }
        }

        #endregion

        #region Menu Bar

        #region File

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Edit

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listView1.Items)
            {
                i.Selected = true;
            }
        }

        private void invertSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listView1.Items)
            {
                i.Selected = !i.Selected;
            }
        }

        #endregion

        #region View

        private void iconViewTypeStripMenuItem_Click(object? sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem;
            ViewMode = (View)tsmi.Tag;
        }

        #endregion

        #region Help

        private void helpTopicsToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException("Help contents are not available yet... But here's the BSOD of W9xNET anyway :-)");
        }

        private void aboutW9xNETToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new W9xNET.Shell32.Forms.FrmAbout(this).ShowDialog();
        }

        #endregion

        #endregion

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (_cmOnItem == null)
            {
                listView1.ContextMenuStrip = contextMenuStrip1;
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                var focusedItem = listView1.FocusedItem;
                if (focusedItem != null && focusedItem.Bounds.Contains(e.Location))
                {
                    listView1.ContextMenuStrip = _cmOnItem;
                }
            }
        }


        public int Reload()
        {
            if (string.IsNullOrEmpty(Path))
            {
                goto MyComputer;
            }

            if (Directory.Exists(Path))
            {
                goto FileSystem;
            }

            goto MyComputer;

        FileSystem:
            return FileSystemModel.Populate(this);

        MyComputer:
            return MyComputerModel.Populate(this);

        ControlPanel:
            return ControlPanelModel.Populate(this);
        }

        public void Navigate(string path, bool newWindow = true) => new FrmMain(this, path).Show();

        public string GetPath() => Path;

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem i in listView1.SelectedItems)
            {
                var file = new FileInfo($"{i.Tag}");
                if (!file.Attributes.HasFlag(FileAttributes.Directory))
                {
                    var p = new Process();
                    p.StartInfo = new ProcessStartInfo(file.FullName)
                    {
                        UseShellExecute = true
                    };
                    p.Start();
                }
                else
                {
                    Navigate($"{i.Tag}");
                }
            }
        }
    }
}