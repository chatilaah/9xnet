using System.Diagnostics;
using W9xNET.User32.Controls;

namespace W9xNET.Explorer.Controls
{
    internal class MSTaskSwWClass : NxnControl
    {
        public TaskbarWindowEntryCollection Items { get; } = new();

        internal const int SpaceBetweenTabs = 3;

        /// <summary>
        /// Gets the number of visible rows
        /// </summary>
        public int Rows => Height / ShellTrayWnd.DefaultChildHeight;

        /// <summary>
        /// Measures the width of the items if they were to be displayed on a single row.
        /// It is up to the developer to determine how much should be deducted if it were to be distributed among more than one row(s)
        /// </summary>
        private int MeasureWidthOfItems => Items.Count * TaskbarWindowEntry.MaxWidth;

        public MSTaskSwWClass()
        {
            Height = ShellTrayWnd.DefaultChildHeight;
        }

        /// <summary>
        /// Arranges the TaskbarWindowEntry collection objects to their appropriate coordinates.
        /// </summary>
        /// <param name="r">The current working row</param>
        /// <param name="nTweWidth">The new width to be used for the TaskbarWindowEntry object</param>
        /// <param name="curr">A reference to the current index</param>
        /// <param name="start">The starting index of the Items array</param>
        /// <param name="end">The ending index of the Items array</param>
        private void ArrangeSingleRow(int r, int nTweWidth, ref int curr, int start, int end)
        {
            int top = (r * TaskbarWindowEntry.MaxHeight) + (r * SpaceBetweenTabs);
            int left = 0;

            for (int i = start; i < end; i++)
            {
                if (curr >= Items.Count)
                {
                    break;
                }

                var item = Items[curr];

                Controls.Add(item);
                item.Top = top;
                item.Left = left;
                item.Width = nTweWidth;
                item.Click += delegate (object? sender, EventArgs e)
                {
                    Items.Remove(item);
                    ArrangeTweBars();
                };

                left += item.Width + SpaceBetweenTabs;
                curr++;
            }
        }

        private void ArrangeTweBars()
        {
            if (Items.Count == 0) return;

            // Get the avaiable row count
            int rows = Rows;

            int width = Width - (Items.Count * SpaceBetweenTabs);

            // Get the estimated Twe width on a single row (rows = 1)
            int tweWidth = (width / Items.Count);

            // Get the initial slots per row
            // On 640x480, the number of items that can be placed is 2
            decimal iSlotsInitial = width / TaskbarWindowEntry.MaxWidth;
            decimal iSlotsActual = width / tweWidth;

            decimal itemsOnRow = iSlotsInitial;

            // We're always defaulting to the initial width of the Twe item
            int nTweWidth = TaskbarWindowEntry.MaxWidth;
            if (iSlotsActual > iSlotsInitial * rows)
            {
                nTweWidth = tweWidth;
                if (nTweWidth != TaskbarWindowEntry.MaxWidth)
                {
                    itemsOnRow = Math.Ceiling((decimal)(iSlotsActual / rows));
                    nTweWidth = width / (int)itemsOnRow;

                    if (nTweWidth < TaskbarWindowEntry.MinWidth)
                    {
                        nTweWidth = TaskbarWindowEntry.MinWidth;
                    }
                }
            }

            int curr = 0;
            for (int r = 0; r < rows; r++)
            {
                ArrangeSingleRow(
                    r,
                    nTweWidth,
                    ref curr,
                    0,
                    (int)itemsOnRow);
            }
        }

        public new void Refresh()
        {
            // Upon creation of the ShellTrayWnd, we don't want to run anything here yet!
            if (Parent == null) return;

            Controls.Clear();

            ArrangeTweBars();
        }
    }
}