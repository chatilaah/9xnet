using System.Diagnostics;

namespace W9xNET.Explorer.Models
{
    internal class FileSystemModel
    {
        /// <summary>
        /// Refreshes the Explorer window to display Control Panel applets.
        /// </summary>
        /// <param name="listView"></param>
        /// <returns>The number of objects displayed</returns>
        internal static int Populate(IExplorer iec)
        {
            iec.TitlebarText = iec.GetPath();
            //iec.TitlebarIcon = W9xNET.Shell32.Assets.Icons.Idi_5;

            iec.ListView.LargeImageList = new ImageList
            {
                ImageSize = new Size(32, 32)
            };
            iec.ListView.SmallImageList = new ImageList
            {
                ImageSize = new System.Drawing.Size(16, 16)
            };

            iec.ListView.LargeImageList.Images.Add("dir", Shell32.Program.Instance.RC!.IconAt(3));
            iec.ListView.LargeImageList.Images.Add("file", Shell32.Program.Instance.RC!.IconAt(0));

            iec.ListView.SmallImageList.Images.Add("dir", Shell32.Program.Instance.RC!.IconAt(3));
            iec.ListView.SmallImageList.Images.Add("file", Shell32.Program.Instance.RC!.IconAt(0));

            var files = Directory.GetFileSystemEntries(iec.GetPath(), "*", SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                FileInfo info = new(file);

                if ((info.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden) continue;
                if ((info.Attributes & FileAttributes.System) == FileAttributes.System) continue;

                var lvItem = new ListViewItem
                {
                    Text = info.Name,
                    Tag = file,
                    ImageKey = ((info.Attributes & FileAttributes.Directory) == FileAttributes.Directory) ? "dir" : "file"
                };

                iec.ListView.Items.Add(lvItem);
            }

            return 0;
        }
    }
}