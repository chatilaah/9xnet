using W9xNET.Shell32.Utils;
using W9xNET.User32.Utils;

namespace W9xNET.Explorer.Models
{
    internal class MyComputerModel
    {
        /// <summary>
        /// Refreshes the Explorer window to display My Computer applets.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="listView"></param>
        /// <returns></returns>
        internal static int Populate(IExplorer iec)
        {
            iec.TitlebarText = Properties.Resources.MyComputer;
            iec.TitlebarIcon = Program.Instance.DisplayIcon!;

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
            // Setup the image list
            //

            foreach (DriveType i in Enum.GetValues(typeof(DriveType)))
            {
                iec.ListView.LargeImageList.Images.Add(i.ToString(), i.ToIcon());
                iec.ListView.SmallImageList.Images.Add(i.ToString(), i.ToIcon());
            }

            var drives = DriveInfo.GetDrives();

            foreach (var drive in drives)
            {
                var lvItem = new ListViewItem
                {
                    ImageKey = drive.DriveType.ToString(),
                    Text = drive.ClassicalName(),
                    Tag = drive
                };

                iec.ListView.Items.Add(lvItem);
            }

            return 0;
        }
    }
}